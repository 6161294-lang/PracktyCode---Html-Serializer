using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlSerializer
{
    public class HtmlElement
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? InnerHtml { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public HtmlElement? Parent { get; set; }
    }
}
