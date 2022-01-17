using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

using WebApp.Strategy.Models;
using WebApp.Strategy.StrategyRepository;

namespace WebApp.Strategy.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly UserManager<AppUser> _userManager;

        public ProductsController(IProductRepository productRepository, UserManager<AppUser> userManager)
        {
            _productRepository = productRepository;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(await _productRepository.GetProductsByUserId(user.Id));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var product = await _productRepository.GetProductById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,UnitInStock,UserId,CreatedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                product.UserId = user.Id;
                product.CreatedDate = DateTime.Now;

                await _productRepository.Add(product);

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var product = await _productRepository.GetProductById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Price,UnitInStock,UserId,CreatedDate")] Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.Update(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var product = await _productRepository.GetProductById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _productRepository.GetProductById(id);
            await _productRepository.Delete(product);

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _productRepository.GetProductById(id) != null;
        }
    }
}