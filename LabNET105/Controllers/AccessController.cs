using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabNET105.Controllers
{
    public class AccessController : Controller
    {
        LabDbContext _context = new LabDbContext();
        private readonly IHttpContextAccessor _contextAccessor;

        public AccessController(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var acc = _context.Accounts.FirstOrDefault(x => x.Username == username);
            if (acc == null)
            {
                ViewBag.Error = "Không tồn tại";
                return View();
            }
            else
            {
                if (acc.Password == password)
                {
                    var account = _context.Accounts.FirstOrDefault(x => x.Username == username);
                    _contextAccessor.HttpContext.Session.SetString("username", account.Username);
                    _contextAccessor.HttpContext.Session.SetString("phone", acc.PhoneNumber);
                    _contextAccessor.HttpContext.Session.SetString("uid", acc.Id.ToString());
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

        [HttpPost]
        public IActionResult Register(string userName, string phone, string pass, string rePass, string addr)
        {
            if (_context.Accounts.FirstOrDefault(x => x.Username == userName) != null || _context.Accounts.FirstOrDefault(x => x.PhoneNumber == phone) != null)
            {
                ViewBag.Error = "Tài khoản đã tồn tại";
            }
            else
            {
                if (pass != rePass)
                {
                    ViewBag.Error = "Mật khẩu không khớp";
                }
                else
                {
                    var acc = new Account();
                    acc.Username = userName;
                    acc.Password = pass;
                    acc.PhoneNumber = phone;
                    acc.Address = addr;
                    _context.Accounts.Add(acc);
                    _context.SaveChanges();


                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}
