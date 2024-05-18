using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace LabNET105.Controllers
{
    public class ProductController : Controller
    {
        LabDbContext _context;

        public ProductController()
        {
            _context = new LabDbContext();
        }

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("uid") == null)
            {
                return RedirectToAction("Index", "Access");
            }
            else
            {
                var listProduct = _context.Products.ToList();
                return View(listProduct);
            }
        }

        public IActionResult Detail(int productId)
        {
            var objProduct = _context.Products.Find(productId);
            return View(objProduct);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {

            }

            return RedirectToAction("Create");
        }
        public IActionResult Edit(int productId)
        {
            var objProduct = _context.Products.Find(productId);
            return View(objProduct);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var objProduct = _context.Products.Find(product.Id);

            objProduct.Name = product.Name;
            objProduct.Description = product.Description;
            objProduct.Price = product.Price;
            objProduct.Quantity = product.Quantity;
            objProduct.Status = product.Status;

            _context.Products.Update(objProduct);
            _context.SaveChanges();

            return RedirectToAction("Edit");
        }

        public IActionResult Delete(int productId)
        {
            var objProduct = _context.Products.Find(productId);

            if (objProduct != null)
            {
                _context.Products.Remove(objProduct);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

       

        public IActionResult AddToCart(int productId, int quantity)
        {
            // Kiểm tra xem có đang đăng nhập không, nếu không thì bắt đăng nhập
            var userIdString = HttpContext.Session.GetString("uid");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Index", "Account"); // chuyển hướng về trang login
            }

            Guid userId = Guid.Parse(userIdString);

            // Lấy sản phẩm từ cơ sở dữ liệu
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại");
            }

            // Kiểm tra xem có giỏ hàng cho tài khoản này không
            var cart = _context.Carts.FirstOrDefault(x => x.AccountId == userId);
            if (cart == null)
            {
                return NotFound("Giỏ hàng không tồn tại");
            }

            // Kiểm tra xem số lượng yêu cầu có lớn hơn số lượng tồn kho không
            if (quantity > product.Quantity)
            {
                return BadRequest("Số lượng yêu cầu vượt quá số lượng tồn kho");
            }

            // Tìm kiếm sản phẩm trong giỏ hàng
            var cartItem = _context.CartItems.FirstOrDefault(p => p.CartId == cart.Id && p.ProductId == productId);
            if (cartItem == null)
            {
                // Thêm sản phẩm mới vào giỏ hàng
                CartItem newCartItem = new CartItem()
                {
                    ProductId = productId,
                    CartId = cart.Id,
                    Quantity = quantity
                };
                _context.CartItems.Add(newCartItem);
            }
            else
            {
                // Kiểm tra xem số lượng cập nhật có vượt quá số lượng tồn kho không
                if (cartItem.Quantity + quantity > product.Quantity)
                {
                    return BadRequest("Số lượng trong kho không đủ để thêm số lượng yêu cầu");
                }

                // Cập nhật số lượng sản phẩm trong giỏ hàng
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            // Chuyển hướng về trang sản phẩm
            return RedirectToAction("Index", "Product");
        }


    }
}
