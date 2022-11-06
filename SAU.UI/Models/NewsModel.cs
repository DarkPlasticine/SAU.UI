using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAU.UI.Models
{
    public class NewsModel
    {
        /// <summary>
        /// Дата обупликования новости
        /// </summary>
        public string PostDate { get; set; }

        /// <summary>
        /// Ссылка на новость
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Заголовок новости
        /// </summary>
        public string Head { get; set; }

        /// <summary>
        /// Краткое описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string LinkImage { get; set; }
    }
}
