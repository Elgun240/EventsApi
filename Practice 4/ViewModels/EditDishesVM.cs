using Practice_4.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_4.ViewModels
{
    public class EditDishesVM
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30,MinimumLength =3)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1,double.MaxValue,ErrorMessage ="Please enter a number!")]
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Restaurant Restaurant { get; set; }
        public int RestaurantID { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public List<Restaurant> Restaurants { get; set; }
        public Product Product { get; set; }

        
        
       
    }
}
