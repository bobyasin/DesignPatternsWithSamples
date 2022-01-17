using System.Collections.Generic;
using System.Threading.Tasks;

using WebApp.Strategy.Models;

namespace WebApp.Strategy.StrategyRepository
{
    public interface IProductRepository
    {
        Task<Product> GetProductById(string id);

        Task<IEnumerable<Product>> GetProductsByUserId(string userId);

        Task<Product> Add(Product product);

        Task Update(Product product);

        Task Delete(Product product);
    }
}