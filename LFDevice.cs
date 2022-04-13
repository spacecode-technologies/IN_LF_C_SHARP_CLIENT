﻿using Newtonsoft.Json.Linq;
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

        private string ethDeviceSerial = null;

        private static List<UsbDeviceModel> usbDevices = new List<UsbDeviceModel>();
        private static List<string> socketList = new List<string>();
        private static string mode;
        private static string selectedServiceSocketId;

        public delegate void connectionStatusDel(string msg);
        public delegate void onSocketErrorDel(string e);
        public delegate void onSocketConnectedDel(bool status, List<string> socketList, string msg);
        public delegate void updateUsbDeviceListDel(List<UsbDeviceModel> usbDevices, string msg);
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
        public delegate void refreshTagDel(bool status);

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
            socketClient.ConnectAsync().Wait();
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

        private void listener()
        {
            socketClient.On("receive_scanStarted", response =>
            {
                scanStartedDelegate(response.GetValue(0).ToString());
            });
            socketClient.On("receive_stopScan", (response) =>
            {
                stopScanDelegate(true, "Scan Stopped");
            });
            socketClient.On("receive_addTag", response =>
            {
                addTagsDelegate(response.GetValue(0).ToString());
            });
            socketClient.On("receive_scanCompleted", response =>
            {
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
                scanErrorDelegate(response.GetValue(0).ToString());
            });

        }

        private void onSocketError(object sender, string e)
        {
            onSocketErrorDelegate(e);
        }

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
                if (socketList.Count > 0)
                {
                    selectedServiceSocketId = socketList[0].ToString();
                    Console.WriteLine(selectedServiceSocketId);
                    socketClient.EmitAsync("generic", response1 =>
                    {
                        Console.WriteLine(response1.ToString());
                        JObject jObject = JObject.Parse(response1.GetValue(0).ToString());
                        if (((bool)jObject.GetValue("status")))
                        {
                            JArray devicesArr = JArray.Parse(jObject.GetValue("devices").ToString());
                            Console.WriteLine("DeviceArr Length: " + devicesArr.Count);

                            foreach (var device in devicesArr)
                            {
                                var deviceObj = JObject.Parse(device.ToString());
                                usbDevices.Add(new UsbDeviceModel { deviceId = deviceObj.GetValue("deviceId").ToString(), socketId = deviceObj.GetValue("socketId").ToString() });
                            }
                            updateUsbDeviceListDelegate(usbDevices, jObject.GetValue("message").ToString());
                        }
                        else
                        {
                            updateUsbDeviceListDelegate(usbDevices, jObject.GetValue("message").ToString());
                        }
                    }, new
                    {
                        eventName = "getDevices",
                        socketId = selectedServiceSocketId
                    });
                }

            }, connectionString);
        }


        public void connectUsbDevice(string socketId, string deviceId)
        {
            socketClient.EmitAsync("send_connectDevice", response => {
                Console.WriteLine(response.ToString());
                var json = JObject.Parse(response.GetValue(0).ToString());
                var status = (bool)json.GetValue("status");
                var message = json.GetValue("message").ToString();
                connectDeviceUsbDelegate(status, message);
            }, new
            {
                socketId,
                deviceId
            });
        }
        
        public void connectEthDevice(string socketId, string deviceId)
        {
            socketClient.EmitAsync("send_connectDevice", response => {
                Console.WriteLine(response.ToString());
                var json = JObject.Parse(response.GetValue(0).ToString());
                var status = (bool)json.GetValue("status");
                var message = json.GetValue("message").ToString();
                ethDeviceSerial = json.GetValue("deviceSerialNumber").ToString();
                connectDeviceEthDelegate(status, message);
            }, new
            {
                socketId,
                deviceId
            });
        }

        public void disconnectDeviceUsb(string socketId, string deviceId)
        {
            socketClient.EmitAsync("generic", response => {
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                disconnectDeviceUsbDelegate(true, message);
            }, new
            {
                eventName = "disconnectDevice",
                socketId = socketId,
                deviceId = deviceId
            });
        }

        public void disconnectDeviceEth(string socketId, string deviceId)
        {
            socketClient.EmitAsync("generic", response => {
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                ethDeviceSerial = null;
                disconnectDeviceEthDelegate(true, message);
            }, new
            {
                eventName = "disconnectDevice",
                socketId = socketId,
                deviceId = ethDeviceSerial
            });
        }
        public void startScan(string scanMode, string selectedServiceSocketId)
        {
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
                eventName = "startScan",
                scanMode = scanMode,
                socketId = selectedServiceSocketId
            });
            
        }

        public void stopScan(string socketId)
        {
            socketClient.EmitAsync("generic", response =>
            {
                Console.WriteLine(response);
            }, new
            {
                eventName = "stopScan",
                socketId = selectedServiceSocketId
            });
        }

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
                eventName = "ledOn",
                socketId = selectedServiceSocketId,
                mode = mode,
                list = list
            });
        }

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
                eventName = "ledOff",
                socketId = selectedServiceSocketId
            });
        }

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
                eventName = "getDevices",
                socketId = selectedServiceSocketId
            });
        }

        public void refreshTag(string socketId)
        {
            socketClient.EmitAsync("generic", response =>
            {
                refreshTagDelegate(true);
            }, new
            {
                eventName = "refreshTags",
                socketId = socketId
            });
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