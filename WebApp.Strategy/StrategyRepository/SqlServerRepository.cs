using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApp.Strategy.Models;

namespace WebApp.Strategy.StrategyRepository
{
    public class SqlServerRepository : IProductRepository
    {
        private readonly AppIdentityDbContext _appIdentityDbContext;

        public SqlServerRepository(AppIdentityDbContext appIdentityDbContext)
        {
            _appIdentityDbContext = appIdentityDbContext;
        }

        public async Task<Product> Add(Product product)
        {
            product.Id = Guid.NewGuid().ToString(); // mongo db id yi kendi ürettiği için orada bu tanımı yapmadık. Sql server için yaptık sadece

            await _appIdentityDbContext.Products.AddAsync(product);
            await _appIdentityDbContext.SaveChangesAsync();

            return product;
        }

        public async Task Delete(Product product)
        {
            // RemoveAsync  olmaması => bu işlem sadece context deki objenin state ini değiştiriyor. O yüzden Remove işleminin async metodu yok
            //_appIdentityDbContext.Entry(product).State = EntityState.Deleted; // => bunun ile alt satırdaki işlem aynı işi yapıyor
            _appIdentityDbContext.Remove(product);
            await _appIdentityDbContext.SaveChangesAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _appIdentityDbContext.Products.FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Product>> GetProductsByUserId(string userId)
        {
            return await _appIdentityDbContext.Products.Where(w => w.UserId == userId).ToListAsync().ConfigureAwait(false);
        }

        public async Task Update(Product product)
        {
            // Update işleminin de async özelliği yok çünkü update işlemi de, o anki objenin property lerini set ettiği için async e gerek yok
            _appIdentityDbContext.Products.Update(product);
            await _appIdentityDbContext.SaveChangesAsync();
        }
    }
}