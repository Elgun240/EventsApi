namespace Practice_4.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string DishName  { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public int Quatntity { get; set; }
        public double SubTotal { get; set; }
        public string Image { get; set; }
        public Restaurant Restaurant{ get; set; }
        public int RestaurantId { get; set; }
        public AppUser? AppUser { get; set; }
        public string AppUserId { get; set; }


    }
}
