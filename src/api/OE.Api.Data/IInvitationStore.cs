using OE.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OE.Api.Data
{
	public interface IInvitationStore
	{
		Task<bool> CreateInvitationAsync(InvitationEntity item);
		Task<InvitationEntity> GetInvitationEntityAsync(string id);
		Task<bool> UpdateInvitationEntityAsync(InvitationEntity item);
	}
}
