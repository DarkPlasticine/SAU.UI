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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SAU.UI.ViewModels;
using Wpf.Ui.Common.Interfaces;

namespace SAU.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : INavigableView<HomeViewModel>
    {
        public HomeViewModel ViewModel { get; }

        public Home(HomeViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

        
    }
}
