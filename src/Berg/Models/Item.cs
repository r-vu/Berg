namespace Berg.Models {
    public class Item {

        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Item() { }

        public Item(string name, decimal price) {
            Name = name;
            Price = price;
        }

        public override bool Equals(object obj) {
            Item other = obj as Item;

            return other != null &&
                ID.Equals(other.ID) &&
                Name.Equals(other.Name) &&
                Price.Equals(other.Price);
        }

        public override int GetHashCode() {
            return unchecked(
                (ID.GetHashCode()
                + Name.GetHashCode()
                + Price.GetHashCode())
                .GetHashCode()
            );
        }

        public override string ToString() {
            return string.Format("ID: {0} Name: {1} Price: {2}", ID, Name, Price);
        }
    }
}
