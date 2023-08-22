using Application.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence;

public static class PersistenceServiceRegistration
{
	public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
	{

		services.AddDbContext<BaseDbContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString("RentACar"));
		});

		services.AddScoped<IBrandRepository, BrandRepository>();
		services.AddScoped<IModelRepository, ModelRepository>();

		return services;
	}

}
