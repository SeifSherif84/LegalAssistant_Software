using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.New
{
    public class SourceInfoResponse
    {
        public string Filename { get; set; } = string.Empty;
        public string? Article { get; set; }
        public List<string> ReferencedArticles { get; set; } = [];
        public double RetrievalScore { get; set; }
        public double RerankScore { get; set; }

        /// <summary>
        /// True when source == "[article-lookup rescue]".
        /// UI may show a separate "extra references found" indicator.
        /// </summary>
        public bool IsArticleLookupRescue { get; set; }
    }
}
