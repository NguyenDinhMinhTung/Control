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
    /// UserManagerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class UserManagerWindow : Window
    {
        List<User> list;
        public UserManagerWindow()
        {
            InitializeComponent();

            updateList();
        }

        private void updateList()
        {
            list = Tools.getListUser();

            lstUser.ItemsSource = list;

            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            txtName.IsEnabled = false;
        }

        private void lstUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstUser.SelectedIndex >= 0)
            {
                txtName.Text = list[lstUser.SelectedIndex].name;
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
                txtName.IsEnabled = true;
            }
            else
            {
                txtName.Text = "";
                txtName.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Tools.updateUsername(list[lstUser.SelectedIndex].id, txtName.Text);
            updateList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("DELETE USER " + list[lstUser.SelectedIndex].name + "?", "WARNING", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Tools.sendCommand("DESTROY", list[lstUser.SelectedIndex].id, 0);
                updateList();
            }
        }
    }
}
