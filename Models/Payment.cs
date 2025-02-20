using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oasis.Models
{
    public class Payment
{
    public int Id { get; set; }
    public int ReferenceId { get; set; }
    public string InvoiceNumber { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string ReferenceNumber { get; set; }
}

}