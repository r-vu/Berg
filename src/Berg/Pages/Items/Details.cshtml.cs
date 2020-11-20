using Berg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Berg.Pages.Items {
    public class DetailsModel : PageModel {
        private readonly Berg.Data.BergContext _context;

        public DetailsModel(Berg.Data.BergContext context) {
            _context = context;
        }

        public Item Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Item = await _context.Item.FirstOrDefaultAsync(m => m.Id == id);

            if (Item == null) {
                return NotFound();
            }
            return Page();
        }
    }
}
