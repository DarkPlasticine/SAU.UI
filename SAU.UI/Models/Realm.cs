#nullable enable
using CommunityToolkit.Mvvm.Input;

namespace SAU.UI.Models
{
    public class Realm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public int Online { get; set; }
        public System.Windows.Media.Brush Color { get; set; }
        public IRelayCommand? Command { get; set; }
    }
}