using Microsoft.Extensions.Configuration;

using MongoDB.Driver;

using System.Collections.Generic;
using System.Threading.Tasks;

using WebApp.Strategy.Models;

namespace WebApp.Strategy.StrategyRepository
{
    public class MongoDbRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _mongoCollection;

        public MongoDbRepository(IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString("MongoDbConnStr");
            var client = new MongoClient(connStr);
            var db = client.GetDatabase("ProductDb"); // yoksa oluşturur varsa return eder

            _mongoCollection = db.GetCollection<Product>("Products");
        }

        public async Task<Product> Add(Product product)
        {
            await _mongoCollection.InsertOneAsync(product);
            return product;
        }

        public async Task Delete(Product product)
        {
            await _mongoCollection.DeleteOneAsync(d => d.Id == product.Id);
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _mongoCollection.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByUserId(string userId)
        {
            return await _mongoCollection.Find(f => f.UserId == userId).ToListAsync();
        }

        public async Task Update(Product product)
        {
            await _mongoCollection.FindOneAndReplaceAsync(f => f.Id == product.Id, product);
        }
    }
}