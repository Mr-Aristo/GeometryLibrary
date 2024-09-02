using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLibrary.Abstracts;
using Serilog;
using StackExchange.Redis;

namespace GeometryLibrary
{
    public class ShapeAreaCalculator<T> : IShapeAreaCalculator<T> where T : IAreaCalculable
    {
        private readonly ILogger _logger;
        private readonly IDatabase _redisCache;

        public ShapeAreaCalculator(ILogger logger, IDatabase redisCache)
        {
            _logger = logger;
            _redisCache = redisCache;
        }

        public async Task<double> CalculateAreaAsync(T shape)
        {
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));

            string cacheKey = $"area_{shape.GetType().Name}_{shape.GetHashCode()}";
            string cachedArea = await _redisCache.StringGetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedArea))
            {
                _logger.Information("Retrieved area from cache for shape {ShapeName}", shape.GetType().Name);
                return double.Parse(cachedArea);
            }

            var area = shape.CalculateArea();

            await _redisCache.StringSetAsync(cacheKey, area.ToString());

            _logger.Information("Calculated and cached area for shape {ShapeName}: {Area}", shape.GetType().Name, area);

            return area;
        }
    }
}
