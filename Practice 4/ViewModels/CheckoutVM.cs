using Practice_4.Models;

namespace Practice_4.ViewModels
{
    public class CheckoutVM
    {
        public Order Order { get; set; }
        public List<Order> Orders { get; set; }
        public string Adress { get; set; }
    }
}
