using System;
using System.Collections.Generic;

namespace HtmlSerializer
{
    public class Selector
    {
        public string TagName { get; set; }

        public string Id { get; set; }

        public List<string> Classes { get; set; } = new List<string>();

        public Selector Parent { get; set; }

        public Selector Child { get; set; }

        public static Selector ParseSelector(string selectorString)
        {
            if (string.IsNullOrWhiteSpace(selectorString))
                return null;

            string[] parts = selectorString.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries
            );

            Selector first = null;
            Selector current = null;

            foreach (string part in parts)
            {
                Selector newSelector = ParseSingleSelector(part);

                if (first == null)
                {
                    first = newSelector;
                }
                else
                {
                    current.Child = newSelector;
                    newSelector.Parent = current;
                }

                current = newSelector;
            }

            return current;
        }

        private static Selector ParseSingleSelector(string part)
        {
            Selector selector = new Selector();

            int index = 0;

            while (index < part.Length &&
                   part[index] != '#' &&
                   part[index] != '.')
            {
                index++;
            }

            string tagName = part.Substring(0, index);

            if (!string.IsNullOrEmpty(tagName) && tagName != "*")
            {
                selector.TagName = tagName;
            }

            while (index < part.Length)
            {
                char symbol = part[index];
                index++;

                int start = index;

                while (index < part.Length &&
                       part[index] != '#' &&
                       part[index] != '.')
                {
                    index++;
                }

                string value = part.Substring(start, index - start);

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Invalid selector");

                if (symbol == '#')
                {
                    selector.Id = value;
                }
                else if (symbol == '.')
                {
                    selector.Classes.Add(value);
                }
            }

            return selector;
        }
    }
}