using Microsoft.AspNetCore.Mvc;
using Practice_4.Models;

namespace Practice_4.ViewModels
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Slider> Sliders { get; set; }
       
    }
}
