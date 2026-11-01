using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace HtmlSerializer
{
    internal class HtmlHelper
    {
        private static readonly HtmlHelper instance = new HtmlHelper();
        public string[] HtmlTags { get; set; }
        public string[] SelfClosingTags { get; set; }
        public static HtmlHelper Instance
        {
            get { return instance; }
        }
        private HtmlHelper()
        {
            string tagsJson = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Data", "HtmlTags.json"));
            string selfClosingJson = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Data", "HtmlVoidTags.json"));

            HtmlTags = JsonSerializer.Deserialize<string[]>(tagsJson) ?? Array.Empty<string>();
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingJson) ?? Array.Empty<string>();
        }
    }
}
