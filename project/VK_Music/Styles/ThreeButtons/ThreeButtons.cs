using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Collections;
using System.Text.RegularExpressions;

namespace VK_Music
{
    public partial class ThreeButtons
    {
        public ThreeButtons()
        {

        }

        void StartClick(object sender, RoutedEventArgs e)
        {
            MainWindow.StartClick(sender, 0);
        }

        void StopClick(object sender, RoutedEventArgs e)
        {
            MainWindow.StopClick(sender, e);
        }

        void InfoClick(object sender, RoutedEventArgs e)
        {
            MainWindow.InfoClick(sender, e);
        }
    }
}
