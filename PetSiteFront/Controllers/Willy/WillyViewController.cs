using Microsoft.AspNetCore.Mvc;

namespace PetSiteFront.Controllers.Willy
{
    public class WillyViewController : Controller
    {
        public IActionResult ProductIndex()
        {
            return View();
        }
        public IActionResult ProductDetail(int id)
        {
            return View();
        }
    }
}
