namespace Practice_4.Models
{
    public class PaidOrder
    {
        public int Id { get; set; }
    
        public string Name { get; set; }
        
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Status Status { get; set; }
        public DateTime OrderDate { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public string Adress { get; set; }
        public double Total { get; set; }
        public int RestaurantId{ get; set; }
        public Restaurant Restaurant { get; set; }

    }
    public enum Status
    {
        Dispatch=1,
        OnWay,
        Cancelled,
        Delivered
    }
}
