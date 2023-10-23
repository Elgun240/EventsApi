using System.ComponentModel.DataAnnotations;

namespace Practice_4.ViewModels
{
    public class ProfileVM
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        [Required,DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(NewPassword))]
        public string CheckPassword { get; set; }
        public string[] ErrorMessages { get; set; }



    }
}
