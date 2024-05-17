using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabNET105.Controllers
{
    public class CartController : Controller
    {
        LabDbContext context;
        public CartController()
        {
            context = new LabDbContext();
        }
        public IActionResult Index() 
        {
            // Kiểm tra dữ liệu đăng nhập
            var check = HttpContext.Session.GetString("uid");
            var getCartId = context.Carts.FirstOrDefault(x => x.AccountId == Guid.Parse(check)).Id;
            if (check == null)
            {
                return RedirectToAction("Index", "Access"); // chuyển hướng về trang login
            }
            else
            {
                var allCartItem = context.CartItems.Where(p => p.CartId == getCartId).ToList();
                return View(allCartItem);
                
            }
        }

    }
}
