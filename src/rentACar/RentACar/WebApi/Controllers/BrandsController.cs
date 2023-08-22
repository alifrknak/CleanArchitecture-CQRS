using Application.Features.Brands.Commands.Create;
using Application.Features.Brands.Commands.Delete;
using Application.Features.Brands.Commands.Update;
using Application.Features.Brands.Queries.GetById;
using Application.Features.Brands.Queries.GetList;
using Core.Application.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BrandsController : BaseController
	{
		[HttpPost("add")]
		public async Task<IActionResult> Add([FromBody] CreateBrandCommand createBrandCommand)
		{
			return Ok(await Mediator.Send(createBrandCommand));
		}

		[HttpGet]
		public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
		{
			GetListBrandQuery getListBrandQuery = new GetListBrandQuery { PageRequest = pageRequest };
			var r = await Mediator.Send(getListBrandQuery);
			return Ok(r);
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetList([FromRoute] Guid id)
		{
			var getByIdbrandQuery = new GetByIdBrandQuery { Id = id };
			return Ok(await Mediator.Send(getByIdbrandQuery));
		}

		[HttpPut("update")]
		public async Task<IActionResult> Update([FromBody] UpdateBrandCommand updateBrandCommand)
		{
			return Ok(await Mediator.Send(updateBrandCommand));
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] Guid id)
		{
			DeleteBrandCommand deleteBrandCommand = new DeleteBrandCommand { Id = id };
			return Ok(await Mediator.Send(deleteBrandCommand));
		}
	}
}
