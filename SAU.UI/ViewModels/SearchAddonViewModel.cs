using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SAU.UI.Models;
using SAU.UI.Services.Interfaces;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json.Linq;
using Wpf.Ui.Common;
using static System.Windows.Forms.LinkLabel;
using Wpf.Ui.Mvvm.Contracts;

namespace SAU.UI.ViewModels
{
    public partial class SearchAddonViewModel: ObservableObject
    {
        [ObservableProperty]
        private List<Addon> _gitHubAddons;

        [ObservableProperty] private Visibility _visibleLoad = Visibility.Hidden;
        [ObservableProperty] private Visibility _visibleAddons = Visibility.Hidden;

        private readonly ISettingsApp _settingsApp;
        private readonly ISnackbarService _snackbarService;

        public SearchAddonViewModel()
        {
            VisibleLoad = Visibility.Visible;
            VisibleAddons = Visibility.Hidden;

            _settingsApp = App.GetService<ISettingsApp>();
            _snackbarService = App.GetService<ISnackbarService>();

            

            _ = GetAddons();
        }

        private async Task GetAddons()
        {
            try
            {

                var client = new HtmlWeb();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                foreach (var source in _settingsApp.GitHubSources)
                {
                    //https://api.github.com/users/fxpw/repos - github api
                    //Link = $"https://github.com/fxpw",

                    var link = new StringBuilder();

                    link.Append("https://api.github.com/users");
                    link.Append(source.Link.Trim('/').Substring(source.Link.LastIndexOf("/")));
                    link.Append("/repos?language=lua");

                    var json = await client.LoadFromWebAsync(link.ToString());
                    var jObject = JArray.Parse(json.Text);

                    foreach (var item in jObject)
                    {
                        var addonClient = new HtmlWeb();

                        var data = await addonClient.LoadFromWebAsync(item["contents_url"]?.ToString().Replace("{+path}", ""));
                        var jaArray = JToken.Parse(data.Text);

                        var md = jaArray.FirstOrDefault(f => f["name"].ToString().Equals("README.md"))!["url"]!.ToString();

                        if (!string.IsNullOrEmpty(md))
                        {
                            var mdDocument = await addonClient.LoadFromWebAsync(md);
                            var jtMd = Base64Decode(JToken.Parse(mdDocument.Text)["content"]!.ToString());
                        }
                    }

                    var temp = jObject;
                }
            }
            catch (Exception e)
            {
                await _snackbarService.ShowAsync(e.GetType().Name, e.Message, SymbolRegular.ErrorCircle24, ControlAppearance.Danger);
            }
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            var tmpString = Encoding.UTF8.GetString(base64EncodedBytes).Split(Environment.NewLine);
            //var rStr = tmpString.se

            return string.Empty;
        }
    }
}
