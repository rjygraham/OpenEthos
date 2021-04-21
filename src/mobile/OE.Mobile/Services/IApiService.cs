using System;
using System.Collections.Generic;
using System.Text;

namespace OE.Mobile.Services
{
	public interface IApiService : IOutboxApiService, IProfileApiService
	{
	}
}
