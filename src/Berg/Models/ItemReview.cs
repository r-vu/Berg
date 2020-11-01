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

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public BergUser Owner { get; set; }
        
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        [Required]
        public double Rating { get; set; }
        public string Body { get; set; }

        public ItemReview() { }

        public ItemReview(double rating) {
            Rating = rating;
        }
        
    }
}
