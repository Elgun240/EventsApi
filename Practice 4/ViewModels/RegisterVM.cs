using Practice_4.Models;
using System.ComponentModel.DataAnnotations;

namespace Practice_4.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Firstname { get; set; }
         [Required]
        public string Lastname { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Restaurant { get; set; }
        [Required]
        public List<Restaurant> Restaurants { get; set; }


    }
}
