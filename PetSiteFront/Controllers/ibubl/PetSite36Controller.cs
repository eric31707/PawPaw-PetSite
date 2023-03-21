using Microsoft.AspNetCore.Mvc;

namespace PetSiteFront.Controllers.ibubl
{
	public class PetSite36Controller : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
        public IActionResult AdoptDetail(int id)
        {
            return View();
        }
        public IActionResult Allshelter()
		{//全國領養資訊
			return View();
		}
		public IActionResult AdoptionPage()
		{//取得所有領養資訊
			return View();
		}

		public IActionResult PostAdoption()
		{//
			return View();
		}
        public IActionResult EditAdoption()
        {//
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostAdoption(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public IActionResult MemberCoupon()
		{//前台會員折價券管理
			return View();
		}
		public IActionResult MbAdoptionList()
		{
			return View();
		}
        public IActionResult Vueshelter()
        {//
            return View();
        }
		public IActionResult MapsAdoptions()
		{
			return View();
		}

    }
}
