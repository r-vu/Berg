using Berg.Data;
using Berg.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Berg.Pages {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private readonly BergContext _context;

        public IList<Item> ItemList { get; set; }

        public IndexModel(ILogger<IndexModel> logger, BergContext context) {
            _logger = logger;
            _context = context;
        }

        public async Task OnGetAsync() {
            IQueryable<Item> items = (from item in _context.Item select item);
            int limit = await items.CountAsync();
            limit = limit > 6 ? 6 : limit;
            ItemList = await items.Skip(0).Take(limit).ToListAsync();
        }
    }
}
