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
    public partial class Test : Window
    {
        private string _login;
        private List<Content> sentences;
        private int _rightAnswer;
        private Question4Answers _currentQA;
        private int questionNumber = 0;
        private int rightAnswersCount = 0;
        private string statistic;

        public Test(string login)
        {
            InitializeComponent();
            
            _login = login;
            lblUser.Content = _login;

            InitAsync();
        }

        private async void InitAsync()
        {
            //рандомно формируется 30 предложений из БД
            string request = "get30:" + _login;
            CommandService commandService = new CommandService();
            string json = await commandService.SendToServer(request);
            sentences = JsonConvert.DeserializeObject<List<Content>>(json);

            PutQuestionOnUI();
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            Handler(sender);
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            Handler(sender);
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            Handler(sender);
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            Handler(sender);
        }

        private void Handler(object sender)
        {
            //кол-во правильных ответов
            if ((sender as Button).Content.ToString() == _currentQA.Answers[_currentQA.RightAnswer])
            {
                rightAnswersCount++;
            }

            if (questionNumber < 30)
            {
                try
                {
                    PutQuestionOnUI();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Opps, last question was eated by program=) If you want continue pass the quiz press Finish test and start it again, else just press Finish test.");
                }
                
                
            }
            else
            {
                statistic = rightAnswersCount + "/30(" + rightAnswersCount * 100 / 30 + "%)";
                MessageBox.Show("Your Result: " + statistic);
                btn1.IsEnabled = false;
                btn2.IsEnabled = false;
                btn3.IsEnabled = false;
                btn4.IsEnabled = false;
            }
        }

        private void PutQuestionOnUI()
        {
            _currentQA = new Question4Answers();
            _currentQA.Question = sentences[questionNumber].Eng;
            string[] answers = new string[4];
            _currentQA.RightAnswer = new Random((int)DateTime.Now.Ticks).Next(0, 3);
            answers[_currentQA.RightAnswer] = sentences[questionNumber].Rus;

            List<int> nums = GetThreeUniqRandomNums();
            
            int j = 0;
            for (int i = 0; i < 4; i++)
            {
                if (i != _currentQA.RightAnswer)
                {
                    answers[i] = sentences[nums[j++]].Rus;
                }
            }
            _currentQA.Answers = answers;

            tbQuestion.Text = _currentQA.Question;
            btn1.Content = _currentQA.Answers[0];
            btn2.Content = _currentQA.Answers[1];
            btn3.Content = _currentQA.Answers[2];
            btn4.Content = _currentQA.Answers[3];

            lblQuestNumber.Content = (questionNumber + 1) + "/30";
            Console.WriteLine(questionNumber);
            Console.WriteLine(sentences.Count);
            questionNumber++;
        }

        private static List<int> GetThreeUniqRandomNums()
        {
            List<int> randomList = new List<int>();
            int maxRange = 29; //всего 30 вопросов
            while (randomList.Count <= 5)
            {
                Random rnd = new Random((int)DateTime.Now.Ticks);
                int number = rnd.Next(1, maxRange);
                if (!randomList.Contains(number))
                    randomList.Add(number);
            }

            return randomList.GetRange(0, 3); ;
        }

        private void BtnFinishTest_Click(object sender, RoutedEventArgs e)
        {
            //запись статистики
            statistic = rightAnswersCount + "/" + questionNumber + "(" + rightAnswersCount * 100 / questionNumber + "%)";
            string request = "postStat:"+ _login + " " + statistic;
            CommandService commandService = new CommandService();
            commandService.SendToServer(request);

            Menu menuWindow = new Menu(_login);
            menuWindow.Show();
            this.Close();
        }
    }
}
