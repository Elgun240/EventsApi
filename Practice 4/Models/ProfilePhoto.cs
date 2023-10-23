namespace Practice_4.Models
{
    public class ProfilePhoto

    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ImagePath { get; set; }
        public AppUser User { get; set; }
    }
}
