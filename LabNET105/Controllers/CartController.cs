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
            return View(allCartItem);
        }

        public IActionResult ThanhToan()
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("uid"));
            var lstCartItems = context.CartItems.ToList();
            Bill bill = new Bill
            {
                AccountId = userId,
                totalPrice = 100000,
                Status = 1,
                BuyDate = DateTime.Now

            };
            context.Bills.Add(bill);
            context.SaveChanges();
            for (int i = 0; i < lstCartItems.Count; i++)
            {
                BillDetail billdetails = new BillDetail()
                {
                    BillId = bill.Id,
                    ProductId = lstCartItems[i].ProductId,
                    Quantity = lstCartItems[i].Quantity,
                    Price = context.Products.FirstOrDefault(x => x.Id == lstCartItems[i].ProductId).Price
                };
                context.BillDetails.Add(billdetails);
                context.SaveChanges();
                if (context.Products.Find(lstCartItems[i].ProductId) != null)
                {
                    var objProduct = context.Products.Find(lstCartItems[i].ProductId);
                    objProduct.Quantity -= lstCartItems[i].Quantity;
                    context.Products.Update(objProduct);
                    context.SaveChanges();
                }
                if (context.CartItems.FirstOrDefault(x => x.ProductId == lstCartItems[i].ProductId) != null)
                {
                    var objCartItem = context.CartItems.FirstOrDefault(x => x.ProductId == lstCartItems[i].ProductId);
                    context.CartItems.Remove(objCartItem);
                    context.SaveChanges();

                }

            }
            //context.SaveChanges();
            return RedirectToAction("ListBill", "Bill");





  

               




        }
    }
}

