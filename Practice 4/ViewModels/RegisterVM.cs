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

    }
}
