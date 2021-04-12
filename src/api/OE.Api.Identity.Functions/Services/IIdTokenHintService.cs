using System;

namespace OE.Api.Identity.Functions.Services
{
	public interface IIdTokenHintService
	{
		string GetIdTokenHint(string emailAddress, string ancestorId, DateTime notBefore, DateTime expires);
	}
}
