using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
    /// UploadNewVersionWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class UploadNewVersionWindow : Window
    {
        int newVersion;
        string[] files;

        public UploadNewVersionWindow()
        {
            InitializeComponent();

            btnUpload.IsEnabled = false;

            newVersion = getNewestVersion() + 1;
            lblVersion.Text = "Version " + newVersion;

        }

        public int getNewestVersion()
        {
            try
            {
                string[] spec = { "||" };
                string[] result = Tools.request("http://akita123.atwebpages.com/main.php?type=10&userid=1").Split(spec, StringSplitOptions.None);
                return int.Parse(result[0]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    AddExtension = true,
                    CheckFileExists = true,
                    Multiselect = true,
                    Title = "Select File"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    files = openFileDialog.FileNames;
                    if (files.Length > 0)
                    {
                        lblPath.Text = "";
                        foreach (string file in files)
                        {
                            lblPath.Text += file + Environment.NewLine;
                        }

                        btnUpload.IsEnabled = true;
                    }
                    else
                    {
                        btnUpload.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task uploadFileAsync(string[] paths)
        {

            var hc = new HttpClient();

            var content = new MultipartFormDataContent();
            foreach (string path in paths)
            {
                var file = path;
                var dic = new Dictionary<string, string>();
                dic["FileName"] = System.IO.Path.GetFileName(file) + ".rar";

                var fileContent = new StreamContent(File.OpenRead(file));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = System.IO.Path.GetFileName(file) + ".rar",
                    Name = @"userfile[]"
                };

                foreach (var param in dic)
                {
                    content.Add(new StringContent(param.Value), param.Key);
                }
                content.Add(fileContent);
            }
            var url = "http://akita123.atwebpages.com/Upload.php";
            var req = await hc.PostAsync(url, content);
            var html = await req.Content.ReadAsStringAsync();


            setNewVersion(newVersion, files);
            MessageBox.Show(html);
            btnUpload.IsEnabled = true;
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            btnUpload.IsEnabled = false;
            uploadFileAsync(files);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void setNewVersion(int version, string[] files)
        {

            string link = "";

            foreach (string file in files)
            {
                link += ("http://akita123.atwebpages.com/folder/" + System.IO.Path.GetFileName(file) + ".rar|").Replace('/', ';');
            }

            link = link.Remove(link.Length - 1);

            Tools.request("http://akita123.atwebpages.com/main.php?type=11&version=" + version + "&link=" + link);
        }
    }
}
