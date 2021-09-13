using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Application.Models
{
	public class AccountTransfer
	{
		public int AccountSource { get; set; }
		public int AccountDestination { get; set; }

		[Column(TypeName = "decimal(12,2)")]
		public decimal TransferAmount { get; set; }
	}
}