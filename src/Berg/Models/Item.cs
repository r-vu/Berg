using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Berg.Models {
    public class Item {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public Uri Image { get; set; }
        public IList<ItemReview> Reviews { get; set; } = new List<ItemReview>();
        public double AverageRating { get; private set; } = -1;

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

        public void CalculateAverageRating() {
            if (Reviews.Count == 0) {
                AverageRating = -1;
            }

            double sum = 0.0;
            foreach (ItemReview review in Reviews) {
                sum += review.Rating;
            }

            AverageRating = sum / Reviews.Count;
        }

        public override bool Equals(object obj) {
            Item other = obj as Item;

            return other != null &&
                Id.Equals(other.Id) &&
                Name.Equals(other.Name) &&
                Price.Equals(other.Price) &&
                Image.Equals(other.Image) &&
                AverageRating.Equals(other.AverageRating);
        }

        public override int GetHashCode() {
            return unchecked(
                (Id.GetHashCode()
                + Name.GetHashCode()
                + Price.GetHashCode()
                + Image.GetHashCode()
                + AverageRating.GetHashCode())
                .GetHashCode()
            );
        }

        public override string ToString() {
            return string.Format("Id: {0} Name: {1} Price: {2} Image: {3} Rating: {4}", Id, Name, Price, Image, AverageRating);
        }
    }
}
