using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WordPressPCL.Models;

namespace ContentViewerApp.Services
{
    public class ContentProvider
    {
        private const string cmsUrl = "https://wls-blueprint-777-a.azurewebsites.net/wp-json/";

        public async Task<string> GetSection(string pageSlug, string sectionSlug)
        {
            string pageContent = await GetPage(pageSlug);

            string startTag = $"<h2 id=\"{sectionSlug}\"";
            string endTag = $"<h2 id=\"";

            int startPos = pageContent.IndexOf(startTag, StringComparison.OrdinalIgnoreCase);

            if (startPos<0)
            {
                throw new KeyNotFoundException($"No section found with slug {pageSlug} > {sectionSlug}");
            }

            int endPos = pageContent.IndexOf(endTag, startPos+20, StringComparison.OrdinalIgnoreCase);

            string sectionContent;

            if (endPos > 1)
            {
                int sectionLen = endPos - startPos;
                sectionContent = pageContent.Substring(startPos, sectionLen);
            }
            else
            {
                sectionContent = pageContent.Substring(startPos);
            }

            int nextPos = sectionContent.IndexOf("</h2>") + 5;
            string header = sectionContent.Substring(0, nextPos);
            sectionContent = sectionContent.Substring(nextPos + 1);

            sectionContent = header + sectionContent.Replace("<h2 ", "<h4 ")
                                                    .Replace("</h2>", "</h4>")
                                                    .Replace("class=\"has-normal-font-size\"", "")
                                                    .Replace("class=\"has-medium-font-size\"", "");

            return sectionContent;
        }

        private string CleanupSection(string sectionContent)
        {
            string endingTag = "<h2 id=\"";
            if (sectionContent.EndsWith(endingTag))
            {
                int len = sectionContent.Length - endingTag.Length;
                sectionContent = sectionContent.Substring(0, len);
            }
            return sectionContent;
        }

        public async Task<string> GetPage(string pageSlug)
        {
            var client = new WordPressPCL.WordPressClient(cmsUrl);

            IEnumerable<Page> pages = await client.Pages.GetAll();

            Page foundPage = pages.FirstOrDefault(p => p.Slug.ToLowerInvariant() == pageSlug.ToLowerInvariant());

            if (foundPage == null)
            {
                throw new KeyNotFoundException($"No page found with slug: {pageSlug}");
            }

            return foundPage.Content.Rendered;
        }

    }
}
