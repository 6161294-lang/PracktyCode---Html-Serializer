using System;
using System.Collections.Generic;
using System.Linq;

namespace HtmlSerializer
{
    public static class HtmlQueryExtensions
    {
        public static HashSet<HtmlElement> FindElements(
            this HtmlElement root,
            Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();

            if (root == null || selector == null)
                return result;

            foreach (HtmlElement element in root.Descendants())
            {
                if (MatchesSelector(element, selector))
                {
                    result.Add(element);
                }
            }

            return result;
        }

        private static bool MatchesSelector(
            HtmlElement element,
            Selector selector)
        {
            if (!MatchesSingleSelector(element, selector))
                return false;

            if (selector.Parent == null)
                return true;

            foreach (HtmlElement ancestor in element.Ancestors())
            {
                if (MatchesSelector(ancestor, selector.Parent))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool MatchesSingleSelector(
            HtmlElement element,
            Selector selector)
        {
            if (selector.TagName != null &&
                !string.Equals(
                    element.Name,
                    selector.TagName,
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (selector.Id != null &&
                element.Id != selector.Id)
            {
                return false;
            }

            foreach (string className in selector.Classes)
            {
                if (!element.Classes.Contains(className))
                {
                    return false;
                }
            }

            return true;
        }
    }
}