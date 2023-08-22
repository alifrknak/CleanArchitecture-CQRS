using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Commands.Update
{
	public class UpdateBrandCommand : IRequest<UpdatedBrandResponse>, ICacheRemoverRequest, ILoggableRequest
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public string CacheKey => "";

		public bool ByPassCache { get; }

		public string? CacheGroupKey => "GetBrands";

		public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, UpdatedBrandResponse>
		{
			private readonly IBrandRepository _brandRepository;
			private readonly IMapper _mapper;
			public UpdateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
			{
				_brandRepository = brandRepository;
				_mapper = mapper;
			}
			public async Task<UpdatedBrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
			{
				Brand? brand = await _brandRepository.GetAsync(brand => brand.Id == request.Id, cancellationToken: cancellationToken);

				brand = _mapper.Map(request, brand);

				await _brandRepository.UpdateAsync(brand);

				UpdatedBrandResponse response = _mapper.Map<UpdatedBrandResponse>(brand);
				return response;
			}
		}
	}
}
