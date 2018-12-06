using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class Registraion : Window
    {
        private IPAddress ip = IPAddress.Parse("127.0.0.1");
        private int port = 3535;

        public Registraion()
        {
            InitializeComponent();

        }

        private async void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (!(ValidateRegistration()))
            {
                return;
            }

            string login = tbLogin.Text;
            string password = tbPassword.Text;

            //метод возваращающий exist если есть user с таким логином
            string request = "checklogin:" + login;
            CommandService commandService = new CommandService();
            string res = await commandService.SendToServer(request);

            if (res == "exist")
            {
                MessageBox.Show("User with current Login is exist");
                return;
            }
            else
            {
                request = "";
                request = "Add:" + login + " " + password; //Add:abai 12345
                await commandService.SendToServer(request);
                MessageBox.Show("Registration success");
            }

            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private bool ValidateRegistration()
        {
            if (string.IsNullOrEmpty(tbLogin.Text) || string.IsNullOrEmpty(tbPassword.Text))
            {
                MessageBox.Show("Please, fill the all poles");
                return false;
            }
            return true;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void BtnRegistration_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void TbLogin_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

            
        }

        private void TbPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

            
        }
    }
}
