using Berg.Data;
using Berg.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Berg.Controllers.Api {
    [Route("/api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase {

        private readonly BergContext _context;
        private readonly UserManager<BergUser> _userManager;

        public ReviewController(BergContext context, UserManager<BergUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET api/<ReviewController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemReview>> Get(int id) {
            ItemReview review = await _context.ItemReview.FindAsync(id);
            if (review == null) {
                return NotFound();
            }
            return Ok(review);
        }

        // POST api/<ReviewController>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ItemReview>> Post(ItemReview review) {
            review.OwnerId = _userManager.GetUserId(HttpContext.User);
            _context.ItemReview.Attach(review);
            _context.Entry(review.Item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Created(nameof(Get) + "/" + review.ReviewId, review);
            //return CreatedAtAction(nameof(Get), new { id = review.ReviewId }, review);
        }

        // PUT api/<ReviewController>/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Put(int id, ItemReview review) {
            ItemReview savedReview = await _context.ItemReview.FindAsync(id);
            string userId = _userManager.GetUserId(HttpContext.User);
            if (review != null) {
                if (!review.OwnerId.Equals(userId)) {
                    return Forbid();
                }

                savedReview.Rating = review.Rating;
                savedReview.Body = review.Body;
                _context.Entry(savedReview).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        // DELETE api/<ReviewController>/5
        // never returns 404, use a GET to find a resource, not DELETE
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Delete(int id) {
            ItemReview review = await _context.ItemReview.FindAsync(id);
            string userId = _userManager.GetUserId(HttpContext.User);
            if (review != null) {
                if (!review.OwnerId.Equals(userId)) {
                    return Forbid();
                }

                _context.ItemReview.Remove(review);
                Item item = await _context.Item.FindAsync(review.ItemId);
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
