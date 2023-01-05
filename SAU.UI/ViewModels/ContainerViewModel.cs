using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using SAU.UI.Models;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace SAU.UI.ViewModels
{
    public partial class ContainerViewModel: ObservableObject
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        [ObservableProperty]
        private IEnumerable<Realm> _realms;

        [ObservableProperty]
        private int _allOnline;

        [ObservableProperty]
        private System.Windows.Media.Brush _onlineColor = System.Windows.Media.Brushes.DarkRed;

        [RelayCommand]
        private void ForceUpdate()
        {
            if (!_isUpdated)
                _ = GetOnlineInfo();
        }

        private bool _isUpdated = false;

        private readonly ISnackbarService _snackbarService;

        public ContainerViewModel()
        {
            _snackbarService = App.GetService<ISnackbarService>();
            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += async (s, e) => await GetOnlineInfo();
            _timer.Start();

            if (!_isUpdated)
                _ = GetOnlineInfo();
        }

        private async Task GetOnlineInfo()
        {
            HtmlDocument jsonData;

            try
            {
                _isUpdated = true;
                var test = new HtmlWeb();
                jsonData = await test.LoadFromWebAsync("https://sirus.su/api/statistic/tooltip");
                var realms = JToken.Parse(jsonData.Text)["realms"];
                var online = JToken.Parse(jsonData.Text)["online_count"];

                if (online is not null)
                    AllOnline = Convert.ToInt32(online);

                OnlineColor = AllOnline > 0
                    ? System.Windows.Media.Brushes.ForestGreen
                    : System.Windows.Media.Brushes.DarkRed;

                var tmpList = new List<Realm>();

                foreach (var item in realms)
                {
                    tmpList.Add(new Realm()
                    {
                        Name = item["name"]!.ToString(),
                        Id = (int) item["id"],
                        IsOnline = (bool) item["isOnline"],
                        Online = (int) item["online"],
                        Color = (bool) item["isOnline"]
                            ? System.Windows.Media.Brushes.ForestGreen
                            : System.Windows.Media.Brushes.DarkRed,
                        Command = ForceUpdateCommand
                    });
                }

                if (tmpList.Any())
                    Realms = tmpList;
            }
            catch (Exception e)
            {
                await _snackbarService.ShowAsync(e.GetType().Name, e.Message, SymbolRegular.Info28,
                    ControlAppearance.Caution);
            }
            finally
            {
                _isUpdated = false;
            }
            
        }
    }
}