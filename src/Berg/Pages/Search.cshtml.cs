using Berg.Data;
using Berg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Berg.Pages {
    public class SearchModel : PageModel {
        private readonly BergContext Context;

        public IList<Item> ItemList { get; set; }

        [FromQuery(Name = "query")]
        public string SearchString { get; set; }

        [FromQuery(Name = "size")]
        public int PageSize { get; set; } = 9;

        [FromQuery(Name = "pg")]
        public int CurrentPage { get; set; } = 1;
        public int MaxPage { get; private set; }

        private static readonly string QUERY_SEARCHSTRING = "query";
        private static readonly string QUERY_PAGESIZE = "size";
        private static readonly string QUERY_CURRENTPAGE = "pg";

        public SearchModel(BergContext context) {
            Context = context;
        }

        public async Task OnGetAsync() {
            IQueryable<Item> query = from item in Context.Item select item;
            query = query.Where(item => item.Name.Contains(SearchString));
            MaxPage = ((query.Count() - 1) / PageSize) + 1;
            ItemList = await query.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToListAsync();
        }

        public string PageNavigationLink(bool next) {
            RouteValueDictionary routeValueDictionary = Request.RouteValues;
            IQueryCollection currentQuery = HttpContext.Request.Query;

            foreach (string key in currentQuery.Keys) {
                routeValueDictionary.Add(key, currentQuery[key]);
            }

            int pageNum = next ? CurrentPage + 1 : CurrentPage - 1;

            if (!routeValueDictionary.TryAdd(QUERY_CURRENTPAGE, pageNum)) {
                routeValueDictionary[QUERY_CURRENTPAGE] = pageNum;
            }

            return Url.RouteUrl(routeValueDictionary);
        }
    }
}
