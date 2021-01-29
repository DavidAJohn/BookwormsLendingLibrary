using System.Collections.Generic;
using System.Threading.Tasks;
using BookwormsUI.Models;
using Microsoft.AspNetCore.Components;

namespace BookwormsUI.Components.Paging
{
    public partial class Paging
    {
        [Parameter]
        public PagingMetadata metadata { get; set; }

        [Parameter]
        public int Spread { get; set; }
        
        [Parameter]
        public EventCallback<int> SelectedPage { get; set; }
            
        private List<PagingLink> _links;
        
        protected override void OnParametersSet()
        {
            CreatePaginationLinks();
        }
        private void CreatePaginationLinks()
        {
            _links = new List<PagingLink>();

            _links.Add(new PagingLink(metadata.PageIndex - 1, metadata.HasPreviousPage, "Previous"));

            for (int i = 1; i <= metadata.TotalPages; i++)
            {
                if (i >= metadata.PageIndex - Spread && i <= metadata.PageIndex + Spread)
                {
                    _links.Add(new PagingLink(i, true, i.ToString()) { Active = metadata.PageIndex == i });
                }
            }

            _links.Add(new PagingLink(metadata.PageIndex + 1, metadata.HasNextPage, "Next"));
        }
        private async Task OnSelectedPage(PagingLink link)
        {
            if (link.Page == metadata.PageIndex || !link.Enabled) return;

            metadata.PageIndex = link.Page;
            await SelectedPage.InvokeAsync(link.Page);
        }
    }
}