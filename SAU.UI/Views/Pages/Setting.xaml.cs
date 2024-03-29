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
using SAU.UI.ViewModels;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace SAU.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : INavigableView<SettingViewModel>
    {
        public Setting(SettingViewModel viewModel, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            

            InitializeComponent();
        }

        public SettingViewModel ViewModel { get; }
    }
}
