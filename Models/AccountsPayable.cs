using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oasis.Models
{
   public class AccountsPayable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string InvoiceNumber { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public DateTime DueDate { get; set; }
}

}