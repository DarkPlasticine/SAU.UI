namespace SAU.UI.Models
{
    public class GitHubSource
    {
        /// <summary>
        /// Флаг включения для сканирования реопзитория
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Ссылка на репозиторий
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Количество аддонов
        /// </summary>
        public int Count { get; set; } = 0;
    }
}