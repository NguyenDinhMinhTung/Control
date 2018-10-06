using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Control
{
    /// <summary>
    /// ViewKeyLog.xaml の相互作用ロジック
    /// </summary>
    public partial class ViewKeyLogWindow : Window
    {
        int touserid;
        int sessionid, sessionidmax;

        public ViewKeyLogWindow(int touserid, int sessionidmax)
        {
            InitializeComponent();

            this.touserid = touserid;
            this.sessionidmax = sessionidmax;

            lblSessionID.Text = "Session ID (MAX = " + sessionidmax + "):";

            this.Title = "VIEW KEY LOG (USERID=" + touserid + ")";
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtSessionid.Text, out sessionid))
            {
                string log = Tools.request("http://akita123.atwebpages.com/main.php?type=8&userid=" + touserid + "&ssid=" + sessionid);
                txtLog.SelectAll();
                txtLog.Selection.Text = log;

            }
            else
            {
                MessageBox.Show("ERROR");
            }
        }
    }
}
