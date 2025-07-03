using InvoiceBillingAPI.DTOs;
using InvoiceBillingAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository _repo;

        public InvoiceController(IInvoiceRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice(CreateInvoiceDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                return BadRequest("Invoice must contain at least one item.");

            var invoiceId = await _repo.CreateInvoiceAsync(dto);
            return Ok(new { Message = "Invoice created successfully", InvoiceId = invoiceId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceSummary(int id)
        {
            var summary = await _repo.GetInvoiceSummaryAsync(id);
            if (summary == null) return NotFound();
            return Ok(summary);
        }
    }
}
