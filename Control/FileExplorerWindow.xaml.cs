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

        public FileExplorerWindow(int touserid, int sessionid)
        {
            InitializeComponent();

            this.touserid = touserid;
            this.sessionid = sessionid;

            Tools.sendCommand("FILEEXPLORER", touserid, sessionid);
            enableControl(false);
            btnRun.IsEnabled = false;
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
            lstExplorer.IsEnabled = btnDelete.IsEnabled = btnRename.IsEnabled = en;

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

            index++;

            lstExplorer.Items.Clear();

            List<FileItem> lstFile = new List<FileItem>();

            if (index > 0)
            {
                FileItem fileItem;
                fileItem.name = "...";
                fileItem.path = path;
                fileItem.isFolder = true;
                lstFile.Add(fileItem);

                lstExplorer.Items.Add(fileItem.name);
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

                lstExplorer.Items.Add(fileItem.name);
            }

            lstFiles.Add(lstFile);

            if (index > 0)
            {
                enableControl(true);
            }
            else
            {
                enableControl(false);
                lstExplorer.IsEnabled = true;
            }

            btnRun.IsEnabled = false;
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            string path = lstFiles[index][lstExplorer.SelectedIndex].path.Replace('\\', ';');
            Tools.sendCommand("RUN " + path, touserid, sessionid);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string path = lstFiles[index][lstExplorer.SelectedIndex].path.Replace('\\', ';');
            Tools.sendCommand("DELETE " + path, touserid, sessionid);
        }

        private void lstExplorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (lstExplorer.SelectedIndex > 0 || (lstExplorer.SelectedIndex == 0 && index == 0))
            {
                int selectIndex = lstExplorer.SelectedIndex;

                if (lstFiles[index][selectIndex].isFolder)
                {
                    Tools.sendCommand("FILEEXPLORER " + lstFiles[index][selectIndex].path.Replace('\\', ';'), touserid, sessionid);
                    path = lstFiles[index][selectIndex].path;
                    lblPath.Text = "Path: " + path;

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
            }
        }
    }
}
