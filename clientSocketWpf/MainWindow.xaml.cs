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
    public partial class MainWindow : Window
    {
        private IPAddress ip = IPAddress.Parse("127.0.0.1");
        private int port = 3535;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            Registraion registraionWindow = new Registraion();
            registraionWindow.Show();
            this.Close();
        }

        private async void BtnSignin_Click(object sender, RoutedEventArgs e)
        {
            if (!(ValidateRegistration()))
            {
                return;
            }

            //-------------------------------------------------
            //есть ли с таким логином и паролем
            string login = tbLogin.Text;
            string password = tbPassword.Text;

            //метод возваращающий exist если есть user с таким логином
            string request = "checklogandpswd:" + login + " " + password;
            CommandService commandService = new CommandService();
            string res = await commandService.SendToServer(request);

            if (res == "exist")
            {
                Menu menuWindow = new Menu(login);
                menuWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid login or pswrd");
            }

            //-------------------------------------------------

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

        private void TbLogin_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

            string login = tbLogin.Text;
            if (login.Length > 10)
            {
                login = login.Remove(login.Length - 1);
                tbLogin.Text = login;

                tbLogin.CaretIndex = tbLogin.Text.Length;
            }
        }

        private void TbPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

            
        }
    }
}
