using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LF_SOCKET_CLIENT
{
    public partial class Form1 : Form
    {
        private static List<UsbDeviceModel> usbDeviceList = new List<UsbDeviceModel>();
        private int currentTabIndex = 0;
        private bool deviceConnected = false;
        private bool inScan = false;
        private bool socketConnected = false;

        private LFDevice lfDevice;

        public Form1()
        {
            InitializeComponent();
            initializeLFDevice();
        }

        private void initializeLFDevice()
        {
            lfDevice = new LFDevice();
            lfDevice.init();

            this.FormClosing += Form1_Closing;

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
        }

        private void refreshTagListener(bool status, string msg)
        {
            updateInfoStatus(msg);
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
            btnRefreshTags.Invoke(new MethodInvoker(delegate
            {
                btnRefreshTags.Enabled = true;
            }));
            updateInfoStatus("Refresh: "+msg);
            if (status)
            {
                usbDeviceList = list;
                foreach (var device in list)
                {
                    usbDeviceSelection.Invoke(new MethodInvoker(delegate
                    {
                        usbDeviceSelection.Items.Add(device.deviceId);
                    }));
                    enableBtnConnectUsb();
                }
            } else
            {
                usbDeviceList.Clear();
                usbDeviceSelection.Invoke(new MethodInvoker(delegate
                {
                    usbDeviceSelection.Items.Clear();
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
            btnLedOn.Invoke(new MethodInvoker(delegate
            {
                btnLedOn.Enabled = true;
            }));
            btnLedAllAtOnce.Invoke(new MethodInvoker(delegate
            {
                btnLedAllAtOnce.Enabled = true;
            }));
            btnLedOff.Invoke(new MethodInvoker(delegate
            {
                btnLedOff.Enabled = false;
            }));
        }

        private void ledOnListener(bool status, string msg)
        {
            updateInfoStatus(msg);
            btnLedOn.Invoke(new MethodInvoker(delegate
            {
                btnLedOn.Enabled = false;
            }));
            btnLedAllAtOnce.Invoke(new MethodInvoker(delegate
            {
                btnLedAllAtOnce.Enabled = false;
            }));
            btnLedOff.Invoke(new MethodInvoker(delegate
            {
                btnLedOff.Enabled = true;
            }));
        }

        private void scanErrorListener(string str)
        {
            updateInfoStatus(str);
        }

        private void scanCompletedListener(bool status, string message, List<string> tags)
        {
            inScan = false;
            btnRefreshTags.Invoke(new MethodInvoker(delegate
            {
                btnRefreshTags.Enabled = false;
            }));
            updateInfoStatus(message);
            btnStartScan.Invoke(new MethodInvoker(delegate
            {
                btnStartScan.Enabled = true;
            }));
            btnStopScan.Invoke(new MethodInvoker(delegate
            {
                btnStopScan.Enabled = false;
            }));
            if (tagList.Items.Count > 0)
            {
                btnLedOn.Invoke(new MethodInvoker(delegate
                {
                    btnLedOn.Enabled = true;
                }));
                btnLedAllAtOnce.Invoke(new MethodInvoker(delegate
                {
                    btnLedAllAtOnce.Enabled = true;
                }));
            }
            btnDisconnectEth.Invoke(new MethodInvoker(delegate
            {
                btnDisconnectEth.Enabled = true;
            }));
            btnDisconnect.Invoke(new MethodInvoker(delegate
            {
                btnDisconnect.Enabled = true;
            }));
        }

        private void scanStartedListener(string str)
        {
            inScan = true;
            updateInfoStatus(str);
            if (!checkContineousMode.Checked)
            {
                tagList.Invoke(new MethodInvoker(delegate
                {
                    tagList.Items.Clear();
                }));
            } else
            {
                btnRefreshTags.Invoke(new MethodInvoker(delegate
                {
                    btnRefreshTags.Enabled = true;
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
            inScan = false;
            btnRefreshTags.Invoke(new MethodInvoker(delegate
            {
                btnRefreshTags.Enabled = false;
            }));
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
            if (tagList.Items.Count > 0)
            {
                btnLedOn.Invoke(new MethodInvoker(delegate
                {
                    btnLedOn.Enabled = true;
                }));
                btnLedAllAtOnce.Invoke(new MethodInvoker(delegate
                {
                    btnLedAllAtOnce.Enabled = true;
                }));
            }
            btnDisconnectEth.Invoke(new MethodInvoker(delegate
            {
                btnDisconnectEth.Enabled = true;
            }));
            btnDisconnect.Invoke(new MethodInvoker(delegate
            {
                btnDisconnect.Enabled = true;
            }));

        }

        private void addTagsListener(string str)
        {
            if (!tagList.Items.Contains(str))
            {
                tagList.Invoke(new MethodInvoker(delegate
                {
                    tagList.Items.Add(str);
                }));
                tagCountText.Invoke(new MethodInvoker(delegate
                {
                    tagCountText.Text = tagList.Items.Count.ToString();
                }));
            }
        }

        private void startScanListener(bool status, string msg)
        {
            updateInfoStatus(msg);
            btnStartScan.Invoke(new MethodInvoker(delegate
            {
                btnStartScan.Enabled = false;
            }));
            btnLedOn.Invoke(new MethodInvoker(delegate
            {
                btnLedOn.Enabled = false;
            }));
            btnLedAllAtOnce.Invoke(new MethodInvoker(delegate
            {
                btnLedAllAtOnce.Enabled = false;
            }));
            btnLedOff.Invoke(new MethodInvoker(delegate
            {
                btnLedOff.Enabled = false;
            }));
            btnDisconnectEth.Invoke(new MethodInvoker(delegate
            {
                btnDisconnectEth.Enabled = false;
            }));
            btnDisconnect.Invoke(new MethodInvoker(delegate
            {
                btnDisconnect.Enabled = false;
            }));
        }

        private void disconnectDeviceUsbListener(bool status, string msg)
        {
            updateInfoStatus(msg);
            if (status)
            {
                deviceConnected = false;
                tagList.Invoke(new MethodInvoker(delegate
                {
                    tagList.Items.Clear();
                }));
                tagCountText.Invoke(new MethodInvoker(delegate
                {
                    tagCountText.Text = "0";
                }));
                txtIpAddress.Invoke(new MethodInvoker(delegate
                {
                    txtIpAddress.Enabled = true;
                }));
                usbDeviceSelection.Invoke(new MethodInvoker(delegate
                {
                    usbDeviceSelection.Enabled = true;
                }));
                btnDisconnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnectEth.Enabled = false;
                }));
                btnConnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnConnectEth.Enabled = true;
                }));
                btnDisconnect.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnect.Enabled = false;
                }));
                enableBtnConnectUsb();
                btnRefresh.Invoke(new MethodInvoker(delegate
                {
                    btnRefresh.Enabled = true;
                }));
                btnStartScan.Invoke(new MethodInvoker(delegate
                {
                    btnStartScan.Enabled = false;
                }));
                checkContineousMode.Invoke(new MethodInvoker(delegate
                {
                    checkContineousMode.Enabled = false;
                }));
                btnRefreshTags.Invoke(new MethodInvoker(delegate
                {
                    btnRefreshTags.Enabled = false;
                }));
                btnLedOn.Invoke(new MethodInvoker(delegate
                {
                    btnLedOn.Enabled = false;
                }));
                btnLedAllAtOnce.Invoke(new MethodInvoker(delegate
                {
                    btnLedAllAtOnce.Enabled = false;
                }));
                btnLedOff.Invoke(new MethodInvoker(delegate
                {
                    btnLedOff.Enabled = false;
                }));
            } else
            {
                btnDisconnect.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnect.Enabled = true;
                }));
            }
        }
        
        private void disconnectDeviceEthListener(bool status, string msg)
        {
            updateInfoStatus(msg);
            if (status)
            {
                deviceConnected = false;
                tagList.Invoke(new MethodInvoker(delegate
                {
                    tagList.Items.Clear();
                }));
                tagCountText.Invoke(new MethodInvoker(delegate
                {
                    tagCountText.Text = "0";
                }));
                txtIpAddress.Invoke(new MethodInvoker(delegate
                {
                    txtIpAddress.Enabled = true;
                }));
                usbDeviceSelection.Invoke(new MethodInvoker(delegate
                {
                    usbDeviceSelection.Enabled = true;
                }));
                btnDisconnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnectEth.Enabled = false;
                }));
                btnConnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnConnectEth.Enabled = true;
                }));
                btnDisconnect.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnect.Enabled = false;
                }));
                enableBtnConnectUsb();
                btnRefresh.Invoke(new MethodInvoker(delegate
                {
                    btnRefresh.Enabled = true;
                }));
                btnStartScan.Invoke(new MethodInvoker(delegate
                {
                    btnStartScan.Enabled = false;
                }));
                checkContineousMode.Invoke(new MethodInvoker(delegate
                {
                    checkContineousMode.Enabled = false;
                }));
                btnRefreshTags.Invoke(new MethodInvoker(delegate
                {
                    btnRefreshTags.Enabled = false;
                }));
                btnLedOn.Invoke(new MethodInvoker(delegate
                {
                    btnLedOn.Enabled = false;
                }));
                btnLedAllAtOnce.Invoke(new MethodInvoker(delegate
                {
                    btnLedAllAtOnce.Enabled = false;
                }));
                btnLedOff.Invoke(new MethodInvoker(delegate
                {
                    btnLedOff.Enabled = false;
                }));
            } else
            {
                btnDisconnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnectEth.Enabled = true;
                }));
            }
        }

        private void connectDeviceUsbListener(bool status, string msg)
        {
            updateInfoStatus(msg);
            if (status)
            {
                deviceConnected = true;
                txtIpAddress.Invoke(new MethodInvoker(delegate
                {
                    txtIpAddress.Enabled = false;
                }));
                usbDeviceSelection.Invoke(new MethodInvoker(delegate
                {
                    usbDeviceSelection.Enabled = false;
                }));
                btnDisconnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnDisconnectEth.Enabled = false;
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
                btnRefresh.Invoke(new MethodInvoker(delegate
                {
                    btnRefresh.Enabled = false;
                }));
                btnStartScan.Invoke(new MethodInvoker(delegate
                {
                    btnStartScan.Enabled = true;
                }));
                checkContineousMode.Invoke(new MethodInvoker(delegate
                {
                    checkContineousMode.Enabled = true;
                }));
            } else
            {
                enableBtnConnectUsb();
            }
        }

        private void connectDeviceEthListener(bool status, string msg)
        {
            updateInfoStatus(msg);
            if (status)
            {
                deviceConnected = true;
                txtIpAddress.Invoke(new MethodInvoker(delegate
                {
                    txtIpAddress.Enabled = false;
                }));
                usbDeviceSelection.Invoke(new MethodInvoker(delegate
                {
                    usbDeviceSelection.Enabled = false;
                }));
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
                    btnDisconnect.Enabled = false;
                }));
                btnConnect.Invoke(new MethodInvoker(delegate
                {
                    btnConnect.Enabled = false;
                }));
                btnRefresh.Invoke(new MethodInvoker(delegate
                {
                    btnRefresh.Enabled = false;
                }));
                btnStartScan.Invoke(new MethodInvoker(delegate
                {
                    btnStartScan.Enabled = true;
                }));
                checkContineousMode.Invoke(new MethodInvoker(delegate
                {
                    checkContineousMode.Enabled = true;
                }));
            } else
            {
                btnConnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnConnectEth.Enabled = true;
                }));
            }
        }

        private void updateUsbDeviceList(bool status, List<UsbDeviceModel> usbDevices, string msg)
        {
            socketConnected = true;
            updateInfoStatus(msg);
            btnRefresh.Invoke(new MethodInvoker(delegate
            {
                btnRefresh.Enabled = true;
            }));
            btnConnectEth.Invoke(new MethodInvoker(delegate
            {
                btnConnectEth.Enabled = true;
            }));
            usbDeviceList = usbDevices;
            foreach (var usbDevice in usbDevices)
            {
                usbDeviceSelection.Invoke(new MethodInvoker(delegate
                {
                    usbDeviceSelection.Items.Add(usbDevice.deviceId);
                }));
                enableBtnConnectUsb();
            }
        }

        private void enableBtnConnectUsb()
        {
            if (usbDeviceList.Count > 0 && usbDeviceSelection.Text.ToString().Trim() != "")
            {
                bool found = false;
                foreach (var device in usbDeviceList)
                {
                    if (!found)
                    {
                        if (device.deviceId == usbDeviceSelection.Text.ToString().Trim() && usbDeviceSelection.SelectedItem != null)
                        {
                            btnConnect.Invoke(new MethodInvoker(delegate
                            {
                                btnConnect.Enabled = true;
                            }));
                            found = true;
                        }
                        else
                        {
                            btnConnect.Invoke(new MethodInvoker(delegate
                            {
                                btnConnect.Enabled = false;
                            }));
                        }
                    }
                }
            }
        }

        private void onLFSocketError(string e)
        {
            updateInfoStatus(e);
        }

        private void onLFSocketConnected(bool status, List<string> socketList, string msg)
        {
            txtInfo.Invoke(new MethodInvoker(delegate
            {
                txtInfo.Text = msg;
            }));
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            updateInfoStatus("Connecting USB Device...");
            btnConnect.Invoke(new MethodInvoker(delegate
            {
                btnConnect.Enabled = false;
            }));
            var socketId = usbDeviceList[usbDeviceSelection.SelectedIndex].socketId;
            var deviceId = usbDeviceList[usbDeviceSelection.SelectedIndex].deviceId;
            lfDevice.connectUsbDevice(socketId, deviceId);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            usbDeviceSelection.Invoke(new MethodInvoker(delegate
            {
                usbDeviceSelection.Text = "";
            }));
            usbDeviceList.Clear();
            usbDeviceSelection.Items.Clear();
            lfDevice.deviceRefresh();
            btnConnect.Invoke(new MethodInvoker(delegate
            {
                btnConnect.Enabled = false;
            }));
            btnRefresh.Invoke(new MethodInvoker(delegate
            {
                btnRefresh.Enabled = false;
            }));
            updateInfoStatus("Refreshing devices...");
        }

        private void btnConnectEth_Click(object sender, EventArgs e)
        {
            btnConnectEth.Invoke(new MethodInvoker(delegate
            {
                btnConnectEth.Enabled = false;
            }));
            updateInfoStatus("Connecting Ethernet Device");
            lfDevice.connectEthDevice(txtIpAddress.Text.Trim());
        }

        private void btnDisconnectEth_Click(object sender, EventArgs e)
        {
            updateInfoStatus("Disconnecting Ethernet Device");
            btnDisconnectEth.Invoke(new MethodInvoker(delegate
            {
                btnDisconnectEth.Enabled = false;
            }));
            lfDevice.disconnectDeviceEth();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            updateInfoStatus("Disconnecting Ethernet Device");
            btnDisconnectEth.Invoke(new MethodInvoker(delegate
            {
                btnDisconnectEth.Enabled = false;
            }));
            var socketId = usbDeviceList[usbDeviceSelection.SelectedIndex].socketId;
            lfDevice.disconnectDeviceUsb(socketId);
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

            lfDevice.startScan(scanMode);
        }

        private void btnStopScan_Click(object sender, EventArgs e)
        {
            lfDevice.stopScan();
        }

        private void machineList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkContineousMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkContineousMode.Checked && inScan)
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
            lfDevice.refreshTag();
            btnRefreshTags.Invoke(new MethodInvoker(delegate
            {
                btnRefreshTags.Enabled = false;
            }));
        }

        private void btnLedOn_Click(object sender, EventArgs e)
        {
            if (tagList.SelectedItems.Count > 0)
            {
                updateInfoStatus("Turning LED ON...");
                btnLedOn.Invoke(new MethodInvoker(delegate
                {
                    btnLedOn.Enabled = false;
                }));
                btnLedAllAtOnce.Invoke(new MethodInvoker(delegate
                {
                    btnLedAllAtOnce.Enabled = false;
                }));
                var list = new List<string>();
                foreach (var tag in tagList.SelectedItems)
                {
                    list.Add(tag.ToString().Split(',')[1].Split('-')[1]);
                }
                lfDevice.ledOn(list);
            } else
            {
                MessageBox.Show("Please select some tags.");
            }
        }

        private void btnLedAllAtOnce_Click(object sender, EventArgs e)
        {
            updateInfoStatus("Turning LED ON...");
            btnLedOn.Invoke(new MethodInvoker(delegate
            {
                btnLedOn.Enabled = false;
            }));
            btnLedAllAtOnce.Invoke(new MethodInvoker(delegate
            {
                btnLedAllAtOnce.Enabled = false;
            }));
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
            if (usbDeviceList.Count > 0)
            {   
                if (usbDeviceSelection.SelectedItem != null)
                {
                    lfDevice.formClosing(usbDeviceList[usbDeviceSelection.SelectedIndex].socketId);
                }
            } else
            {
                lfDevice.formClosing(null);
            }
        }

        private void updateInfoStatus(string msg)
        {
            txtInfo.Invoke(new MethodInvoker(delegate
            {
                txtInfo.Text = msg;
            }));
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deviceConnected)
            {
                tabControl1.SelectTab(currentTabIndex);
            } else
            {
                currentTabIndex = tabControl1.SelectedIndex;
            }
        }

        private void usbDeviceSelection_OnTextUpdate(object sender, EventArgs e)
        {
            if (usbDeviceSelection.Text.ToString().Trim() == "")
            {
                btnConnect.Invoke(new MethodInvoker(delegate
                {
                    btnConnect.Enabled = false;
                }));
            } else
            {
                enableBtnConnectUsb();
            }
        }

        private void txtIpAddress_TextChanged(object sender, EventArgs e)
        {
            if (txtIpAddress.Text.ToString().Trim() == "")
            {
                btnConnectEth.Invoke(new MethodInvoker(delegate
                {
                    btnConnectEth.Enabled = false;
                }));
            } else
            {
                if (socketConnected)
                {
                    btnConnectEth.Invoke(new MethodInvoker(delegate
                    {
                        btnConnectEth.Enabled = true;
                    }));
                }
            }
         }

        private void usbDeviceSelection_SelectionIndexChanged(object sender, EventArgs e)
        {
            if (usbDeviceSelection.Text.ToString().Trim() == "")
            {
                btnConnect.Invoke(new MethodInvoker(delegate
                {
                    btnConnect.Enabled = false;
                }));
            }
            else
            {
                enableBtnConnectUsb();
            }
        }
    }
}
