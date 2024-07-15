using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pizzario.Web.Models;
using Pizzario.Web.Service.IService;

namespace Pizzario.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductList()
        {
            List<Products>? productList = new List<Products>();
            ResponseDto? response = await _productService.GetAllProductsAsync();
            if (response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<Products>>(Convert.ToString(response.Result));
            }
            return View(productList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Products product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ResponseDto response = await _productService.CreateProductAsync(product);
                    if (response.IsSuccess)
                    {
                        TempData["success"] = "Product created successfully!";
                        return RedirectToAction("ProductList");
                    }
                    else
                    {
                        TempData["error"] = response?.Message;
                        ModelState.AddModelError("", "Failed to create coupon. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            return View("ProductList", await _productService.GetAllProductsAsync());
        }

        //[HttpDelete]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            ResponseDto? response = await _productService.DeleteProductAsync(productId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully!";
                return RedirectToAction("ProductList");

            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }
    }
}
