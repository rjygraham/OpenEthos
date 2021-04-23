using System;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace OE.Api.MicrosoftGraph
{
	public interface IO365GraphService
	{
		Task SendEmailAsync(Message message);
	}
}
