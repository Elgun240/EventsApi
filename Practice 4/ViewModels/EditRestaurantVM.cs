using Practice_4.Models;

namespace Practice_4.ViewModels
{
    public class EditRestaurantVM
    {
        public Restaurant Restaurant { get; set; }
        public List<Category> Categories { get; set; }
        public int CategoryId { get; set; }
    }
}
