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
    public partial class EditContentWindow : Window
    {
        private string _login;
        public EditContentWindow(string login)
        {
            try
            {
                InitializeComponent();
                _login = login;

                RenderDataGridAsync();
            }
            catch (Exception)
            {

                MessageBox.Show("Opps, the program thoughtful a bit. Press \"MainMenu\" and click \"Edit Content\" again");
            }
            
        }

        private async void RenderDataGridAsync()
        {
            List<Content> records = new List<Content>();
            string request = "getAllContent:" + _login;

            CommandService commandService = new CommandService();
            string json = await commandService.SendToServer(request);

            records = JsonConvert.DeserializeObject<List<Content>>(json);
            myDataGrid.ItemsSource = records;
        }

        private async void BtnAdd_ClickAsync(object sender, RoutedEventArgs e)
        {
            string eng = tbEng.Text;
            string rus = tbRus.Text;
            if (string.IsNullOrEmpty(eng) || string.IsNullOrEmpty(rus))
            {
                MessageBox.Show("Please, fill 'Eng' and 'Rus' fields for adding");
                return;
            }

            //добавление контента в табл Content
            string request = "postContent:" + eng + "#" + rus + "#" + _login; //postContent:Eng#Rus#abai

            CommandService commandService = new CommandService();
            await commandService.SendToServer(request);

            RenderDataGridAsync();
        }

        private async void BtnUpdate_ClickAsync(object sender, RoutedEventArgs e)
        {
            string id = tbId.Text;
            string eng = tbEng.Text;
            string rus = tbRus.Text;
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(eng) || string.IsNullOrEmpty(rus))
            {
                MessageBox.Show("Please, fill 'Id', 'Eng' and 'Rus' fields for updating");
                return;
            }

            //обновление контента в табл Content
            string request = "updateContent:" + id + "#" + eng + "#" + rus + "#" + _login; //updateContent:Id#Eng#Rus#abai

            CommandService commandService = new CommandService();
            await commandService.SendToServer(request);

            RenderDataGridAsync();
        }

        private async void BtnDel_ClickAsync(object sender, RoutedEventArgs e)
        {
            string id = tbId.Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Please, fill 'Id' field for deleting");
                return;
            }

            //удаление контента в табл Content
            string request = "deleteContent:" + id; //deleteContent:Id

            CommandService commandService = new CommandService();
            await commandService.SendToServer(request);

            RenderDataGridAsync();
        }

        private void TbId_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void TbId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //только числа
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Menu menuWindow = new Menu(_login);
            menuWindow.Show();
            this.Close();
        }
    }
}
