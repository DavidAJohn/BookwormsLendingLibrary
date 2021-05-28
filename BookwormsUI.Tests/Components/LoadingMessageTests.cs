using Xunit;
using Bunit;
using BookwormsUI.Components;

namespace BookwormsUI.Tests.Components
{
    public class LoadingMessageTests : TestContext
    {
        [Fact]
        public void LoadingMessage_ShouldDisplayExpectedText()
        {
            var cut = RenderComponent<LoadingMessage>(parameters => parameters
                .Add(p => p.Message, "Test Message")
            );

            cut.Find("h5").MarkupMatches("<h5>Test Message</h5>");
        }
    }
}