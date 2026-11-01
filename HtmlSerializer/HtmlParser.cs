using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace HtmlSerializer
{
    public class HtmlParser
    {
        public static List<string> Parse(string html)
        {
            var matches = Regex.Matches(html, @"<[^>]+>|[^<]+");

            return matches
                .Select(m => m.Value.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }

        public static HtmlElement BuildTree(List<string> htmlParts)
        {
            HtmlElement root = new HtmlElement();
            HtmlElement current = root;

            foreach (string part in htmlParts)
            {
                if (part.StartsWith("<") && part.EndsWith(">"))
                {
                    string tagContent = part.Substring(1, part.Length - 2).Trim();

                    if (tagContent.StartsWith("/"))
                    {
                        string closingTag = tagContent.Substring(1).Trim();

                        if (current.Parent != null &&
                            current.Name == closingTag)
                        {
                            current = current.Parent;
                        }

                        continue;
                    }

                    string tagName = Regex.Match(tagContent, @"^[^\s/>]+").Value;

                    if (!HtmlHelper.Instance.HtmlTags.Contains(tagName))
                        continue;

                    HtmlElement newElement = new HtmlElement();
                    newElement.Name = tagName;
                    newElement.Parent = current;

                    string attributesContent = tagContent.Substring(tagName.Length);

                    var attributes = Regex.Matches(
                        attributesContent,
                        @"([^\s=]+)(?:\s*=\s*(?:""([^""]*)""|'([^']*)'|([^\s""'=<>`]+)))?"
                    );

                    foreach (Match attribute in attributes)
                    {
                        string attributeName = attribute.Groups[1].Value;
                        string attributeValue =
                            attribute.Groups[2].Success ? attribute.Groups[2].Value :
                            attribute.Groups[3].Success ? attribute.Groups[3].Value :
                            attribute.Groups[4].Success ? attribute.Groups[4].Value : "";

                        newElement.Attributes.Add(
                            attributeName + (attribute.Groups[2].Success ||
                                             attribute.Groups[3].Success ||
                                             attribute.Groups[4].Success
                                ? "=" + attributeValue
                                : "")
                        );

                        if (attributeName == "id")
                            newElement.Id = attributeValue;

                        if (attributeName == "class")
                        {
                            newElement.Classes.AddRange(
                                attributeValue.Split(
                                    ' ',
                                    StringSplitOptions.RemoveEmptyEntries
                                )
                            );
                        }
                    }

                    current.Children.Add(newElement);

                    bool isSelfClosing =
                        tagContent.EndsWith("/") ||
                        HtmlHelper.Instance.SelfClosingTags.Contains(tagName);

                    if (!isSelfClosing)
                    {
                        current = newElement;
                    }
                }
                else
                {
                    if (current.InnerHtml == null)
                        current.InnerHtml = part;
                    else
                        current.InnerHtml += part;
                }
            }

            return root;
        }

    }
}
