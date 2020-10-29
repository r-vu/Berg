using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berg.Data;
using Berg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Berg.Pages
{
    public class SearchModel : PageModel
    {
        private readonly BergContext Context;

        public IList<Item> ItemList { get; set; }

        [FromQuery(Name = "keywords")]
        public string SearchString { get; set; }

        [FromQuery(Name = "size")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "page")]
        public int PageNumber { get; set; } = 1;

        public SearchModel(BergContext context) {
            Context = context;
        }

        public async Task OnGetAsync() {
            IQueryable<Item> query = from item in Context.Item select item;
            ItemList = await query.Where(item => item.Name.Contains(SearchString))
                .Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
        }
    }
}
