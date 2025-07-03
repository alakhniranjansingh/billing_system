using Dapper;
using InvoiceBillingAPI.Data;
using InvoiceBillingAPI.Models;
//using InvoiceBillingAPI.NewFolder.Repositories.Interfaces;
using InvoiceBillingAPI.Repositories.Interfaces;

namespace InvoiceBillingAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var query = "SELECT * FROM Products WHERE IsActive = TRUE";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(query);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Products WHERE ProductId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Product>(query, new { Id = id });
        }

        public async Task<int> CreateAsync(Product product)
        {
            var query = @"INSERT INTO Products (ProductName, Price, Stock, IsActive)
                          VALUES (@ProductName, @Price, @Stock, @IsActive)
                          RETURNING ProductId";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, product);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var query = @"UPDATE Products 
                          SET ProductName = @ProductName, Price = @Price, Stock = @Stock, IsActive = @IsActive
                          WHERE ProductId = @ProductId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, product);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Products WHERE ProductId = @Id";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { Id = id });
            return rows > 0;
        }
    }
}
