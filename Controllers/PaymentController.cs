using Dapper;
using Microsoft.AspNetCore.Mvc;
using Oasis.Models;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly DapperContext _context;

    public PaymentController(DapperContext context)
    {
        _context = context;
    }

    // Get all payments
    [HttpGet]
    public async Task<IActionResult> GetPayments()
    {
        var query = "SELECT * FROM payment";

        using (var connection = _context.CreateConnection())
        {
            var payments = await connection.QueryAsync<Payment>(query);
            return Ok(payments);
        }
    }

    // Get a single payment by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentById(int id)
    {
        var query = "SELECT * FROM payment WHERE id = @Id";

        using (var connection = _context.CreateConnection())
        {
            var payment = await connection.QuerySingleOrDefaultAsync<Payment>(query, new { Id = id });

            if (payment == null)
                return NotFound("Payment not found");

            return Ok(payment);
        }
    }

    // Create a new payment
    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
    {
        var query = @"INSERT INTO payment (referenceId, invoiceNumber, name, type, paymentDate, paymentMethod, amount, referenceNumber) 
                      VALUES (@ReferenceId, @InvoiceNumber, @Name, @Type, @PaymentDate, @PaymentMethod, @Amount, @ReferenceNumber)";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, payment);
            return Ok("Payment created successfully");
        }
    }

    // Update an existing payment
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayment(int id, [FromBody] Payment payment)
    {
        var query = @"UPDATE payment SET 
                        referenceId = @ReferenceId, 
                        invoiceNumber = @InvoiceNumber, 
                        name = @Name, 
                        type = @Type, 
                        paymentDate = @PaymentDate, 
                        paymentMethod = @PaymentMethod, 
                        amount = @Amount, 
                        referenceNumber = @ReferenceNumber 
                      WHERE id = @Id";

        using (var connection = _context.CreateConnection())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { payment.ReferenceId, payment.InvoiceNumber, payment.Name, payment.Type, payment.PaymentDate, payment.PaymentMethod, payment.Amount, payment.ReferenceNumber, Id = id });

            if (affectedRows == 0)
                return NotFound("Payment not found");

            return Ok("Payment updated successfully");
        }
    }

    // Delete a payment
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var query = "DELETE FROM payment WHERE id = @Id";

        using (var connection = _context.CreateConnection())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });

            if (affectedRows == 0)
                return NotFound("Payment not found");

            return Ok("Payment deleted successfully");
        }
    }
}
