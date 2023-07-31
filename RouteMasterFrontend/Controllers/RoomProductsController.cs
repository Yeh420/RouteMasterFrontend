using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Controllers
{
    public class RoomProductsController : Controller
    {
        public RouteMasterContext db { get; set; }
        public RoomProductsController(RouteMasterContext context)
        {
            db = context;
        }
        // GET: RoomProductsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RoomProductsController
        public ActionResult ShowCalendar()
        {
            var products = db.RoomProducts.Where(rp=>rp.RoomId ==1);
            return View(products);
        }

        // GET: RoomProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RoomProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: RoomProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoomProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: RoomProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoomProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
