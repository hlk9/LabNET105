using DAL;
using DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabNET105.Controllers
{
    public class CartController : Controller
    {
        LabDbContext context;

        public List<CartItemViewModel> _lstCart;
        public CartController()
        {

            context = new LabDbContext();
        }

        public IActionResult Index(int id)
        {
            // Kiểm tra dữ liệu đăng nhập
            var check = HttpContext.Session.GetString("uid");
            if (check == null)
            {
                return RedirectToAction("Index", "Access"); // chuyển hướng về trang login
            }

            var accountId = Guid.Parse(check);
            var getCartId = context.Carts.FirstOrDefault(x => x.AccountId == accountId)?.Id;
            if (getCartId == null)
            {
                // Xử lý khi không tìm thấy giỏ hàng
                return RedirectToAction("Index", "Home");
            }

            // Lấy danh sách các CartItem và thông tin sản phẩm liên quan
            var allCartItem = context.CartItems
                                    .Where(p => p.CartId == getCartId)
                                    .Select(p => new
                                    {
                                        ProductName = p.Product.Name,
                                        Price = p.Product.Price,
                                        Quantity = p.Quantity,
                                        TotalPrice = p.Quantity * p.Product.Price
                                    })
                                    .ToList();

            return View(allCartItem);
        }




    }
}

