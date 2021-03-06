using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF_SOCKET_CLIENT
{
    internal class LFDevice
    {
        private static SocketIO socketClient;

        private string deviceSerialNumber = null;

        private static List<UsbDeviceModel> usbDevices = new List<UsbDeviceModel>();
        private static List<string> socketList = new List<string>();
        private static string mode;
        private string selectedServiceSocketId = null;

        public delegate void connectionStatusDel(string msg);
        public delegate void onSocketErrorDel(string e);
        public delegate void onSocketConnectedDel(bool status, List<string> socketList, string msg);
        public delegate void updateUsbDeviceListDel(bool status, List<UsbDeviceModel> usbDevices, string msg);
        public delegate void connectDeviceUsbDel(bool status, string msg);
        public delegate void connectDeviceEthDel(bool status, string msg);
        public delegate void disconnectDeviceUsbDel(bool status, string msg);
        public delegate void disconnectDeviceEthDel(bool status, string msg);
        public delegate void startScanDel(bool status, string msg);
        public delegate void stopScanDel(bool status, string msg);
        public delegate void addTagsDel(string str);
        public delegate void scanStartedDel(string str);
        public delegate void scanCompletedDel(bool status, string msg, List<string> tags);
        public delegate void scanErrorDel(string str);
        public delegate void deviceStatusDel(string str);
        public delegate void ledOnDel(bool status, string msg);
        public delegate void ledOffDel(bool status, string msg);
        public delegate void deviceRefreshDel(bool status, List<UsbDeviceModel> list, string msg);
        public delegate void refreshTagDel(bool status, string msg);

        public connectionStatusDel connectionStatusDelegate;
        public onSocketErrorDel onSocketErrorDelegate;
        public onSocketConnectedDel onSocketConnectedDelegate;
        public updateUsbDeviceListDel updateUsbDeviceListDelegate;
        public connectDeviceUsbDel connectDeviceUsbDelegate;
        public connectDeviceEthDel connectDeviceEthDelegate;
        public disconnectDeviceUsbDel disconnectDeviceUsbDelegate;
        public disconnectDeviceEthDel disconnectDeviceEthDelegate;
        public startScanDel startScanDelegate;
        public stopScanDel stopScanDelegate;
        public addTagsDel addTagsDelegate;
        public scanStartedDel scanStartedDelegate;
        public scanCompletedDel scanCompletedDelegate;
        public scanErrorDel scanErrorDelegate;
        public deviceStatusDel deviceStatusDelegate;
        public ledOnDel ledOnDelegate;
        public ledOffDel ledOffDelegate;
        public deviceRefreshDel deviceRefreshDelegate;
        public refreshTagDel refreshTagDelegate;

        // this function is to stablish the connection with spacecode
        public async Task<bool> init()
        {
            socketClient = new SocketIO(ConfigurationManager.AppSettings["baseUrl"], new SocketIOOptions
            {
                Query = new Dictionary<string, string>
                {
                    {"token", "v3" }
                },
                EIO = 4,
            });
            await socketClient.ConnectAsync();
            socketClient.OnConnected += onSocketConnected;
            socketClient.OnError += onSocketError;

            // making a connection with services
            await socketClient.EmitAsync("connection", response =>
            {
                Console.WriteLine(response.ToString());
            }, new
            {
                deviceType = "client"
            });

            listener();
            return true;
        }

        // these are the listeners for the event calling from the socket
        private void listener()
        {
            // when scan is getting started
            socketClient.On("receive_scanStarted", response =>
            {
                Console.WriteLine("receive_scanStarted: "+response.ToString());
                scanStartedDelegate(response.GetValue(0).ToString());
            });

            // when tag is getting read by the reader and adding to the list, it will give the tags one by one using this event
            socketClient.On("receive_addTag", response =>
            {
                Console.WriteLine("receive_addTag: " + response.ToString());
                addTagsDelegate(response.GetValue(0).ToString());
            });

            // when scan is getting completed
            socketClient.On("receive_scanCompleted", response =>
            {
                Console.WriteLine("receive_scanCompleted: " + response.ToString());
                JObject json = JObject.Parse(response.GetValue(0).ToString());
                var status = (bool)json.GetValue("status");
                var message = json.GetValue("message").ToString();
                JArray jarray = JArray.Parse(json.GetValue("tags").ToString());
                List<string> tags = new List<string>();
                foreach (var tag in jarray)
                {
                    tags.Add(tag.ToString());
                }
                scanCompletedDelegate(status, message, tags);
            });
            socketClient.On("receive_scanError", response =>
            {
                Console.WriteLine("receive_scanError: " + response.ToString());
                scanErrorDelegate(response.GetValue(0).ToString());
            });

        }

        private void onSocketError(object sender, string e)
        {
            onSocketErrorDelegate(e);
        }

        // when socket is getting conencted, it is registering the client with spacecode
        private void onSocketConnected(object sender, EventArgs e)
        {
            var connectionString = new
            {
                deviceType = "client"
            };
            socketClient.EmitAsync("connection", response =>
            {
                Console.WriteLine(response.ToString());
                JObject json = JObject.Parse(response.GetValue(0).ToString());
                JArray jArray = JArray.Parse(json.GetValue("sockets").ToString());
                foreach (var machine in jArray)
                {
                    JObject jObject = JObject.Parse(machine.ToString());
                    socketList.Add(jObject.GetValue("socketId").ToString());
                }
                if (onSocketConnectedDelegate != null)
                    onSocketConnectedDelegate(true, socketList, "Socket connected");
                // setting the first socket id as selected socket id
                Task.Delay(500);
                if (socketList.Count > 0)
                {
                    foreach (var socketId in socketList)
                    {
                        Task.Delay(1000);
                        if (selectedServiceSocketId == null)
                        {
                            Console.WriteLine(selectedServiceSocketId);
                            socketClient.EmitAsync("generic", response1 =>
                            {
                                selectedServiceSocketId = socketId;
                                Console.WriteLine(response1.ToString());
                                Console.WriteLine(socketId);
                                Console.WriteLine(selectedServiceSocketId);
                                JObject jObject = JObject.Parse(response1.GetValue(0).ToString());
                                bool status = ((bool)jObject.GetValue("status"));
                                if (status)
                                {
                                    JArray devicesArr = JArray.Parse(jObject.GetValue("devices").ToString());
                                    Console.WriteLine("DeviceArr Length: " + devicesArr.Count);

                                    foreach (var device in devicesArr)
                                    {
                                        var deviceObj = JObject.Parse(device.ToString());
                                        usbDevices.Add(new UsbDeviceModel { deviceId = deviceObj.GetValue("deviceId").ToString(), socketId = deviceObj.GetValue("socketId").ToString() });
                                    }
                                    updateUsbDeviceListDelegate(status, usbDevices, jObject.GetValue("message").ToString());
                                }
                                else
                                {
                                    updateUsbDeviceListDelegate(status, usbDevices, jObject.GetValue("message").ToString());
                                }
                            }, new
                            {
                                type = "socket",
                                eventName = "getDevices",
                                socketId = socketId
                            });
                        }
                    }
                } else
                {
                    updateUsbDeviceListDelegate(false, usbDevices, "No services connected");
                }

            }, connectionString);
        }

        // in-order to connect with usb device
        public void connectUsbDevice(string socketId, string deviceId)
        {
            socketClient.EmitAsync("send_connectDevice", response => {
                Console.WriteLine(response.ToString());
                var json = JObject.Parse(response.GetValue(0).ToString());
                var status = (bool)json.GetValue("status");
                var message = json.GetValue("message").ToString();
                try
                {
                    deviceSerialNumber = json.GetValue("deviceSerialNumber").ToString();
                    mode = "usbMode";
                    selectedServiceSocketId = socketId;
                    Console.WriteLine("Device serial number: " + deviceSerialNumber);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                connectDeviceUsbDelegate(status, message);
            }, new
            {
                type = "socket",
                socketId,
                deviceId
            });
        }
        
        // in-order to connect with ethernet device
        public void connectEthDevice(string deviceId)
        {
            Console.WriteLine(selectedServiceSocketId);
            socketClient.EmitAsync("send_connectDevice", response => {
                Console.WriteLine(response.ToString());
                var json = JObject.Parse(response.GetValue(0).ToString());
                var status = (bool)json.GetValue("status");
                var message = json.GetValue("message").ToString();
                try
                {
                    deviceSerialNumber = json.GetValue("deviceSerialNumber").ToString();
                    mode = "ethMode";
                    Console.WriteLine("Device serial number: " + deviceSerialNumber);
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
                connectDeviceEthDelegate(status, message);
            }, new
            {
                type = "socket",
                socketId = selectedServiceSocketId,
                deviceId,
            });
        }

        // to disconnect the usb device
        public void disconnectDeviceUsb(string socketId)
        {
            ledOff();
            socketClient.EmitAsync("generic", response => {
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                disconnectDeviceUsbDelegate(true, message);
            }, new
            {
                type = "socket",
                eventName = "disconnectDevice",
                socketId = socketId,
                deviceId = deviceSerialNumber
            });
        }

        // to disconnect with ethernet device
        public void disconnectDeviceEth()
        {
            ledOff();
            Console.WriteLine(deviceSerialNumber);
            socketClient.EmitAsync("generic", response => {
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                deviceSerialNumber = null;
                disconnectDeviceEthDelegate(true, message);
            }, new
            {
                type = "socket",
                eventName = "disconnectDevice",
                socketId = selectedServiceSocketId,
                deviceId = deviceSerialNumber
            });
        }
        
        // to start scan
        public void startScan(string scanMode)
        {
            Console.WriteLine(selectedServiceSocketId);
            LFDeviceModel model = new LFDeviceModel();
            socketClient.EmitAsync("generic", response =>
            {
                Console.WriteLine(response.ToString());
                JObject json = JObject.Parse(response.GetValue(0).ToString());
                var status = (bool)json.GetValue("status");
                var message = json.GetValue("message").ToString();
                startScanDelegate(status, message);
            },
            new
            {
                type = "socket",
                eventName = "startScan",
                scanMode = scanMode,
                deviceId = deviceSerialNumber,
                socketId = selectedServiceSocketId
            });
            
        }

        // to stop scan
        public void stopScan()
        {
            socketClient.EmitAsync("generic", response =>
            {
                Console.WriteLine(response);
                stopScanDelegate(true, "Scan Stopped");
            }, new
            {
                type = "socket",
                eventName = "stopScan",
                deviceId = deviceSerialNumber,
                socketId = selectedServiceSocketId
            });
        }

        // to turn on the selected led
        public void ledOn(List<string> list)
        {
            socketClient.EmitAsync("generic", response =>
            {
                Console.WriteLine(response);
                var json = JObject.Parse(response.GetValue(0).ToString());
                var status = (bool)json.GetValue("status");
                var message = json.GetValue("message").ToString();
                ledOnDelegate(status, message);
            }, new
            {
                type = "socket",
                eventName = "ledOn",
                socketId = selectedServiceSocketId,
                mode = mode,
                list = list,
                deviceId = deviceSerialNumber
            });
        }

        // to turn off the leds
        public void ledOff()
        {
            socketClient.EmitAsync("generic", response =>
            {
                Console.WriteLine(response.ToString());
                var json = JObject.Parse(response.GetValue(0).ToString());
                var status = (bool)json.GetValue("status");
                var message = json.GetValue("message").ToString();
                ledOffDelegate(status, message);
            }, new
            {
                type = "socket",
                eventName = "ledOff",
                socketId = selectedServiceSocketId,
                deviceId = deviceSerialNumber
            });
        }


        // to refresh the device list
        internal void deviceRefresh()
        {
            socketClient.EmitAsync("generic", response =>
            {
                Console.WriteLine(response.ToString());
                JObject jObject = JObject.Parse(response.GetValue(0).ToString());
                var message = jObject.GetValue("message").ToString();
                if (((bool)jObject.GetValue("status")))
                {
                    JArray devicesArr = JArray.Parse(jObject.GetValue("devices").ToString());
                    Console.WriteLine("DeviceArr Length: " + devicesArr.Count);
                    var usbDeviceList = new List<UsbDeviceModel>();
                    foreach (var device in devicesArr)
                    {
                        var deviceObj = JObject.Parse(device.ToString());
                        usbDeviceList.Add(new UsbDeviceModel { deviceId = deviceObj.GetValue("deviceId").ToString(), socketId = deviceObj.GetValue("socketId").ToString() });
                    }
                    deviceRefreshDelegate(true, usbDeviceList, message);
                }
                else
                {
                    deviceRefreshDelegate(false, null, message);
                }
            }, new
            {
                type = "socket",
                eventName = "getDevices",
                socketId = selectedServiceSocketId
            });
        }

        // to refresh the tags when the scanning is in continues mode
        public void refreshTag()
        {
            Console.WriteLine("Refresh tag triggered");
            socketClient.EmitAsync("generic", response =>
            {
                Console.WriteLine(response.ToString());
                JObject jObject = JObject.Parse(response.GetValue(0).ToString());
                var message = jObject.GetValue("msg").ToString();
                refreshTagDelegate(true, message);
            }, new
            {
                type = "socket",
                eventName = "refreshTags",
                socketId = selectedServiceSocketId,
                deviceId = deviceSerialNumber
            });
        }

        internal void formClosing(string socketId)
        {
            disconnectDeviceEth();
            if (socketId != null)
                disconnectDeviceUsb(socketId);
        }
    }

    class UsbDeviceModel
    {
        public string deviceId { get; set; }
        public string socketId { get; set; }
    }

    class LFDeviceModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}
