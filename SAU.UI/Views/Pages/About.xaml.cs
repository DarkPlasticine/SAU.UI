using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Wpf.Ui.Mvvm.Contracts;

namespace SAU.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About
    {
        public About(ISnackbarService snackbarService, IDialogService dialogService)
        {
            InitializeComponent();
        }

        private void CardAction_Click(object sender, RoutedEventArgs e)
        {
            var url = @"https://github.com/DarkPlasticine/SAU.UI";
            var sInfo = new System.Diagnostics.ProcessStartInfo(url)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }
    }
}
