using Berg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;


namespace Berg.Pages.Items {
    public class CreateModel : PageModel {
        private readonly Berg.Data.BergContext _context;

        [BindProperty]
        public Item Item { get; set; }

        [BindProperty(Name = "urlInputField")]
        public string ImageUrl { get; set; }

        [BindProperty(Name = "imageUploadInput")]
        public ItemImage ImageFile { get; set; }

        public CreateModel(Berg.Data.BergContext context) {
            _context = context;

        }

        public IActionResult OnGet() {
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            //ImageFile = WebImage.GetImageFromRequest();
            if (ImageUrl != null) {
                Item.Image = new Uri(ImageUrl);
            } else if (ImageFile != null) {
                // Image API?

                Item.Name = Item.Name + "_UsedUpload";
            }

            _context.Item.Add(Item);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostUploadAsync() {
            return Page();
        }
    }

    public class ItemImage {

        [Required]
        public IFormFile FormFile { get; set; }
    }
}
