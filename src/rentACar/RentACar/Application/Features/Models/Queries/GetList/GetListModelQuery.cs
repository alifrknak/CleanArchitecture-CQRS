using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Models.Queries.GetList;

public class GetListModelQuery : IRequest<GetListResponse<GetListModelListItemDto>>
{
	public PageRequest PageRequest { get; set; }

	public class GetListModelQueryHandler : IRequestHandler<GetListModelQuery, GetListResponse<GetListModelListItemDto>>
	{
		private readonly IModelRepository _brandRepository;
		private readonly IMapper _mapper;
		public GetListModelQueryHandler(IModelRepository brandRepository, IMapper mapper)
		{
			_brandRepository = brandRepository;
			_mapper = mapper;
		}
		public async Task<GetListResponse<GetListModelListItemDto>> Handle(GetListModelQuery request, CancellationToken cancellationToken)
		{
			Paginate<Model> models = await _brandRepository.GetListAsync(
				include: m => m.Include(b => b.Brand).Include(f => f.Fuel).Include(t => t.Transmission),
				index: request.PageRequest.PageIndex,
				size: request.PageRequest.PageSize
				);

			var response = _mapper.Map<GetListResponse<GetListModelListItemDto>>(models);
			return response;
		}
	}
}


