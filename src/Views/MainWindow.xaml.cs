﻿using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.UserControls; 

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainVM MainVM { get; set; }

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                this.MainVM = new MainVM(this); 
                this.DataContext = this.MainVM;
                this.Menu.DataContext = this.MainVM; 
                this.SqlPage.DataContext = this.MainVM; 
                this.TablesPage.DataContext = this.MainVM; 
                
                this.MainVM.Config.Initialize(); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
    }
}
