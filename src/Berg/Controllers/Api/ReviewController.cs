using Berg.Data;
using Berg.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Berg.Controllers.Api {
    [Route("/api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase {

        private readonly BergContext _context;

        public ReviewController(BergContext context) {
            _context = context;
        }

        // GET api/<ReviewController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<ActionResult<ItemReview>> Get(int id) {
            ItemReview review = await _context.ItemReview.FindAsync(id);
            if (review == null) {
                return NotFound();
            }
            return Ok(review);
        }

        // POST api/<ReviewController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ItemReview>> Post(ItemReview review) {
            _context.ItemReview.Attach(review);
            _context.Entry(review.Item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Created(nameof(Get) + "/" + review.ReviewId, review);
            //return CreatedAtAction(nameof(Get), new { id = review.ReviewId }, review);
        }

        // PUT api/<ReviewController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, ItemReview review) {
            return NoContent();
        }

        // DELETE api/<ReviewController>/5
        // never returns 404, use a GET to find a resource, not DELETE
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id) {
            ItemReview review = await _context.ItemReview.FindAsync(id);
            if (review != null) {
                _context.ItemReview.Remove(review);
                Item item = await _context.Item.FindAsync(review.ItemId);
                _context.Entry(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
