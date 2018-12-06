using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace clientSocketWpf
{
    public partial class Menu : Window
    {
        private string _login;

        public Menu(string login)
        {
            InitializeComponent();

            _login = login;
            lblUserName.Content = login;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BtnStartTest_Click(object sender, RoutedEventArgs e)
        {
            Test testWindow = new Test(_login);
            testWindow.Show();
            this.Close();
        }

        private void BtnAddContent_Click(object sender, RoutedEventArgs e)
        {
            EditContentWindow editContentWindow = new EditContentWindow(_login);
            editContentWindow.Show();
            this.Close();
        }

        private void BtnStat_Click(object sender, RoutedEventArgs e)
        {
            StatWindow statWindow = new StatWindow(_login);
            statWindow.Show();
            this.Close();
        }
    }
}
