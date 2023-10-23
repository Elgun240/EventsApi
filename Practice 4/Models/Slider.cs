using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_4.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
