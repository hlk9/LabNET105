using DAL;
using DAL.Models;
using DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabNET105.Controllers
{
    public class CartController : Controller
    {
        LabDbContext context;

        public List<CartItemViewModel> _lstCart;
        public List<BillViewModel> _lstBill;

        public CartController()
        {
            _lstCart = new List<CartItemViewModel>();
            _lstBill = new List<BillViewModel>();
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
                                        ProductId = p.ProductId,
                                        ProductName = p.Product.Name,
                                        Price = p.Product.Price,
                                        Quantity = p.Quantity,
                                        TotalPrice = p.Quantity * p.Product.Price
                                    })
                                    .ToList();

            for (int i = 0; i < allCartItem.Count; i++)
            {
                CartItemViewModel cartItemViewModel = new CartItemViewModel
                {
                    Product = context.Products.Find(allCartItem[i].ProductId),
                    Quantity = allCartItem[i].Quantity,
                    TotalPrice = allCartItem[i].TotalPrice
                };
                _lstCart.Add(cartItemViewModel);

            }


            return View(allCartItem);
        }

        [HttpPost]
        public IActionResult ThanhToan(int id)
        {
            try
            {

                



                Product product = context.Products.FirstOrDefault(x => x.Id == id);
                if (product.Quantity > 1)
                {
                    --product.Quantity;
                }
                else
                {
                    context.Products.Remove(product);
                }
                context.SaveChanges();
                return RedirectToAction("ListBill", "Bill");

            }
            catch
            {
                return BadRequest();
            }




        }
    }
}

