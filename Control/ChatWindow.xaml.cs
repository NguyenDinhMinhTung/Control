using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Control
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ChatWindow : Window
    {
        public delegate void SendMessage(string mess);

        int userID = 1;
        SendMessage sendMessage;

        public ChatWindow(SendMessage sendMessage)
        {
            InitializeComponent();
            this.sendMessage = sendMessage;
        }

        private void send()
        {
            if (txtMessage.Text != "")
            {
                sendMessage(txtMessage.Text);
                txtChatBox.AppendText("SERVER: " + txtMessage.Text + Environment.NewLine);
                txtChatBox.ScrollToEnd();
                txtMessage.Text = "";
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            send();
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                send();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
