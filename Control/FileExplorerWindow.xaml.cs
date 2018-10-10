using System;
using System.Collections.Generic;
using System.IO;
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
    /// FileExplorerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FileExplorerWindow : Window
    {
        delegate void _Action();

        struct FileItem
        {
            public string path;
            public string name;
            public Boolean isFolder;
        }

        List<List<FileItem>> lstFiles = new List<List<FileItem>>();
        int index = -1;

        int touserid, sessionid;

        string path = "";
        string nextPath = "";

        public FileExplorerWindow(int touserid, int sessionid)
        {
            InitializeComponent();

            this.touserid = touserid;
            this.sessionid = sessionid;

            Tools.sendCommand("FILEEXPLORER", touserid);
            enableControl(false);
            btnRun.IsEnabled = false;
        }

        public void enableBtnRun(bool enable)
        {
            btnRun.Dispatcher.Invoke(new _Action(() => { btnRun.IsEnabled = enable; }));
        }

        public void enableLstExplorer(bool enable)
        {
            lstExplorer.Dispatcher.Invoke(new _Action(() => { lstExplorer.IsEnabled = enable; }));
        }

        public void enableBtnDelete(bool enable)
        {
            btnDelete.Dispatcher.Invoke(new _Action(() => { btnDelete.IsEnabled = enable; }));
        }

        public void enableBtnRename(bool enable)
        {
            btnRename.Dispatcher.Invoke(new _Action(() => { btnRename.IsEnabled = enable; }));
        }

        public void enableBtnCancel(bool enable)
        {
            btnCancel.Dispatcher.Invoke(new _Action(() => { btnCancel.IsEnabled = enable; }));
        }

        private void lstExplorer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstExplorer.SelectedIndex >= 0)
            {
                if (lstFiles[index][lstExplorer.SelectedIndex].isFolder)
                {
                    btnRun.IsEnabled = false;
                }
                else
                {
                    btnRun.IsEnabled = true;
                }
            }
            else
            {
                enableControl(false);
            }
        }

        private void enableControl(Boolean en)
        {
            enableLstExplorer(en);
            enableBtnDelete(en);
            enableBtnRename(en);
        }

        private string getName(string path)
        {
            if (path.LastIndexOf('\\') > 0)
                return path.Substring(path.LastIndexOf('\\') + 1);
            else
                return path;
        }

        public void update(string cmd)
        {
            string[] paths = cmd.Split('|');

            path = nextPath;
            lblPath.Dispatcher.Invoke(new _Action(() => { lblPath.Text = "Path: " + path; }));

            index++;

            lstExplorer.Dispatcher.Invoke(new _Action(() =>
            {
                lstExplorer.Items.Clear();
            }));

            List<FileItem> lstFile = new List<FileItem>();

            if (index > 0)
            {
                FileItem fileItem;
                fileItem.name = "...";
                fileItem.path = path;
                fileItem.isFolder = true;
                lstFile.Add(fileItem);

                lstExplorer.Dispatcher.Invoke(new _Action(() => { lstExplorer.Items.Add(fileItem.name); }));
            }

            Boolean isFolder = true;

            foreach (string s in paths)
            {
                if (s.ToUpper().Equals("BEGINFILELIST"))
                {
                    isFolder = false;
                    continue;
                }

                FileItem fileItem;
                fileItem.name = index > 0 ? getName(s) : s;
                fileItem.path = s;
                fileItem.isFolder = isFolder;
                lstFile.Add(fileItem);

                lstExplorer.Dispatcher.Invoke(new _Action(() =>
                {
                    lstExplorer.Items.Add(fileItem.name);
                }));
            }

            lstFiles.Add(lstFile);

            if (index > 0)
            {
                enableControl(true);
            }
            else
            {
                enableControl(false);
                lstExplorer.Dispatcher.Invoke(new _Action(() => { lstExplorer.IsEnabled = true; }));
            }

            //btnRun.IsEnabled = false;
            enableBtnRun(false);
            enableBtnCancel(false);
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            string path = lstFiles[index][lstExplorer.SelectedIndex].path.Replace('\\', ';');
            Tools.sendCommand("RUN " + path, touserid);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string path = lstFiles[index][lstExplorer.SelectedIndex].path.Replace('\\', ';');
            Tools.sendCommand("DELETE " + path, touserid);
        }

        private void access(int selectIndex)
        {
            if (selectIndex > 0 || (selectIndex == 0 && index == 0))
            {
                if (lstFiles[index][selectIndex].isFolder)
                {
                    Tools.sendCommand("FILEEXPLORER " + lstFiles[index][selectIndex].path.Replace('\\', ';'), touserid);
                    btnCancel.IsEnabled = true;
                    nextPath = lstFiles[index][selectIndex].path;

                    enableControl(false);
                    btnRun.IsEnabled = false;
                }
            }
            else if (lstExplorer.SelectedIndex == 0 && index > 0)
            {
                lstFiles.RemoveAt(index);

                index--;

                List<FileItem> list = lstFiles[index];

                lstExplorer.Items.Clear();

                if (index > 0)
                {
                    path = list[0].path;
                }
                else
                {
                    path = "";
                }

                lblPath.Text = "Path: " + path;

                foreach (FileItem file in list)
                {
                    lstExplorer.Items.Add(file.name);
                }

                enableControl(true);
                btnRun.IsEnabled = false;
                btnCancel.IsEnabled = false;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //if (index > 0) access(0);

            enableControl(true);
        }

        private void lstExplorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            access(lstExplorer.SelectedIndex);
        }
    }
}
