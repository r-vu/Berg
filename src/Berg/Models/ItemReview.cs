using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Berg.Models {
    public class ItemReview {

        [Key]
        public int ReviewId { get; set; }

        // Reminder that Entity Framework uses lazy loading by default
        // This means when retrieving an ItemReview from the context,
        // the fields Owner and Item will be null, and only the Id fields
        // will be defined. To force load, use .Include()

        [ForeignKey("OwnerId")]
        public BergUser Owner { get; set; }
        public string OwnerId { get; set; }
        
        [ForeignKey("ItemId")]
        public Item Item { get; set; }
        public int ItemId { get; set; }

        [Required]
        public double Rating { get; set; }
        public string Body { get; set; }

        public ItemReview() { }

        public ItemReview(double rating) {
            Rating = rating;
        }

        public ItemReview(string ownerId, int itemId, double rating) {
            OwnerId = ownerId;
            ItemId = itemId;
            Rating = rating;
        }

        public override bool Equals(object obj) {
            ItemReview other = obj as ItemReview;

            return other != null &&
                ReviewId.Equals(other.ReviewId) &&
                OwnerId.Equals(other.OwnerId) &&
                ItemId.Equals(other.ItemId) &&
                Rating.Equals(other.Rating) &&
                Body.Equals(other.Body);
        }

        public override int GetHashCode() {
            return unchecked(
                ReviewId +
                OwnerId.GetHashCode() +
                ItemId +
                Rating.GetHashCode() +
                Body.GetHashCode()
            );
        }

        public override string ToString() {
            return string.Format(
                "ReviewId: {0} ItemId: {1} OwnerId: {2} Rating: {3} Body: {4}",
                ReviewId, ItemId, OwnerId, Rating, Body
            );
        }

    }
}
