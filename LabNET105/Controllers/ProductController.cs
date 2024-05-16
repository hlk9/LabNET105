﻿using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

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

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int productId)
        {
            var objProduct = _context.Products.Find(productId);

            if(objProduct != null)
            {
                _context.Products.Remove(objProduct);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
