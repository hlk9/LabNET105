using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult ListBills()
        {
            var bills = _context.Bills.ToList() ;
            return View(bills);
        }

        // GET: BillController/Details/5
        public ActionResult Details(int id)
        {
            var detailbills= _context.BillDetails.ToList() ;
            return View(detailbills);
        }

        // GET: BillController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BillController/Create
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
