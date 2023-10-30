using Practice_4.Models;
using System.Security.Principal;

namespace Practice_4.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        
        public List<Order> Orders { get; set; }
        public List<PaidOrder> PaidOrders { get; set; }
        public List<PaidOrder> DeliveredOrders { get; set; }
        public string Adress { get; set; }
        public string AppUserId { get; set; }
        public double SubTotal { get; set; }
        public string GrandTotal { get; set; }


        //public string DishName { get; set; }
        //public double Price { get; set; }
        //public double Total { get; set; }
        //public int Quatntity { get; set; }

        //public string Image { get; set; }

        //public int RestaurantId { get; set; }
        //public Order Order { get; set; }

    }
}
