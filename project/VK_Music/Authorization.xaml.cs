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
using VkNet;
using System.IO;

namespace VK_Music
{
	/// <summary>
	/// Логика взаимодействия для Authorization.xaml
	/// </summary>
	public partial class Authorization : Window
	{
        MainWindow main;
        bool Check_flag_save = false;
		public Authorization()
		{
			this.InitializeComponent();
			
			// Вставьте ниже код, необходимый для создания объекта.
		}

		private void cancel_button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Close();
		}

        private void input_button_Click(object sender, RoutedEventArgs e)
        {
            main = this.Owner as MainWindow;
            if (Check_flag_save == true)
            {
                CheckFile();
            }
            Authorization_method();
            this.Close();
        }

        public void Authorization_method()
        {
            var param = new ApiAuthParams();
            param.Login = login_box.Text;
            param.Password = password_box.Password;
            param.ApplicationId = 5232865;
            param.Settings = VkNet.Enums.Filters.Settings.All;

            main.api.Authorize(param);

            var field = new VkNet.Enums.Filters.ProfileFields();
            var info_user = main.api.Users.Get(Convert.ToInt32(main.api.UserId), field);

            main.first_second_name.Content = info_user.FirstName + " " + info_user.LastName;

            MainWindow.listStatic = main.api.Audio.Get(Convert.ToUInt32(main.api.UserId));
            main.listBoxSong.Items.Clear();
            foreach (var temp in MainWindow.listStatic)
            {
                ListBoxItem my = new ListBoxItem();
                my.Content = "  " + temp.Artist + " - " + temp.Title;
                my.Style = (Style)FindResource("SongItem");
                main.listBoxSong.Items.Add(my);
            }
        }

        private void checkboxSave_Checked(object sender, RoutedEventArgs e)
        {
            Check_flag_save = true;
        }

        public void CheckFile()
        {
            if (Directory.Exists("Account"))
            {
                if (File.Exists("auth.ryz"))
                {
                    ReadBinaryFile();
                }
                else if (!File.Exists("auth.ryz"))
                {
                    WriteBinaryFile();
                }
            }
            else if (!Directory.Exists("Account"))
            {
                Directory.CreateDirectory("Account");
                WriteBinaryFile();
            }
        }

        public bool CheckFile2()
        {
            if (Directory.Exists("Account"))
            {
                if (File.Exists("Account/auth.ryz"))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public void WriteBinaryFile() 
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("Account/auth.ryz", FileMode.Create)))
            {
                writer.Write(login_box.Text);
                writer.Write(password_box.Password);
            }
        }
        public void ReadBinaryFile()
        {
            using (BinaryReader reader = new BinaryReader(File.Open("Account/auth.ryz", FileMode.Open)))
            {
                login_box.Text = reader.ReadString();
                password_box.Password = reader.ReadString();
            }
        }
	}
}