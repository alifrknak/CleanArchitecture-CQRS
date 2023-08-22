using Application.Features.Brands.Queries.GetList;
using Application.Features.Models.Queries.GetList;
using Application.Features.Models.Queries.GetListByDynamic;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ModelsController : BaseController
	{
		[HttpGet]
		public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
		{
			GetListModelQuery getListModelQuery = new GetListModelQuery { PageRequest = pageRequest };
			return Ok(await Mediator.Send(getListModelQuery));
		}

		[HttpPost("ByDynamic")]
		public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery dynamicQuery = null)
		{
			GetListByDynamicModelQuery getListByDynamicModelQuery = new GetListByDynamicModelQuery { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
			return Ok(await Mediator.Send(getListByDynamicModelQuery));
		}
	}
}
