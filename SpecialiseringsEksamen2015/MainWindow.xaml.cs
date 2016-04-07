using System;
using System.Windows;
using System.Windows.Controls;
using SpecialiseringsEksamen2015.Pages;

namespace SpecialiseringsEksamen2015
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Content = new MainMenu(this);
        }
    }
}