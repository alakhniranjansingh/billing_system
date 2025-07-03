using InvoiceBillingAPI.DTOs;

namespace InvoiceBillingAPI.Repositories.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<int> CreateInvoiceAsync(CreateInvoiceDto dto);
        Task<InvoiceSummaryDto?> GetInvoiceSummaryAsync(int invoiceId);
    }
}
