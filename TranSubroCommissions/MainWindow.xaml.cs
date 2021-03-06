﻿using System;
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
using SourceChord.FluentWPF;
namespace TranSubroCommissions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow( ) 
        {
            InitializeComponent();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Visibility = Visibility.Visible;
            SettingsButtons.Visibility = Visibility.Visible;
        }
        private void CloseSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Visibility = Visibility.Hidden;
            SettingsButtons.Visibility = Visibility.Hidden;
        }
        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsButtons.Visibility = Visibility.Hidden;
            SettingsPanel.Visibility = Visibility.Hidden;
        }
    }
}
