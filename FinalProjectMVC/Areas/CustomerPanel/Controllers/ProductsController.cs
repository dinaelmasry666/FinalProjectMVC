﻿using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.CustomerPanel.ViewModel;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Areas.SellerPanel.ViewModel;
using FinalProjectMVC.Constants;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinalProjectMVC.Areas.CustomerPanel.Controllers
{
    [Area("CustomerPanel")]
    public class ProductsController : Controller
    {

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<SellerProduct> _sellerProductRepo;

        public ProductsController(
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<SellerProduct> sellerProductRepo
            )

        {
            _productRepository = productRepository;
            this._categoryRepository = categoryRepository;
            this._sellerProductRepo = sellerProductRepo;
        }

        // GET: SellerPanel/Products
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.FilterAsync(p => p.SellerProducts?.Any(x => x.Count > 0) ?? false);
            var viewModelList = await CreateDisplayViewModelList(products);
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View(viewModelList);
        }

        public async Task<IActionResult> filter(int id)
        {
            var products = await _productRepository.FilterAsync(p => (p.SellerProducts?.Any(x => x.Count > 0) ?? false) && p.SubCategoryId == id);
            var viewModelList = await CreateDisplayViewModelList(products);
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View("Index", viewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string product_Name)
        {
            var products = await _productRepository.FilterAsync(p => (p.SellerProducts?.Any(x => x.Count > 0) ?? false) && p.Name.Contains(product_Name, StringComparison.OrdinalIgnoreCase));
            var viewModelList = await CreateDisplayViewModelList(products);
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View("Index", viewModelList);
        }

        private async Task<List<DisplayInStockProductsViewModel>> CreateDisplayViewModelList(IEnumerable<Product> products)
        {
            var viewModelList = new List<DisplayInStockProductsViewModel>();
            foreach (var product in products)
            {
                var sellerProducts = await _sellerProductRepo.FilterAsync(sp => sp.ProductId == product.Id && sp.Count > 0);
                if (sellerProducts != null)
                {
                    var lowestPrice = sellerProducts.Min(sp => sp.Price);

                    // Returns the Cheapest instance of sold by a seller.
                    var sellerProductWithLowestPrice = sellerProducts.FirstOrDefault(sp => sp.Price == lowestPrice);

                    var productViewModel = new DisplayInStockProductsViewModel
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductPrice = lowestPrice,
                        ProductDescription = product.Description,
                        ProductImage = product.ProductImage,
                        SellerId = sellerProductWithLowestPrice?.SellerId,
                        SellerNameWithLowestPrice = sellerProductWithLowestPrice?.Seller?.ApplicationUser?.FirstName,
                        Count = sellerProductWithLowestPrice.Count,
                        Brand = product?.Brand?.Name,
                        SubCategory = product?.SubCategory?.Name
                    };
                    viewModelList.Add(productViewModel);
                }
            }
            return viewModelList;
        }




        // GET: SellerPanel/Products/Details/5
        [Route("Product/{id:int}/{SellerId}")]
        public async Task<IActionResult> Details(int? id, string? SellerId)
        {
            if (id == null)
            {
                return NotFound();
            }


            var sellerProductRow = (await _sellerProductRepo.FilterAsync(sp => sp.ProductId == id && sp.SellerId == SellerId)).FirstOrDefault();
            if (sellerProductRow == null)
            {
                return NotFound();
            }

            var availableSellers = await _sellerProductRepo.FilterAsync(sp => sp.ProductId == id && sp.Count > 0);

            ViewData["SellerName"] = new SelectList(availableSellers, "SellerId", "DataTextFieldLabel", SellerId);


            var DetailedProductviewModel = new DetailedProductViewModel
            {
                ProductId = sellerProductRow.ProductId,
                ProductName = sellerProductRow.Product?.Name,
                ProductPrice = sellerProductRow.Price,
                ProductDescription = sellerProductRow.Product?.Description,
                ProductImage = sellerProductRow.Product?.ProductImage,

                SellerName = sellerProductRow.Seller?.ApplicationUser?.FirstName,
                SellerId = sellerProductRow.SellerId,
                Count = sellerProductRow.Count,

                CurrentSellerProduct = sellerProductRow,

                Brand = sellerProductRow.Product?.Brand?.Name,
                SubCategory = sellerProductRow.Product?.SubCategory?.Name,

                ReviewsList = sellerProductRow.Product.Reviews.Where(r => !r.IsDeleted).OrderByDescending(r => r.CreatedDate).ToList()


            };


            return View(DetailedProductviewModel);
        }






    }
}

