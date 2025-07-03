namespace InvoiceBillingAPI.DTOs
{
    public class InvoiceItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateInvoiceDto
    {
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();
    }

    public class InvoiceSummaryDto
    {
        public int InvoiceId { get; set; }
        public string CustomerName { get; set; } = null!;
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
