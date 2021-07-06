using ContentViewerApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Threading.Tasks;

namespace ContentViewerApp.Tests
{
    [TestClass]
    public class ContentProviderTests
    {
        [TestMethod]
        public async Task GetPage()
        {
            const string slug = "safety-domains";

            var provider = new ContentProvider();

            string content = await provider.GetPage(slug);

            content.ShouldNotBeEmpty();
        }

        [TestMethod]
        public async Task GetMiddleSection()
        {
            const string pageSlug = "safety-domains";
            const string sectionSlug = "injuries";

            var provider = new ContentProvider();

            string content = await provider.GetSection(pageSlug, sectionSlug);

            content.ShouldNotBeEmpty();
        }

        [TestMethod]
        public async Task GetFinalSection()
        {
            const string pageSlug = "safety-domains";
            const string sectionSlug = "staff-youth-relationships";

            var provider = new ContentProvider();

            string content = await provider.GetSection(pageSlug, sectionSlug);

            content.ShouldNotBeEmpty();
        }

    }
}
