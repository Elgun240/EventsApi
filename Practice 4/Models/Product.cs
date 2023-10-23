using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_4.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Restaurant Restaurant { get; set; }
        public int RestaurantID { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public int SaledCount { get; set; }
    }
}
