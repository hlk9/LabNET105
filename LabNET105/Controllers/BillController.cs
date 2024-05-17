using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using DAL.ViewModel;


namespace LabNET105.Controllers
{
    public class BillController : Controller
    {
        LabDbContext _context;

        public BillController()
        {
            _context = new LabDbContext();
        }
        // GET: BillController


        public IActionResult ListBill()
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("uid"));

            ICollection<Bill> model = _context.Bills.Where(x => x.AccountId == userId).ToList();

            return View(model);


        }
        // GET: BillController/Details/5
        public ActionResult Details(int id)
        {

            var detailbills = _context.BillDetails.ToList();
            return View(detailbills);
        }

        //Cái này là để xem những billdetail nào có trong bill









        // GET: BillController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BillController/Edit/5
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

        // GET: BillController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BillController/Delete/5
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
