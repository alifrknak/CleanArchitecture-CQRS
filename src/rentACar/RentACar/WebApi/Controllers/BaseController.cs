using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	public class BaseController : ControllerBase
	{
		private IMediator? _mediator;
		public IMediator? Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
	}
}
