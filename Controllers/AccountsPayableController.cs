using Microsoft.AspNetCore.Mvc;
using Dapper;
using Oasis.Models;

[Route("api/[controller]")]
[ApiController]
public class AccountsPayableController : ControllerBase
{
    private readonly DapperContext _context;

    public AccountsPayableController(DapperContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        using var connection = _context.CreateConnection();
        var accounts = await connection.QueryAsync<AccountsPayable>("SELECT * FROM accountsPayable");
        return Ok(accounts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var account = await connection.QueryFirstOrDefaultAsync<AccountsPayable>("SELECT * FROM accountsPayable WHERE id = @Id", new { Id = id });
        return account == null ? NotFound() : Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AccountsPayable account)
    {
        using var connection = _context.CreateConnection();
        var sql = "INSERT INTO accountsPayable (name, invoiceNumber, date, amount, status, dueDate) VALUES (@Name, @InvoiceNumber, @Date, @Amount, @Status, @DueDate)";
        await connection.ExecuteAsync(sql, account);
        return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AccountsPayable account)
    {
        using var connection = _context.CreateConnection();
        var sql = "UPDATE accountsPayable SET name=@Name, invoiceNumber=@InvoiceNumber, date=@Date, amount=@Amount, status=@Status, dueDate=@DueDate WHERE id=@Id";
        var result = await connection.ExecuteAsync(sql, new { account.Name, account.InvoiceNumber, account.Date, account.Amount, account.Status, account.DueDate, Id = id });
        return result > 0 ? NoContent() : NotFound();
    }
  [HttpPut("PayNow/{id}")]
public async Task<IActionResult> PayNow(int id, [FromBody] Payment paymentDto)
{
    using var connection = _context.CreateConnection();
    connection.Open(); // Open the connection before beginning a transaction
    using var transaction = connection.BeginTransaction();
    try
    {
        // 1. Update the accounts payable record to mark it as paid.
        var updateSql = "UPDATE accountsPayable SET status = @Status WHERE id = @Id";
        var updateResult = await connection.ExecuteAsync(
            updateSql, 
            new { Status = "Paid", Id = id }, 
            transaction
        );
        
        if (updateResult <= 0)
        {
            transaction.Rollback();
            return NotFound(new { Message = "Accounts payable record not found." });
        }

        // 2. Insert a new payment record with the provided details.
        var insertSql = @"
            INSERT INTO payment 
                (referenceId, invoiceNumber, name, type, paymentDate, paymentMethod, amount, referenceNumber) 
            VALUES 
                (@ReferenceId, @InvoiceNumber, @Name, @Type, @PaymentDate, @PaymentMethod, @Amount, @ReferenceNumber)";
        var insertResult = await connection.ExecuteAsync(insertSql, paymentDto, transaction);

        transaction.Commit();
        return NoContent();
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        var errorResponse = new 
        { 
            Message = "An error occurred while processing the payment.", 
            Error = ex.Message 
        };
        return StatusCode(500, errorResponse);
    }
}


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var result = await connection.ExecuteAsync("DELETE FROM accountsPayable WHERE id = @Id", new { Id = id });
        return result > 0 ? NoContent() : NotFound();
    }


}
