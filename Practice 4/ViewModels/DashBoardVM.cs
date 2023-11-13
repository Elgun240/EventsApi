using Practice_4.Models;

namespace Practice_4.ViewModels
{
    public class DashBoardVM
    {
        public int AppUsers { get; set; }
        public int Products { get; set; }
        public int   Restaurants { get; set; }
        public int Categories { get; set; }
        public int PaidOrders { get; set; }
        public int PendingOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int RejectedOrders { get; set; }
    }
}
