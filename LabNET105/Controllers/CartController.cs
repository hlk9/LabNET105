using DAL;
using DAL.Models;
using DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

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
                                        id = p.Id,
                                        ProductId = p.ProductId,
                                        ProductName = p.Product.Name,
                                        Price = p.Product.Price,
                                        Quantity = p.Quantity,
                                        TotalPrice = p.Quantity * p.Product.Price
                                    })
                                    .ToList();
            return View(allCartItem);
        }

        public IActionResult DeleteProductToCart(int id)
        {
            var objCartItem = context.CartItems.Find(id);

            context.CartItems.Remove(objCartItem);
            context.SaveChanges();
            return RedirectToAction("Index", "Cart");
            
        }

        public IActionResult ThanhToan()
        {

            Guid userId = Guid.Parse(HttpContext.Session.GetString("uid"));
            int cartId = context.Carts.FirstOrDefault(x => x.AccountId == userId).Id;
            var lstCartItems = context.CartItems.Where(x => x.CartId == cartId).ToList();

            if(lstCartItems.Count == 0 || lstCartItems.Count == null)
            {
                return NotFound("Có cái c gì đâu mà thanh toán");
            }
            else
            {
                for (int i = 0; i < lstCartItems.Count; i++)
                {
                    var product = context.Products.FirstOrDefault(x => x.Id == lstCartItems[i].ProductId);

                    if(product != null)
                    {
                        if(product.Quantity < lstCartItems[i].Quantity)
                        {
                            return BadRequest("Số lượng trong giỏ hàng lớn hơn số lượng trong kho");
                        }
                    }
                }
                Bill bill = new Bill
                {
                    AccountId = userId,
                    totalPrice = 0,
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
                    //context.SaveChanges();
                    if (context.Products.Find(lstCartItems[i].ProductId) != null)
                    {
                        var objProduct = context.Products.Find(lstCartItems[i].ProductId);
                        objProduct.Quantity -= lstCartItems[i].Quantity;
                        context.Products.Update(objProduct);
                        //context.SaveChanges();
                    }
                    if (context.CartItems.FirstOrDefault(x => x.CartId == lstCartItems[i].CartId) != null)
                    {
                        var objCartItem = context.CartItems.FirstOrDefault(x => x.CartId == lstCartItems[i].CartId);
                        context.CartItems.Remove(objCartItem);
                        //context.SaveChanges();

                    }

                }
                context.SaveChanges();

                var listBillDetail = context.BillDetails.Where(x => x.BillId == bill.Id).ToList();
                double total = 0;
                foreach (var item in listBillDetail)
                {
                    total += item.Price * item.Quantity;
                }

                var objBill = context.Bills.Find(bill.Id);
                objBill.totalPrice = total;
                context.Bills.Update(objBill);
                context.SaveChanges();

                return RedirectToAction("ListBill", "Bill");
            }
            
        }

        public IActionResult BuyBack(int id)
        {
            var lstBillDetail = context.BillDetails.Where(x => x.BillId == id).ToList();

            Guid userId = Guid.Parse(HttpContext.Session.GetString("uid"));
            var cartId = context.Carts.FirstOrDefault(x => x.AccountId == userId).Id;

            var lstCartItems = context.CartItems.Where(x => x.CartId == cartId).ToList();



            for (int i = 0; i < lstBillDetail.Count; i++)
            {
                if(lstCartItems.Count == 0 || lstCartItems.Count == null)
                {
                    CartItem cartItem = new CartItem
                    {
                        CartId = cartId,
                        ProductId = lstBillDetail[i].ProductId,
                        Quantity = lstBillDetail[i].Quantity
                    };

                    context.CartItems.Add(cartItem);
                    context.SaveChanges();
                }
                else
                {
                    for (int j = 0; j < lstCartItems.Count; j++)
                    {
                        if (lstBillDetail[i].ProductId == lstCartItems[j].ProductId)
                        {
                            var objCartitem = context.CartItems.Find(lstCartItems[j].Id);

                            objCartitem.Quantity += lstBillDetail[i].Quantity;

                            context.CartItems.Update(objCartitem);
                            context.SaveChanges();
                        }

                    }
                }
            }

            return RedirectToAction("Index", "Cart");
        }
    }
}

