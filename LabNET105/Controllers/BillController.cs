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

            ICollection<Bill> model = _context.Bills.Where(x => x.AccountId == userId && x.Status != 100).ToList();

            return View(model);


        }

        public IActionResult HuyBill(int id)
        {
            var lstBillDetail = _context.BillDetails.ToList();
            for(int i = 0; i < lstBillDetail.Count; i++)
            {
                if (_context.Products.FirstOrDefault(x => x.Id == lstBillDetail[i].ProductId) != null)
                {
                    var obj = _context.Products.FirstOrDefault(x => x.Id == lstBillDetail[i].ProductId);
                    obj.Quantity += lstBillDetail[i].Quantity;
                    _context.Products.Update(obj);
                    _context.SaveChanges();
                }    
            }
            var statusbill = _context.Bills.Find(id);
            statusbill.Status = 100;
            _context.Bills.Update(statusbill);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
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
