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
    /// ProcessManager.xaml の相互作用ロジック
    /// </summary>
    public partial class ProcessManagerWindow : Window
    {
        public delegate void RequestListProcess();
        public delegate void KillProcess(string processname);

        public delegate void _Action();

        RequestListProcess requestListProcess;
        KillProcess killProcess;

        List<_Process> list = new List<_Process>();

        public ProcessManagerWindow(RequestListProcess requestListProcess, KillProcess killProcess)
        {
            InitializeComponent();
            this.requestListProcess = requestListProcess;
            this.killProcess = killProcess;

            lstProcess.ItemsSource = list;

            requestListProcess();
        }

        private void btnKillProcess_Click(object sender, RoutedEventArgs e)
        {
            killProcess((lstProcess.SelectedItem as _Process).processname);
        }

        public void setdata(string data)
        {
            string[] spec = { "|||" } ;
            string[] data1 = data.Split(spec, StringSplitOptions.None);

            list.Clear();

            for (int i = 0; i < data1.Length; i += 2)
            {

                _Process proc = new _Process(data1[i], data1[i + 1]);
                list.Add(proc);
            }

            lstProcess.Dispatcher.Invoke(new _Action(() =>
            {
                lstProcess.Items.Refresh();
            }));
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            requestListProcess();
        }
    }

    class _Process
    {
        public string title { get; }
        public string processname { get; }

        public _Process(string title, string processname)
        {
            this.title = title;
            this.processname = processname;
        }
    }
}
