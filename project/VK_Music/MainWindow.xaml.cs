using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VkNet;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using WMPLib;
using VkNet.Model.Attachments;
using VK_Music.Additional;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;



namespace VK_Music
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Uri[] qw;
        public VkApi api;
        public static ReadOnlyCollection<Audio> listStatic;
        public static WrapSong _listStaticSong;
        public List<Audio> listNoStatic;
        public static ListBox listBoxStatic;
        public static MediaPlayer player;
        public static ListWrapSong listSong;
        public static TextBlock _Artist_label;
        public static ToolTip tool;
        public static TextBlock _Duration;
        DispatcherTimer timer;

        public static WindowsMediaPlayer wmp = new WindowsMediaPlayer();

        public static bool PlayPause;

        public MainWindow()
        {
            InitializeComponent();
            api = new VkApi();
            player = new MediaPlayer();
        }

        public MainWindow(ListBox list)
        {
            listBoxSong = list;
        }

        private void In_account_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ExpanderMenu.IsExpanded = false;
            Authorization auth = new Authorization();
            auth.Owner = this;
            auth.ShowDialog();
            MainWindow.listBoxStatic = listBoxSong;
            listSong = new ListWrapSong(listStatic);
            wmp.settings.volume = 75;
        }

        private static Uri[] GlobalReplace(int check)
        {
            MainWindow main = new MainWindow(listBoxStatic);
            string pattern = "- ";
            string[] temp_uri;
            Uri[] ret = new Uri[2];
            string new_uri = "";

            if (check == 1)
            {
                foreach (var temp in listSong._ListAudioInfo)
                {
                    string pattern2 = " - ";
                    string[] substrings = Regex.Split(_Artist_label.Text, pattern2);
                    if (String.Compare(substrings[1], temp._AudioInfo.Title) == 0)
                    {
                        var temp2 = listSong.ReturnNext(temp._AudioInfo.Url);
                        if (temp2 == null) { break; }
                        temp_uri = temp2._AudioInfo.Url.ToString().Split('?');
                        _Artist_label.Text = temp2._AudioInfo.Artist + " - " + temp2._AudioInfo.Title;
                        new_uri = temp_uri[0].ToString().Replace("https", "http");
                        ret[1] = new Uri(temp_uri[0] + "?" + temp_uri[1]);
                        break;
                    }
                }
            }
            else if (check == 0)
            {
                string[] substrings = Regex.Split(main.listBoxSong.SelectedItem.ToString(), pattern);
                foreach (var temp in listSong._ListAudioInfo)
                {
                    if (String.Compare(substrings[1], temp._AudioInfo.Title) == 0)
                    {
                        temp_uri = temp._AudioInfo.Url.ToString().Split('?');
                        _Artist_label.Text = temp._AudioInfo.Artist + " - " + temp._AudioInfo.Title;
                        new_uri = temp_uri[0].ToString().Replace("https", "http");
                        ret[1] = new Uri(temp_uri[0] + "?" + temp_uri[1]);
                        break;
                    }
                }
            }
            Uri ur2 = new Uri("http://Example.com");
            if (new_uri == "")
            {
                ur2 = new Uri("http://Example.com");
            }
            else
            {
                ur2 = new Uri(new_uri);
            }

            ret[0] = ur2;

            return ret;
        }

        public static void StartClick(object sender, int check)
        {
            if (check == 1)
            {
                qw = GlobalReplace(1);
            }
            else
            {
                qw = GlobalReplace(0);
            }
            if (qw[0].ToString() != wmp.URL)
            {
                if (qw[0].ToString() != " ")
                {
                    wmp.URL = qw[0].ToString();
                }
                else if (qw[0].ToString() == "http://Example.com")
                {
                    wmp.controls.stop();
                }
            }
            if (listSong.CheckUrl(qw[1]) == true)
            {
                wmp.controls.pause();
            }
            else if (listSong.CheckUrl(qw[1]) == false)
            {
                wmp.controls.play();
            }
            listSong.InverseStartPause(qw[1]);

            tool.Content = _Artist_label.Text;
            _Artist_label.ToolTip = tool;
        }

        public static void StopClick(object sender, RoutedEventArgs e)
        {
            if (wmp.URL != "")
            {
                wmp.controls.stop();
                listSong.ChangeStartPause(qw[1], false);
            }
        }

        public static void InfoClick(object sender, RoutedEventArgs e)
        {
            OnTextBlockRightTapped(sender, e);
        }

        private static void OnTextBlockRightTapped(object sender, RoutedEventArgs args)
        {

            string pattern = "- ";
            string[] substrings = Regex.Split(listBoxStatic.SelectedItem.ToString(), pattern);
            WrapSong song = null;
            song = listSong.ForPopupa(substrings[1]);


            StackPanel stackPanel = new StackPanel();

            TextBlock text1 = new TextBlock
            {
                Margin = new Thickness(7, 7, 244, 0),
                TextWrapping = TextWrapping.Wrap,
                Height = 16,
                VerticalAlignment = VerticalAlignment.Top,
                FontWeight = FontWeights.Bold,
                Text = "Исполнитель - "
            };

            TextBlock text2 = new TextBlock
            {
                Margin = new Thickness(7, 8, 213, 0),
                TextWrapping = TextWrapping.Wrap,
                Height = 16,
                VerticalAlignment = VerticalAlignment.Top,
                FontWeight = FontWeights.Bold,
                Text = "Название - "
            };

            TextBlock text3 = new TextBlock
            {
                Margin = new Thickness(7, 9, 233, 0),
                TextWrapping = TextWrapping.Wrap,
                Height = 16,
                VerticalAlignment = VerticalAlignment.Top,
                FontWeight = FontWeights.Bold,
                Text = "Жанр - "
            };

            TextBlock text4 = new TextBlock
            {
                Margin = new Thickness(7, 10, 213, 0),
                TextWrapping = TextWrapping.Wrap,
                Height = 16,
                VerticalAlignment = VerticalAlignment.Top,
                FontWeight = FontWeights.Bold,
                Text = "Битрейт - "
            };

            TextBlock text5 = new TextBlock
            {
                Margin = new Thickness(7, 11, 213, 0),
                TextWrapping = TextWrapping.Wrap,
                Height = 16,
                VerticalAlignment = VerticalAlignment.Top,
                FontWeight = FontWeights.Bold,
                Text = "Продолжительность - "
            };

            TextBlock artist_lable = new TextBlock
            {
                Margin = new Thickness(98, -90.3, 100, 0),
                TextWrapping = TextWrapping.Wrap,
                Height = 16,
                VerticalAlignment = VerticalAlignment.Top,
                Text = song._AudioInfo.Artist
            };

            TextBlock name_lable = new TextBlock
            {
                Margin = new Thickness(78, -67.3, 100, 0),
                TextWrapping = TextWrapping.Wrap,
                Height = 16,
                VerticalAlignment = VerticalAlignment.Top,
                Text = song._AudioInfo.Title
            };

            TextBlock genre_lable = new TextBlock
            {
                Margin = new Thickness(54, -42.3, 100, 0),
                TextWrapping = TextWrapping.Wrap,
                Height = 16,
                VerticalAlignment = VerticalAlignment.Top,
                Text = song._AudioInfo.Genre.ToString()
            };

            TextBlock bitrete_lable = new TextBlock
            {
                Margin = new Thickness(67, -16.3, 100, 0),
                TextWrapping = TextWrapping.Wrap,
                Height = 16,
                VerticalAlignment = VerticalAlignment.Top,
                Text = song._AudioInfo.Duration.ToString()
            };


            stackPanel.Children.Add(text1);
            stackPanel.Children.Add(text2);
            stackPanel.Children.Add(text3);
            stackPanel.Children.Add(text4);
            stackPanel.Children.Add(artist_lable);
            stackPanel.Children.Add(name_lable);
            stackPanel.Children.Add(genre_lable);
            stackPanel.Children.Add(bitrete_lable);

            Border border = new Border
            {
                Child = stackPanel,
                Background = Brushes.AliceBlue,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(2),
                Padding = new Thickness(5)
            };

            Popup popup = new Popup
            {
                Child = border,
                Placement = PlacementMode.Mouse,
                PopupAnimation = PopupAnimation.Slide,
                StaysOpen = false
            };

            popup.IsOpen = true;
        }

        private void Slider_Volume_ValueChanged(object sender, EventArgs e)
        {
            wmp.settings.volume = Convert.ToInt32(Slider_Volume.Value);
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            tool = new System.Windows.Controls.ToolTip();

            _Artist_label = new TextBlock
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Margin = new Thickness(10, 5, 0, 0),
                Style = (Style)FindResource("TextBlockStyleTahomaBlack"),
                Width = 300,
            };
            
            _Duration = new TextBlock
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                Margin = new Thickness(305, 5, 0, 0),
                TextWrapping = TextWrapping.Wrap,
                Style = (Style)FindResource("TextBlockStyleTahomaBlack"),
                Width = 50
            };

            statusGrid.Children.Add(MainWindow._Artist_label);
            statusGrid.Children.Add(MainWindow._Duration);


            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(OnTimedEvent);
            timer.Start();
        }

        void OnTimedEvent(object sender, EventArgs e)
        {
            if (wmp.URL != "")
            {
                Slider_Song_ValueChanged(sender, e);
            }
        }

        private void Slider_Song_ValueChanged(object sender, EventArgs e)
        {
            if (wmp.URL != "")
            {
                Slider_Song.Maximum = Convert.ToInt32(wmp.currentMedia.duration);
                Slider_Song.Value = Convert.ToInt32(wmp.controls.currentPosition);

                int s = (int)(wmp.currentMedia.duration - wmp.controls.currentPosition);
                int m = s / 60;
                s = s - (m * 60);

                _Duration.Text = String.Format("-{0:D}:{1:D2}", m, s);

                if (s == 1)
                {
                        StartClick(sender, 1);
                }
            }
        }

        private void Slider_Song_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (wmp.URL != "")
            {
                wmp.controls.pause();
                wmp.controls.currentPosition = Slider_Song.Value;
                wmp.controls.play();
            }
        }
    }
}
