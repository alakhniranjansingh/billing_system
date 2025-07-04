﻿using Dapper;
using InvoiceBillingAPI.Data;
using InvoiceBillingAPI.Models;
using InvoiceBillingAPI.Repositories.Interfaces;

namespace InvoiceBillingAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DapperContext _context;

        public CustomerRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var query = "SELECT * FROM Customers";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Customer>(query);
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Customers WHERE CustomerId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
        }

        public async Task<int> CreateAsync(Customer customer)
        {
            var query = @"INSERT INTO Customers (FullName, Email, Phone, Address)
                          VALUES (@FullName, @Email, @Phone, @Address)
                          RETURNING CustomerId";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, customer);
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            var query = @"UPDATE Customers 
                          SET FullName = @FullName, Email = @Email, Phone = @Phone, Address = @Address
                          WHERE CustomerId = @CustomerId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, customer);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Customers WHERE CustomerId = @Id";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { Id = id });
            return rows > 0;
        }
    }
}
