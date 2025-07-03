using Dapper;
using InvoiceBillingAPI.Data;
using InvoiceBillingAPI.DTOs;
using InvoiceBillingAPI.Repositories.Interfaces;

namespace InvoiceBillingAPI.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DapperContext _context;

        public InvoiceRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateInvoiceAsync(CreateInvoiceDto dto)
        {
            using var connection = _context.CreateConnection();
            connection.Open(); // ✅ Important: Open connection before transaction
            using var transaction = connection.BeginTransaction();

            try
            {
                var insertInvoiceQuery = @"
                    INSERT INTO Invoices (CustomerId, InvoiceDate, Discount, Tax, CreatedAt)
                    VALUES (@CustomerId, @InvoiceDate, @Discount, @Tax, CURRENT_TIMESTAMP)
                    RETURNING InvoiceId;
                ";

                var invoiceId = await connection.ExecuteScalarAsync<int>(
                    insertInvoiceQuery,
                    new
                    {
                        dto.CustomerId,
                        dto.InvoiceDate,
                        dto.Discount,
                        dto.Tax
                    },
                    transaction
                );

                var insertItemQuery = @"
                    INSERT INTO InvoiceItems (InvoiceId, ProductId, Quantity)
                    VALUES (@InvoiceId, @ProductId, @Quantity);
                ";

                foreach (var item in dto.Items)
                {
                    await connection.ExecuteAsync(
                        insertItemQuery,
                        new
                        {
                            InvoiceId = invoiceId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        },
                        transaction
                    );
                }

                transaction.Commit(); // ✅ Synchronous commit
                return invoiceId;
            }
            catch
            {
                transaction.Rollback(); // ✅ Synchronous rollback
                throw;
            }
        }

        public async Task<InvoiceSummaryDto?> GetInvoiceSummaryAsync(int invoiceId)
        {
            var query = @"
                SELECT 
                    i.InvoiceId,
                    c.FullName AS CustomerName,
                    i.InvoiceDate,
                    i.TotalAmount,
                    i.Tax,
                    i.Discount,
                    i.GrandTotal
                FROM Invoices i
                INNER JOIN Customers c ON i.CustomerId = c.CustomerId
                WHERE i.InvoiceId = @InvoiceId";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<InvoiceSummaryDto>(query, new { InvoiceId = invoiceId });
        }
    }
}
