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
        private static List<UsbDeviceModel> usbDevices = new List<UsbDeviceModel>();
        private static ArrayList socketList = new ArrayList();
        private static string mode;
        private static string selectedServiceSocketId;

        public Form1()
        {
            InitializeComponent();

            this.FormClosing += Form1_Closing;

            btnDisconnectEth.Enabled = false;
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

            socketClient.On("connectToRoom", (response) =>
            {
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = response.GetValue(0).ToString();
                }));
            });

            socketClient.On("getConnectedDevices", (response) =>
            {
                JArray jArray = JArray.Parse(response.GetValue(0).ToString());
                foreach (var machine in jArray)
                {
                    JObject jObject = JObject.Parse(machine.ToString());
                    socketList.Add(jObject.GetValue("socketId").ToString());
                }
                Console.WriteLine("This is getting called");
            });

            socketClient.On("receive_getDevices", (response) =>
            {
                JObject jObject = JObject.Parse(response.GetValue(0).ToString());
                if (((bool)jObject.GetValue("status")))
                {
                    JArray devicesArr = JArray.Parse(jObject.GetValue("devices").ToString());
                    Console.WriteLine("DeviceArr Length: "+devicesArr.Count);
                    foreach (var device in devicesArr)
                    {
                        var deviceObj = JObject.Parse(device.ToString());
                        usbDevices.Add(new UsbDeviceModel { deviceId = deviceObj.GetValue("deviceId").ToString(), socketId = deviceObj.GetValue("socketId").ToString() });
                        usbDeviceSelection.Invoke(new MethodInvoker(delegate
                        {
                            usbDeviceSelection.Items.Add(deviceObj.GetValue("deviceId").ToString());
                        }));
                    }
                    txtInfo.Invoke(new MethodInvoker(delegate
                    {
                        txtInfo.Text = jObject.GetValue("message").ToString();
                    }));
                }
                else
                {
                    txtInfo.Invoke(new MethodInvoker(delegate
                    {
                        txtInfo.Text = jObject.GetValue("message").ToString();
                    }));
                }
            });

            socketClient.On("receive_connectDevice", (response) =>
            {
                var json = JObject.Parse(response.GetValue(0).ToString());
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = json.GetValue("message").ToString();
                }));
                if ((bool)json.GetValue("status"))
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
            });

            socketClient.On("receive_addTag", response =>
            {
                if (tagList.InvokeRequired)
                {
                    tagList.Invoke(new MethodInvoker(delegate
                    {
                        tagList.Items.Add(response.GetValue(0).ToString());
                    }));
                } else
                {
                    tagList.Items.Add(response.GetValue(0).ToString());
                }
                tagCountText.Invoke(new MethodInvoker(delegate
                {
                    tagCountText.Text = tagList.Items.Count.ToString();
                }));
            });

            socketClient.On("receive_stopScan", (response) =>
            {
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = "Scan Stopped";
                }));
                btnStartScan.Invoke(new MethodInvoker(delegate
                {
                    btnStartScan.Enabled = true;
                }));
                btnStopScan.Invoke(new MethodInvoker(delegate
                {
                    btnStopScan.Enabled = false;
                }));
            });

            socketClient.On("receive_scanStarted", (response) =>
            {
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = "Scan Started";
                }));
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
            });

            socketClient.On("receive_scanCompleted", (response) =>
            {
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = "Scan Completed";
                }));
                btnStartScan.Invoke(new MethodInvoker(delegate
                {
                    btnStartScan.Enabled = true;
                }));
                btnStopScan.Invoke(new MethodInvoker(delegate
                {
                    btnStopScan.Enabled = false;
                }));
            });

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

            socketClient.On("receive_ledOn", (response) =>
            {
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = message;
                }));
            });

            socketClient.On("receive_ledOff", (response) =>
            {
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = message;
                }));
            });

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
                                usbDeviceSelection.Invoke(new MethodInvoker(delegate
                                {
                                    usbDeviceSelection.Items.Add(deviceObj.GetValue("deviceId").ToString());
                                }));
                            }
                            txtInfo.Invoke(new MethodInvoker(delegate
                            {
                                txtInfo.Text = jObject.GetValue("message").ToString();
                            }));
                        }
                        else
                        {
                            txtInfo.Invoke(new MethodInvoker(delegate
                            {
                                txtInfo.Text = jObject.GetValue("message").ToString();
                            }));
                        }
                    }, new
                    {
                        eventName = "getDevices",
                        socketId = selectedServiceSocketId
                    });
                }
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = response.ToString();
                }));

                // getdevices

            }, connectionString);
            socketClient.EmitAsync("getConnectedDevices", "");
            
        }

        private void onSocketError(object sender, string e)
        {
            txtInfo.Invoke(new MethodInvoker(delegate
            {
                txtInfo.Text = e;
            }));
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            mode = "usbMode";
            socketClient.EmitAsync("send_connectDevice", response => { 
                Console.WriteLine(response.ToString());
                var json = JObject.Parse(response.GetValue(0).ToString());
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = json.GetValue("message").ToString();
                }));
                if ((bool)json.GetValue("status"))
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
            }, new
            {
                socketId = usbDevices[usbDeviceSelection.SelectedIndex].socketId,
                deviceId = usbDevices[usbDeviceSelection.SelectedIndex].deviceId
            });
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            usbDevices.Clear();
            usbDeviceSelection.Items.Clear();
            socketClient.EmitAsync("generic", response =>
            {
                Console.WriteLine(response.ToString());
                JObject jObject = JObject.Parse(response.GetValue(0).ToString());
                if (((bool)jObject.GetValue("status")))
                {
                    JArray devicesArr = JArray.Parse(jObject.GetValue("devices").ToString());
                    Console.WriteLine("DeviceArr Length: " + devicesArr.Count);
                    foreach (var device in devicesArr)
                    {
                        var deviceObj = JObject.Parse(device.ToString());
                        usbDevices.Add(new UsbDeviceModel { deviceId = deviceObj.GetValue("deviceId").ToString(), socketId = deviceObj.GetValue("socketId").ToString() });
                        usbDeviceSelection.Invoke(new MethodInvoker(delegate
                        {
                            usbDeviceSelection.Items.Add(deviceObj.GetValue("deviceId").ToString());
                        }));
                    }
                    txtInfo.Invoke(new MethodInvoker(delegate
                    {
                        txtInfo.Text = jObject.GetValue("message").ToString();
                    }));
                }
                else
                {
                    txtInfo.Invoke(new MethodInvoker(delegate
                    {
                        txtInfo.Text = jObject.GetValue("message").ToString();
                    }));
                }
            }, new
            {
                eventName = "getDevices",
                socketId = selectedServiceSocketId
            });
        }

        private void btnConnectEth_Click(object sender, EventArgs e)
        {
            mode = "ethernet";
            if (socketList.Count > 0)
            {
                var data = new
                {
                    socketId = selectedServiceSocketId,
                    deviceId = txtIpAddress.Text,
                };
                socketClient.EmitAsync("send_connectDevice", data);
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
            socketClient.EmitAsync("generic", response => {
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = message;
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
            }, new
            {
                eventName = "disconnectDevice",
                socketId = socketList[0],
                deviceId = txtIpAddress.Text
            });
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            socketClient.EmitAsync("generic", response => {
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = message;
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
            }, new
            {
                eventName = "disconnectDevice",
                socketId = usbDevices[usbDeviceSelection.SelectedIndex].socketId,
                deviceId = usbDevices[usbDeviceSelection.SelectedIndex].deviceId
            });
            btnDisconnect.Enabled = false;
            btnConnect.Invoke(new MethodInvoker(delegate
            {
                btnConnect.Enabled = true;
            }));
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
            //socketClient.EmitAsync("send_startScan", response => 
            //{
            //    Console.WriteLine(response.ToString());
            //}, 
            //new
            //{
            //    scanMode = scanMode
            //});
            socketClient.EmitAsync("generic", response =>
            {
                Console.WriteLine(response.ToString());
            },
            new
            {
                eventName = "startScan",
                scanMode = scanMode,
                socketId = selectedServiceSocketId
            });
        }

        private void btnStopScan_Click(object sender, EventArgs e)
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
            socketClient.EmitAsync("generic", response =>
            {
                tagList.Invoke(new MethodInvoker(delegate
                {
                    tagList.Items.Clear();
                }));
                tagCountText.Invoke(new MethodInvoker(delegate
                {
                    tagCountText.Text = "0";
                }));
            }, new 
            {
                eventName = "refreshTags",
                socketId = usbDevices[usbDeviceSelection.SelectedIndex].socketId
            });
        }

        private void btnLedOn_Click(object sender, EventArgs e)
        {
            var list = new List<string>();
            foreach (var tag in tagList.SelectedItems)
            {
                list.Add(tag.ToString().Split(',')[1].Split('-')[1]);
            }
            socketClient.EmitAsync("generic", response => 
            {
                Console.WriteLine(response);
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = message;
                }));
            }, new
            {
                eventName = "ledOn",
                socketId = selectedServiceSocketId,
                mode = mode,
                list = list
            });
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
            socketClient.EmitAsync("generic", response => 
            {
                Console.WriteLine(response.ToString());
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = message;
                }));
            }, new
            {
                eventName = "ledOn",
                socketId = selectedServiceSocketId,
                mode = mode,
                list = list
            });
        }

        private void btnLedOff_Click(object sender, EventArgs e)
        {
            socketClient.EmitAsync("generic", response => 
            {
                Console.WriteLine(response.ToString());
                var json = JObject.Parse(response.GetValue(0).ToString());
                var message = json.GetValue("message").ToString();
                txtInfo.Invoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = message;
                }));
            }, new 
            {
                eventName = "ledOff",
                socketId = selectedServiceSocketId
            });
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            socketClient.EmitAsync("disconnectDevice", "");
        }
    }

    class UsbDeviceModel
    {
        public string deviceId { get; set; }
        public string socketId { get; set; }
    }
}
