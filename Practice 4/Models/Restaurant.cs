using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace Practice_4.Models
{
    public class Restaurant
    {

        public int Id { get; set; }

        [Required]

        public string Name { get; set; }

        [Required]

        public string Description { get; set; }

        [Required]

        
        public string OpeningTime { get; set; }

        [Required]

       
        public string ClosingTime { get; set; }

        public string OenDays { get; set; }
        public string Adress { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string Contact { get; set; }
        public string URL { get; set; }
        public string Email { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; } = 1;

    }
    


}
