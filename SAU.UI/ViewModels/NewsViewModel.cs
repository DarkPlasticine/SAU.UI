using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;
using SAU.UI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using Wpf.Ui.TaskBar;

namespace SAU.UI.ViewModels
{
    public partial class NewsViewModel : ObservableObject
    {
        private bool _dataInitialized = false;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private IEnumerable<NewsModel> _news;

        [ObservableProperty]
        private Visibility _visibleLoad;

        [ObservableProperty]
        private Visibility _visibleNews;

        [RelayCommand]
        private void GoPost(string link)
        {
            var sInfo = new System.Diagnostics.ProcessStartInfo(link)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }

        public NewsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            VisibleLoad = Visibility.Visible;
            VisibleNews = Visibility.Hidden;

            Task.Run(async () => await LoadNewsAsync());
        }

        private async Task LoadNewsAsync()
        {
            if (_dataInitialized)
                return;

            var url = new Uri(@"https://sirus.su/");

            //var options = new ChromeOptions();
            //options.AddArgument("--headless");

            //var html = new ChromeDriver(options).;
            //html.Navigate().GoToUrl(url);

            var test = new HtmlWeb();
            var jsonData = await test.LoadFromWebAsync("https://sirus.su/api/posts?lang=ru");
            var token = JToken.Parse(jsonData.Text)["data"];

            var tmpList = new List<NewsModel>();

            foreach (var item in token)
            {
                tmpList.Add(new NewsModel
                {
                    Head = item["title"].ToString(),
                    LinkImage = item["cover_url"].ToString(),
                    PostDate = DateTime.Parse(item["created_at"].ToString()).ToString("d", new CultureInfo(Application.Current.Resources["Culture"].ToString())),
                    Link = $"https://forum.sirus.su/threads/{item["forum_topic_id"]}/"
                });
            }           

            _dataInitialized = true;

            //await Task.Delay(4000);

            VisibleLoad = Visibility.Hidden;
            VisibleNews = Visibility.Visible;

            News = tmpList;
        }
    }
}
