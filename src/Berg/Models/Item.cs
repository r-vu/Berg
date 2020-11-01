using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Berg.Models {
    public class Item {

        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public Uri Image { get; set; }
        public IList<ItemReview> Reviews { get; set; } = new List<ItemReview>();

        private readonly static Uri NO_IMAGE = new Uri("https://upload.wikimedia.org/wikipedia/commons/a/ac/No_image_available.svg");

        public Item() { }

        public Item(string name, decimal price) {
            Name = name;
            Price = price;
            Image = NO_IMAGE;
        }

        public Item(string name, decimal price, Uri image) {
            Name = name;
            Price = price;
            Image = image;
        }

        public override bool Equals(object obj) {
            Item other = obj as Item;

            return other != null &&
                ID.Equals(other.ID) &&
                Name.Equals(other.Name) &&
                Price.Equals(other.Price) &&
                Image.Equals(other.Image);
        }

        public override int GetHashCode() {
            return unchecked(
                (ID.GetHashCode()
                + Name.GetHashCode()
                + Price.GetHashCode()
                + Image.GetHashCode())
                .GetHashCode()
            );
        }

        public override string ToString() {
            return string.Format("ID: {0} Name: {1} Price: {2} Image: {3}", ID, Name, Price, Image);
        }
    }
}
