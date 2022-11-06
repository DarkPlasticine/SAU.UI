using SAU.UI.ViewModels;
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
using Wpf.Ui.Common.Interfaces;

namespace SAU.UI.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для News.xaml
    /// </summary>
    public partial class News: INavigableView<NewsViewModel>
    {
        public NewsViewModel ViewModel { get; }

        public News(NewsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void CardAction_Click(object sender, RoutedEventArgs e)
        {
            var args = (sender as Wpf.Ui.Controls.CardAction);
            ViewModel.GoPostCommand.Execute(args.CommandParameter);
        }
    }
}
