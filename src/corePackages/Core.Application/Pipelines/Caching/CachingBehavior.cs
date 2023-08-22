using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>, ICachebleRequest
{

	private readonly CacheSetting _cacheSetting;
	private readonly IDistributedCache _cache;

	public CachingBehavior(IDistributedCache cache, IConfiguration configuration)
	{
		_cacheSetting = configuration.GetSection("CacheSettings").Get<CacheSetting>() ?? throw new InvalidOperationException();
		_cache = cache;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (request.ByPassCache)
			return await next();

		TResponse response;
		byte[]? buffer = await _cache.GetAsync(request.CacheKey, cancellationToken);

		if (buffer is not null)
		{
			response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(buffer));
		}
		else
		{
			response = await getResponseAndAddToCache(request, next, cancellationToken);
		}

		return response;
	}

	private async Task<TResponse?> getResponseAndAddToCache(TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		TResponse response = await next();

		TimeSpan slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSetting.SlidingExpiration);
		DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };

		byte[] serializedData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

		await _cache.SetAsync(request.CacheKey, serializedData, cacheOptions, cancellationToken);

		if (request.CacheGroupKey is not null)
		{
			await addCacheKeyToGroup(request, slidingExpiration, cancellationToken);
		}

		return response;
	}

	private async Task addCacheKeyToGroup(TRequest request, TimeSpan slidingExpiration, CancellationToken cancellationToken)
	{

		byte[]? cacheGroupCache = await _cache.GetAsync(key: request.CacheGroupKey!, cancellationToken);
		HashSet<string> cacheKeysInGroup;
		if (cacheGroupCache != null)
		{
			cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroupCache))!;
			if (!cacheKeysInGroup.Contains(request.CacheKey))
				cacheKeysInGroup.Add(request.CacheKey);
		}
		else
		{
			cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });
		}
		
		byte[] newCacheGroupCache = JsonSerializer.SerializeToUtf8Bytes(cacheKeysInGroup);

		byte[]? cacheGroupCacheSlidingExpirationCache = await _cache.GetAsync(
			key: $"{request.CacheGroupKey}SlidingExpiration",
			cancellationToken
		);
		int? cacheGroupCacheSlidingExpirationValue = null;
		if (cacheGroupCacheSlidingExpirationCache != null)
			cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(Encoding.Default.GetString(cacheGroupCacheSlidingExpirationCache));
		if (cacheGroupCacheSlidingExpirationValue == null || slidingExpiration.TotalSeconds > cacheGroupCacheSlidingExpirationValue)
			cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(slidingExpiration.TotalSeconds);
		byte[] serializeCachedGroupSlidingExpirationData = JsonSerializer.SerializeToUtf8Bytes(cacheGroupCacheSlidingExpirationValue);

		DistributedCacheEntryOptions cacheOptions =
			new() { SlidingExpiration = TimeSpan.FromSeconds(Convert.ToDouble(cacheGroupCacheSlidingExpirationValue)) };

		await _cache.SetAsync(key: request.CacheGroupKey!, newCacheGroupCache, cacheOptions, cancellationToken);

		await _cache.SetAsync(
			key: $"{request.CacheGroupKey}SlidingExpiration",
			serializeCachedGroupSlidingExpirationData,
			cacheOptions,
			cancellationToken
		);
	}
}
