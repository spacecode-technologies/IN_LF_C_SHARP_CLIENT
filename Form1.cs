using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LF_SOCKET_CLIENT
{
    public partial class Form1 : Form
    {
        private static SocketIO socketClient;
        private static bool connectionStatus = false;
        private static List<UsbDeviceModel> usbDeviceList = new List<UsbDeviceModel>();
        private List<string> socketList = new List<string>();
        private static string mode;
        private static string selectedServiceSocketId;

        private LFDevice lfDevice;

        public Form1()
        {
            InitializeComponent();
            lfDevice = new LFDevice();
            lfDevice.init();

            this.FormClosing += Form1_Closing;

            btnDisconnectEth.Enabled = false;

            lfDevice.connectionStatusDelegate += connectionStatusListener;
            lfDevice.onSocketConnectedDelegate += onLFSocketConnected;
            lfDevice.onSocketErrorDelegate += onLFSocketError;
            lfDevice.updateUsbDeviceListDelegate += updateUsbDeviceList;
            lfDevice.connectDeviceUsbDelegate += connectDeviceUsbListener;
            lfDevice.connectDeviceEthDelegate += connectDeviceEthListener;
            lfDevice.disconnectDeviceUsbDelegate += disconnectDeviceUsbListener;
            lfDevice.disconnectDeviceEthDelegate += disconnectDeviceEthListener;
            lfDevice.startScanDelegate += startScanListener;
            lfDevice.stopScanDelegate += stopScanDelegate;
            lfDevice.addTagsDelegate += addTagsListener;
            lfDevice.scanStartedDelegate += scanStartedListener;
            lfDevice.scanCompletedDelegate += scanCompletedListener;
            lfDevice.scanErrorDelegate += scanErrorListener;
            lfDevice.ledOnDelegate += ledOnListener;
            lfDevice.ledOffDelegate += ledOffListener;
            lfDevice.deviceRefreshDelegate += deviceRefreshListener;
            lfDevice.refreshTagDelegate += refreshTagListener;

            //socketClient = new SocketIO(ConfigurationManager.AppSettings["baseUrl"], new SocketIOOptions
            //{
            //    Query = new Dictionary<string, string>
            //    {
            //        {"token", "v3" }
            //    },
            //    EIO = 4,
            //});
            //socketClient.ConnectAsync().Wait();
            //socketClient.OnConnected += onSocketConnected;
            //socketClient.OnError += onSocketError;

            //socketClient.On("connectToRoom", (response) =>
            //{
            //    txtInfo.Invoke(new MethodInvoker(delegate
            //    {
            //        txtInfo.Text = response.GetValue(0).ToString();
            //    }));
            //});

            //socketClient.On("getConnectedDevices", (response) =>
            //{
            //    JArray jArray = JArray.Parse(response.GetValue(0).ToString());
            //    foreach (var machine in jArray)
            //    {
            //        JObject jObject = JObject.Parse(machine.ToString());
            //        socketList.Add(jObject.GetValue("socketId").ToString());
            //    }
            //    Console.WriteLine("This is getting called");
            //});

            //socketClient.On("receive_getDevices", (response) =>
            //{
            //    JObject jObject = JObject.Parse(response.GetValue(0).ToString());
            //    if (((bool)jObject.GetValue("status")))
            //    {
            //        JArray devicesArr = JArray.Parse(jObject.GetValue("devices").ToString());
            //        Console.WriteLine("DeviceArr Length: "+devicesArr.Count);
            //        foreach (var device in devicesArr)
            //        {
            //            var deviceObj = JObject.Parse(device.ToString());
            //            usbDevices.Add(new UsbDeviceModel { deviceId = deviceObj.GetValue("deviceId").ToString(), socketId = deviceObj.GetValue("socketId").ToString() });
            //            usbDeviceSelection.Invoke(new MethodInvoker(delegate
            //            {
            //                usbDeviceSelection.Items.Add(deviceObj.GetValue("deviceId").ToString());
            //            }));
            //        }
            //        txtInfo.Invoke(new MethodInvoker(delegate
            //        {
            //            txtInfo.Text = jObject.GetValue("message").ToString();
            //        }));
            //    }
            //    else
            //    {
            //        txtInfo.Invoke(new MethodInvoker(delegate
            //        {
            //            txtInfo.Text = jObject.GetValue("message").ToString();
            //        }));
            //    }
            //});

            //socketClient.On("receive_connectDevice", (response) =>
            //{
            //    var json = JObject.Parse(response.GetValue(0).ToString());
            //    txtInfo.Invoke(new MethodInvoker(delegate
            //    {
            //        txtInfo.Text = json.GetValue("message").ToString();
            //    }));
            //    if ((bool)json.GetValue("status"))
            //    {
            //        btnDisconnectEth.Invoke(new MethodInvoker(delegate
            //        {
            //            btnDisconnectEth.Enabled = true;
            //        }));
            //        btnConnectEth.Invoke(new MethodInvoker(delegate
            //        {
            //            btnConnectEth.Enabled = false;
            //        }));
            //        btnDisconnect.Invoke(new MethodInvoker(delegate
            //        {
            //            btnDisconnect.Enabled = true;
            //        }));
            //        btnConnect.Invoke(new MethodInvoker(delegate
            //        {
            //            btnConnect.Enabled = false;
            //        }));
            //    }
            //});

            //============================================================================
            //socketClient.On("receive_addTag", response =>
            //{
            //    if (tagList.InvokeRequired)
            //    {
            //        tagList.Invoke(new MethodInvoker(delegate
            //        {
            //            tagList.Items.Add(response.GetValue(0).ToString());
            //        }));
            //    } else
            //    {
            //        tagList.Items.Add(response.GetValue(0).ToString());
            //    }
            //    tagCountText.Invoke(new MethodInvoker(delegate
            //    {
            //        tagCountText.Text = tagList.Items.Count.ToString();
            //    }));
            //});

            //socketClient.On("receive_stopScan", (response) =>
            //{
            //    txtInfo.Invoke(new MethodInvoker(delegate
            //    {
            //        txtInfo.Text = "Scan Stopped";
            //    }));
            //    btnStartScan.Invoke(new MethodInvoker(delegate
            //    {
            //        btnStartScan.Enabled = true;
            //    }));
            //    btnStopScan.Invoke(new MethodInvoker(delegate
            //    {
            //        btnStopScan.Enabled = false;
            //    }));
            //});

            //=================================================================

            //socketClient.On("receive_scanStarted", (response) =>
            //{
            //    txtInfo.Invoke(new MethodInvoker(delegate
            //    {
            //        txtInfo.Text = "Scan Started";
            //    }));
            //    if (!checkContineousMode.Checked)
            //    {
            //        tagList.Invoke(new MethodInvoker(delegate
            //        {
            //            tagList.Items.Clear();
            //        }));
            //    }
            //    btnStartScan.Invoke(new MethodInvoker(delegate
            //    {
            //        btnStartScan.Enabled = false;
            //    }));
            //    btnStopScan.Invoke(new MethodInvoker(delegate
            //    {
            //        btnStopScan.Enabled = true;
            //    }));
            //});

            //socketClient.On("receive_scanCompleted", (response) =>
            //{
            //    txtInfo.Invoke(new MethodInvoker(delegate
            //    {
            //        txtInfo.Text = "Scan Completed";
            //    }));
            //    btnStartScan.Invoke(new MethodInvoker(delegate
            //    {
            //        btnStartScan.Enabled = true;
            //    }));
            //    btnStopScan.Invoke(new MethodInvoker(delegate
            //    {
            //        btnStopScan.Enabled = false;
            //    }));
            //});

            //socketClient.On("receive_disconnectDevice", (response) =>
            //{
            //    var json = JObject.Parse(response.GetValue(0).ToString());
            //    var message = json.GetValue("message").ToString();
            //    txtInfo.Invoke(new MethodInvoker(delegate
            //    {
            //        txtInfo.Text = message;
            //    }));
            //    btnConnect.Invoke(new MethodInvoker(delegate
            //    {
            //        btnConnect.Enabled = true;
            //    }));
            //    btnConnectEth.Invoke(new MethodInvoker(delegate
            //    {
            //        btnConnectEth.Enabled = true;
            //    }));
            //    btnDisconnect.Invoke(new MethodInvoker(delegate
            //    {
            //        btnDisconnect.Enabled = false;
            //    }));
            //    btnDisconnectEth.Invoke(new MethodInvoker(delegate
            //    {
            //        btnDisconnectEth.Enabled = false;
            //    }));
            //});

            //socketClient.On("receive_ledOn", (response) =>
            //{
            //    var json = JObject.Parse(response.GetValue(0).ToString());
            //    var message = json.GetValue("message").ToString();
            //    txtInfo.Invoke(new MethodInvoker(delegate
            //    {
            //        txtInfo.Text = message;
            //    }));
            //});

            //socketClient.On("receive_ledOff", (response) =>
            //{
            //    var json = JObject.Parse(response.GetValue(0).ToString());
            //    var message = json.GetValue("message").ToString();
            //    txtInfo.Invoke(new MethodInvoker(delegate
            //    {
            //        txtInfo.Text = message;
            //    }));
            //});

        }

        private void refreshTagListener(bool status)
        {
            if (status)
            {
                tagList.Invoke(new MethodInvoker(delegate
                {
                    tagList.Items.Clear();
                }));
                tagCountText.Invoke(new MethodInvoker(delegate
                {
                    tagCountText.Text = "0";
                }));
            }
        }

        private void deviceRefreshListener(bool status, List<UsbDeviceModel> list, string msg)
        {
            updateInfoStatus(msg);
            usbDeviceList = list;
            foreach (var device in list)
            {
                usbDeviceSelection.Invoke(new MethodInvoker(delegate
                {
                    usbDeviceSelection.Items.Add(device);
                }));
            }
        }

        private void connectionStatusListener(string msg)
        {
            updateInfoStatus(msg);
        }

        private void ledOffListener(bool status, string msg)
        {
            updateInfoStatus(msg);
        }

        private void ledOnListener(bool status, string msg)
        {
            updateInfoStatus(msg);
        }

        private void scanErrorListener(string str)
        {
            updateInfoStatus(str);
        }

        private void scanCompletedListener(bool status, string message, List<string> tags)
        {
            updateInfoStatus(message);
            btnStartScan.Invoke(new MethodInvoker(delegate
            {
                btnStartScan.Enabled = true;
            }));
            btnStopScan.Invoke(new MethodInvoker(delegate
            {
                btnStopScan.Enabled = false;
            }));
        }

        private void scanStartedListener(string str)
        {
            updateInfoStatus(str);
            if (!checkContineousMode.Checked)
            {
                tagList.Invoke(new MethodInvoker(delegate
                {
                    tagList.Items.Clear();
                }));
            }
            btnStartScan.Invoke(new MethodInvoker(delegate
            {
                btnStartScan.Enabled = false;
            }));
            btnStopScan.Invoke(new MethodInvoker(delegate
            {
                btnStopScan.Enabled = true;
            }));
        }

        private void stopScanDelegate(bool status, string msg)
        {
            txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = msg;
                }));
            btnStartScan.Invoke(new MethodInvoker(delegate
            {
                btnStartScan.Enabled = true;
            }));
            btnStopScan.Invoke(new MethodInvoker(delegate
            {
                btnStopScan.Enabled = false;
            }));
        }

        private void addTagsListener(string str)
        {
            if (tagList.InvokeRequired)
            {
                tagList.Invoke(new MethodInvoker(delegate
                {
                    tagList.Items.Add(str);
                }));
            }
            else
            {
                tagList.Items.Add(str);
            }
            tagCountText.Invoke(new MethodInvoker(delegate
            {
                tagCountText.Text = tagList.Items.Count.ToString();
            }));
        }

        private void startScanListener(bool status, string msg)
        {
            updateInfoStatus(msg);
        }

        private void disconnectDeviceUsbListener(bool status, string msg)
        {
            txtInfo.Invoke(new MethodInvoker(delegate
            {
                txtInfo.Text = msg;
            }));
            btnConnect.Invoke(new MethodInvoker(delegate
            {
                btnConnect.Enabled = true;
            }));
            btnConnectEth.Invoke(new MethodInvoker(delegate
            {
                btnConnectEth.Enabled = true;
            }));
            btnDisconnect.Invoke(new MethodInvoker(delegate
            {
                btnDisconnect.Enabled = false;
            }));
            btnDisconnectEth.Invoke(new MethodInvoker(delegate
            {
                btnDisconnectEth.Enabled = false;
            }));
            btnDisconnect.Enabled = false;
            btnConnect.Invoke(new MethodInvoker(delegate
            {
                btnConnect.Enabled = true;
            }));
        }
        
        private void disconnectDeviceEthListener(bool status, string msg)
        {
            txtInfo.Invoke(new MethodInvoker(delegate
            {
                txtInfo.Text = msg;
            }));
            btnConnect.Invoke(new MethodInvoker(delegate
            {
                btnConnect.Enabled = true;
            }));
            btnConnectEth.Invoke(new MethodInvoker(delegate
            {
                btnConnectEth.Enabled = true;
            }));
            btnDisconnect.Invoke(new MethodInvoker(delegate
            {
                btnDisconnect.Enabled = false;
            }));
            btnDisconnectEth.Invoke(new MethodInvoker(delegate
            {
                btnDisconnectEth.Enabled = false;
            }));
            btnDisconnect.Enabled = false;
            btnConnect.Invoke(new MethodInvoker(delegate
            {
                btnConnect.Enabled = true;
            }));
        }

        private void connectDeviceUsbListener(bool status, string msg)
        {
            selectedServiceSocketId = usbDeviceList[usbDeviceSelection.SelectedIndex].socketId;
            updateInfoStatus(msg);
            if (status)
            {
                btnDisconnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnectEth.Enabled = true;
                }));
                btnConnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnConnectEth.Enabled = false;
                }));
                btnDisconnect.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnect.Enabled = true;
                }));
                btnConnect.Invoke(new MethodInvoker(delegate
                {
                    btnConnect.Enabled = false;
                }));
            }
        }

        private void connectDeviceEthListener(bool status, string msg)
        {
            selectedServiceSocketId = socketList[0];
            updateInfoStatus(msg);
            if (status)
            {
                btnDisconnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnectEth.Enabled = true;
                }));
                btnConnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnConnectEth.Enabled = false;
                }));
                btnDisconnect.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnect.Enabled = true;
                }));
                btnConnect.Invoke(new MethodInvoker(delegate
                {
                    btnConnect.Enabled = false;
                }));
            }
        }

            private void updateUsbDeviceList(List<UsbDeviceModel> usbDevices, string msg)
        {
            usbDeviceList = usbDevices;
            updateInfoStatus(msg);
            foreach (var usbDevice in usbDevices)
            {
                usbDeviceSelection.Invoke(new MethodInvoker(delegate
                {
                    usbDeviceSelection.Items.Add(usbDevice.deviceId);
                }));
            }
        }

        private void onLFSocketError(string e)
        {
            updateInfoStatus(e);
        }

        private void onLFSocketConnected(bool status, List<string> socketList, string msg)
        {
            this.socketList = socketList;
            if (socketList.Count > 0)
                selectedServiceSocketId = this.socketList[0];
            else
                updateInfoStatus("No service connected");
            txtInfo.Invoke(new MethodInvoker(delegate
            {
                txtInfo.Text = msg;
            }));
        }

        //private void onSocketConnected(object sender, EventArgs e)
        //{
        //    var connectionString = new
        //    {
        //        deviceType = "client"
        //    };
        //    socketClient.EmitAsync("connection", response =>
        //    {
        //        Console.WriteLine(response.ToString());
        //        JObject json = JObject.Parse(response.GetValue(0).ToString());
        //        JArray jArray = JArray.Parse(json.GetValue("sockets").ToString());
        //        foreach (var machine in jArray)
        //        {
        //            JObject jObject = JObject.Parse(machine.ToString());
        //            socketList.Add(jObject.GetValue("socketId").ToString());
        //        }
        //        // setting the first socket id as selected socket id
        //        if (socketList.Count > 0)
        //        {
        //            selectedServiceSocketId = socketList[0].ToString();
        //            Console.WriteLine(selectedServiceSocketId);
        //            socketClient.EmitAsync("generic", response1 =>
        //            {
        //                Console.WriteLine(response1.ToString());
        //                JObject jObject = JObject.Parse(response1.GetValue(0).ToString());
        //                if (((bool)jObject.GetValue("status")))
        //                {
        //                    JArray devicesArr = JArray.Parse(jObject.GetValue("devices").ToString());
        //                    Console.WriteLine("DeviceArr Length: " + devicesArr.Count);
        //                    foreach (var device in devicesArr)
        //                    {
        //                        var deviceObj = JObject.Parse(device.ToString());
        //                        usbDevices.Add(new UsbDeviceModel { deviceId = deviceObj.GetValue("deviceId").ToString(), socketId = deviceObj.GetValue("socketId").ToString() });
        //                        usbDeviceSelection.Invoke(new MethodInvoker(delegate
        //                        {
        //                            usbDeviceSelection.Items.Add(deviceObj.GetValue("deviceId").ToString());
        //                        }));
        //                    }
        //                    txtInfo.Invoke(new MethodInvoker(delegate
        //                    {
        //                        txtInfo.Text = jObject.GetValue("message").ToString();
        //                    }));
        //                }
        //                else
        //                {
        //                    txtInfo.Invoke(new MethodInvoker(delegate
        //                    {
        //                        txtInfo.Text = jObject.GetValue("message").ToString();
        //                    }));
        //                }
        //            }, new
        //            {
        //                eventName = "getDevices",
        //                socketId = selectedServiceSocketId
        //            });
        //        }
        //        txtInfo.Invoke(new MethodInvoker(delegate
        //        {
        //            txtInfo.Text = response.ToString();
        //        }));

        //        // getdevices

        //    }, connectionString);
        //    socketClient.EmitAsync("getConnectedDevices", "");
            
        //}

        //private void onSocketError(object sender, string e)
        //{
        //    txtInfo.Invoke(new MethodInvoker(delegate
        //    {
        //        txtInfo.Text = e;
        //    }));
        //}

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            var socketId = usbDeviceList[usbDeviceSelection.SelectedIndex].socketId;
            var deviceId = usbDeviceList[usbDeviceSelection.SelectedIndex].deviceId;
            lfDevice.connectUsbDevice(socketId, deviceId);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            usbDeviceList.Clear();
            usbDeviceSelection.Items.Clear();
            lfDevice.deviceRefresh();
        }

        private void btnConnectEth_Click(object sender, EventArgs e)
        {
            mode = "ethernet";
            if (socketList.Count > 0)
            {
                lfDevice.connectEthDevice(selectedServiceSocketId, txtIpAddress.Text.Trim());
            } else
            {
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = "No services connected";
                }));
            }
        }

        private void btnDisconnectEth_Click(object sender, EventArgs e)
        {
            lfDevice.disconnectDeviceEth(selectedServiceSocketId, txtIpAddress.Text.Trim());
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            var socketId = usbDeviceList[usbDeviceSelection.SelectedIndex].socketId;
            var deviceId = usbDeviceList[usbDeviceSelection.SelectedIndex].deviceId;
            lfDevice.disconnectDeviceUsb(socketId, deviceId);
        }

        private void btnStartScan_Click(object sender, EventArgs e)
        {
            string scanMode;
            if (checkContineousMode.Checked)
            {
                scanMode = "continous";
            } else
            {
                scanMode = "none";
            }

            lfDevice.startScan(scanMode, selectedServiceSocketId);
        }

        private void btnStopScan_Click(object sender, EventArgs e)
        {
            lfDevice.stopScan(selectedServiceSocketId);
        }

        private void machineList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkContineousMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkContineousMode.Checked)
            {
                btnRefreshTags.Invoke(new MethodInvoker(delegate
                {
                    btnRefreshTags.Enabled = true;
                }));
            } else
            {
                btnRefreshTags.Invoke(new MethodInvoker(delegate
                {
                    btnRefreshTags.Enabled = false;
                }));
            }
        }

        private void btnRefreshTags_Click(object sender, EventArgs e)
        {
            lfDevice.refreshTag(selectedServiceSocketId);
        }

        private void btnLedOn_Click(object sender, EventArgs e)
        {
            var list = new List<string>();
            foreach (var tag in tagList.SelectedItems)
            {
                list.Add(tag.ToString().Split(',')[1].Split('-')[1]);
            }
            lfDevice.ledOn(list);
        }

        private void btnLedAllAtOnce_Click(object sender, EventArgs e)
        {
            for (int i=0; i<tagList.Items.Count; i++)
            {
                tagList.SelectedItem = tagList.Items[i];
            }
            var list = new List<string>();
            foreach (var tag in tagList.SelectedItems)
            {
                list.Add(tag.ToString().Split(',')[1].Split('-')[1]);
            }
            lfDevice.ledOn(list);
        }

        private void btnLedOff_Click(object sender, EventArgs e)
        {
            lfDevice.ledOff();
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void updateInfoStatus(string msg)
        {
            txtInfo.Invoke(new MethodInvoker(delegate
            {
                txtInfo.Text = msg;
            }));
        }
    }
}
