using DAL;
using Microsoft.AspNetCore.Mvc;

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
            var check = HttpContext.Session.GetString("username");
            if (String.IsNullOrEmpty(check))
            {
                return RedirectToAction("Index", "Access"); // chuyển hướng về trang login
            }
            else
            {
                if (int.TryParse(check, out int usernameInt))
                {
                    var allCartItem = context.CartItems.Where(p => p.ProductId == usernameInt).ToList();
                    return View(allCartItem);
                }
                else
                {
                    
                    return RedirectToAction("Index", "Home");
                }
            }
        }

    }
}
