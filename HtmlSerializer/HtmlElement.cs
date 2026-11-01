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
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();

            foreach (HtmlElement child in Children)
            {
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();

                yield return current;

                foreach (HtmlElement child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = Parent;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
    }
}
