using System.ComponentModel.DataAnnotations.Schema;

namespace MicroRabbit.Banking.Domain.Models
{
  public class Account
  {
    public int Id { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string AccountType { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal AccountBalance { get; set; }
  }
}