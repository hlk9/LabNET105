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
            var listProduct = _context.Products.ToList();
            return View(listProduct);
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
            // Kiểm tra xem có đang đăng nhập ko, nếu ko thì bắt đăng nhập
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            Guid userId = Guid.Parse(HttpContext.Session.GetString("uid"));
            var getCartId = _context.Carts.FirstOrDefault(x => x.AccountId == userId);
            if (userId == null)
            {
                return RedirectToAction("Index", "Account"); // chuyển hướng về trang login
            }
            else
            {
                var cartItem = _context.CartItems.FirstOrDefault(p => p.CartId == getCartId.Id  && p.ProductId == productId);
                if (cartItem == null)
                {
                    
                    CartItem details = new CartItem()
                    {
                        ProductId = productId,
                        CartId = getCartId.Id,
                        Quantity = quantity
                    };
                    _context.CartItems.Add(details);
                    _context.SaveChanges();
                }
                else if (cartItem.Quantity >=  product.Quantity)
                {
                    return BadRequest(" Số lượng trong kho đã hết");
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity + quantity;
                    _context.CartItems.Update(cartItem); 
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Product");
        }

    }
}
