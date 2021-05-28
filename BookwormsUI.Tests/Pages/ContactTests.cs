using Xunit;
using Bunit;
using BookwormsUI.Pages;

namespace BookwormsUI.Tests
{
    public class ContactTests : TestContext
    {
        [Fact]
        public void ContactPageShouldShowExpectedHeaderText()
        {
            var cut = RenderComponent<Contact>();

            cut.Find("h4").MarkupMatches("<h4 class=\"mb-5\">Contact</h4>");
        }
    }
}