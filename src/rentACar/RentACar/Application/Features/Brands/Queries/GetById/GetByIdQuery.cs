using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Queries.GetById;

public class GetByIdBrandQuery : IRequest<GetByIdBrandResponse>, ICachebleRequest, ILoggableRequest
{
	public Guid Id { get; set; }
	public string Name { get; set; }

	public string CacheKey => $"GetByIdBrandQuery({Id})";

	public bool ByPassCache { get; }

	public TimeSpan? SlidingExpiration { get; }

	public string? CacheGroupKey => "GetBrands";

	public class GetByIdQueryHandler : IRequestHandler<GetByIdBrandQuery, GetByIdBrandResponse>
	{
		private readonly IBrandRepository _brandRepository;
		private readonly IMapper _mapper;
		public GetByIdQueryHandler(IBrandRepository brandRepository, IMapper mapper)
		{
			_brandRepository = brandRepository;
			_mapper = mapper;
		}
		public async Task<GetByIdBrandResponse> Handle(GetByIdBrandQuery request, CancellationToken cancellationToken)
		{
			Brand? brand = await _brandRepository.GetAsync(brand => brand.Id == request.Id,
				cancellationToken: cancellationToken);

			GetByIdBrandResponse response = _mapper.Map<GetByIdBrandResponse>(brand);
			return response;
		}
	}
}
