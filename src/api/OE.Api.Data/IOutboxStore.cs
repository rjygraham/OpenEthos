using OE.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OE.Api.Data
{
	public interface IOutboxStore
	{
		Task<bool> SaveOutboxEntityAsync(OutboxEntity item);
	}
}
