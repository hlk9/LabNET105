using DAL;
using Microsoft.AspNetCore.Mvc;

namespace LabNET105.Controllers
{
    public class AccessController : Controller
    {
        LabDbContext _context= new LabDbContext();
       
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var acc = _context.Accounts.FirstOrDefault(x => x.Username == username);
            if(acc==null)
            {
                ViewBag.Error = "Không tồn tại";
                return View();
            }  
            else
            {
                   if(acc.Password==password)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ViewBag.Error = "Sai TK MK";
                    return View();
                }
            }

           
          
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
