using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class StatWindow : Window
    {
        private string _login;
        public StatWindow(string login)
        {
            InitializeComponent();
            _login = login;

            lblUserName.Content = "User: " + _login;
            RenderDataGridAsync();

        }

        private async void RenderDataGridAsync()
        {
            List<Statistic> statRecords = new List<Statistic>();
            string request = "getAllStat:" + _login; // getAllStat:abai

            CommandService commandService = new CommandService();
            string json = await commandService.SendToServer(request);

            statRecords = JsonConvert.DeserializeObject<List<Statistic>>(json);
            myDataGrid.ItemsSource = statRecords;
        }

        private void BtnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Menu menuWindow = new Menu(_login);
            menuWindow.Show();
            this.Close();
        }
    }
}
