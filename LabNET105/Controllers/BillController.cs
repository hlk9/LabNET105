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


        public IActionResult  ListBill (int id, string name,double totalprice)
        {
            
            var model = from a in _context.Bills
                        join b in _context.BillDetails

                        on a.Id equals b.BillId
                        join c in _context.Accounts
                        on a.AccountId equals c.Id

                        where a.Id == id

                        select new BillViewModel
                        {
                            Id = a.Id,
                            Name = c.Username,
                            TotalPrice = b.Price * b.Quantity
                        };
            model.OrderByDescending(x => x.TotalPrice);
            

            return View(model.ToList());

                       
        }
        // GET: BillController/Details/5
        public ActionResult Details(int id)
        {
          
            var detailbills= _context.BillDetails.ToList() ;
            return View(detailbills);
        }

        //Cái này là để xem những billdetail nào có trong bill


        public IActionResult ThanhToan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ThanhToan(BillDetail billdetails, int id)
        {
            try
            {

                _context.BillDetails.Add(billdetails);
                CartItem cartitem = _context.CartItems.Where(x => x.ProductId == id).FirstOrDefault();
                if (cartitem.Quantity > 1)
                {
                    --cartitem.Quantity;
                }
                else
                {
                    _context.CartItems.Remove(cartitem);
                }
                return RedirectToAction("Index", "Home");


            }
            catch
            {
                return BadRequest();
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
