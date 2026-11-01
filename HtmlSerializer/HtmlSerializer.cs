using System.Collections.Generic;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class HtmlSerializer
    {
        private readonly HtmlLoader loader = new HtmlLoader();

        public async Task<HtmlElement> Serialize(string url)
        {
            string html = await loader.Load(url);

            List<string> htmlParts = HtmlParser.Parse(html);

            HtmlElement root = HtmlParser.BuildTree(htmlParts);

            return root;
        }
    }
}