using Berg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Berg.Pages.Items {
    public class IndexModel : PageModel {
        private readonly Berg.Data.BergContext _context;

        public IndexModel(Berg.Data.BergContext context) {
            _context = context;
        }

        public IList<Item> Item { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync() {
            //Item = await _context.Item.ToListAsync();

            IQueryable<Item> items = from item in _context.Item select item;

            if (!string.IsNullOrEmpty(SearchString)) {
                items = items.Where(item => item.Name.Contains(SearchString));
            }

            Item = await items.ToListAsync();
        }
    }
}
