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
        }

        private void lstUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstUser.SelectedIndex >= 0)
            {
                txtName.Text = list[lstUser.SelectedIndex].name;
                lblId.Text = "ID: " + list[lstUser.SelectedIndex].id;
                btnUpdate.IsEnabled = true;
            }
            else
            {
                txtName.Text = "";
                btnUpdate.IsEnabled = false;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Tools.updateUsername(list[lstUser.SelectedIndex].id, txtName.Text);
            updateList();
        }
    }
}
