using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

namespace MatchGame
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed; //Поле итератор для таймера 
        int matchesFound; //Поле итератор для правильных комбинаций

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }
        /// <summary>
        /// Отображает значение таймера в UI.
        /// Останавливает таймер при обнаружении всех парных изображение.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + "Play again?";
            }
        }

        
        private void SetUpGame()//Устанавливает значения для отображения UI
        {
            List<string> animalEmoji = new List<string>()
            {
                "🐵","🐵",
                "🐺","🐺",
                "🦓","🦓",
                "🐮","🐮",
                "🐷","🐷",
                "🐼","🐼",
                "🐰","🐰",
                "🦊","🦊",
                "🐶","🐶",
                "🦒","🦒",
                "🐭","🐭",
                "🐗","🐗",
                "🐲","🐲",
                "🦍","🦍",
                "🦛","🦛",
                "🦥","🦥",
            }; //Список изображений животных

            List<string> dubleAnimal = new List<string>(); //Резервный список 
                                                           //для сохранения пары выбраного
            Random rnd = new Random();
            //Переменная "счетчик" для подсчета количества выбраных животных
            int itr = 0; 

            //Цикл устанавливает значения для TextBlock в UI
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                //Пустая строка для хранения случайно выбранного животного.
                string nextEmoji = string.Empty;
                if (itr != 8) 
                {
                    //До восьми итераций, TextBlock получают свои значения из списка animalEmoji
                    if (textBlock.Name != "timeTextBlock")
                    {
                        textBlock.Visibility = Visibility.Visible;
                        int index = rnd.Next(animalEmoji.Count);
                        nextEmoji = animalEmoji[index];
                        textBlock.Text = nextEmoji;
                        animalEmoji.RemoveAt(index);
                        itr++;
                    }
                }
                else
                {
                    //После восьми итераций, TextBlock получают свои значения из резервного списка
                    //пар животных dubleAnimal
                    if (textBlock.Name != "timeTextBlock")
                    {
                        textBlock.Visibility = Visibility.Visible;
                        int index = rnd.Next(dubleAnimal.Count);
                        nextEmoji = dubleAnimal[index];
                        textBlock.Text = nextEmoji;
                        dubleAnimal.RemoveAt(index);
                        
                    }
                }

                //Находит пару для выбраного эмоджи в списке animalEmoji,
                //сохраняет её в резервный список и удаляет её из списка animalEmoji
                for (int i = 0; i < animalEmoji.Count; i++)
                {
                    if (nextEmoji == animalEmoji[i])
                    {
                        dubleAnimal.Add(animalEmoji[i]);
                        animalEmoji.RemoveAt(i);
                        break;
                    }
                }
            }

            timer.Start(); //Запускает таймер
            tenthsOfSecondsElapsed = 0; //Устанавливает начальное значение таймера
            matchesFound = 0; //Устанавливает начальное значение для количества правильных комбинаций
        }

        TextBlock lastTextBlockClicked; //Резервная ссылка для хранения выбранного эмоджи
        bool findingMatch = false; //Переменная индикатор первого нажатия на элемент TextBlock

        //Собите нажатия на TextBlock
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false) //Если обьект не был выбран
            {
                textBlock.Visibility = Visibility.Hidden; //Устанавливает значение видимости "скрытый"
                lastTextBlockClicked = textBlock; //Сохраняет выбраный эмоджи в резервную ссылку
                findingMatch = true; //Значение индекатора сообщает о том что пользователь выбрал первый эмоджи
            }
            else if (textBlock.Text == lastTextBlockClicked.Text) //Если второй выбраный эмоджи совпадает с первым
            {
                matchesFound++; //Увеличивает количество правильных комбинаций
                textBlock.Visibility = Visibility.Hidden; //Устанавливает значение видимости "скрытый"
                findingMatch = false; //Индикатор сообщает о том что пользователь еще не выбрал элемент для сравнения
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible; //Делает видимым выбраный TextBlock
                findingMatch = false; //Индикатор сообщает о том что пользователь еще не выбрал элемент для сравнения
            }
        }

        //Начинает новую игру при нажатии на таймер
        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
