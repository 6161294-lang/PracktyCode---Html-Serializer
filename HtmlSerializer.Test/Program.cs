using System.Xml;
using HtmlSerializer;

var parts = HtmlParser.Parse(
    "<html><body><div id=\"main\"><p>Hello</p></div></body></html>"
);

HtmlElement root = HtmlParser.BuildTree(parts);

Console.WriteLine(root.Children[0].Name);