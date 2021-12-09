using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using ProductsCRUD.AutomatedCacher.Interface;
using ProductsCRUD.AutomatedCacher.Model;
using ProductsCRUD.DomainModels;
using ProductsCRUD.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductsCRUD.AutomatedCacher.Concrete
{
    public class MemoryCacheAutomater : IMemoryCacheAutomater
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheModel _memoryCacheModel;

        public MemoryCacheAutomater(IProductsRepository ordersRepository, IMemoryCache memoryCache, IOptions<MemoryCacheModel> memoryCacheModel)
        {
            _productsRepository = ordersRepository;
            _memoryCache = memoryCache;
            _memoryCacheModel = memoryCacheModel.Value;
        }

        public void AutomateCache()
        {
            RegisterCache(_memoryCacheModel.Products, null, EvictionReason.None, null);
        }

        private MemoryCacheEntryOptions GetMemoryCacheEntryOptions()
        {
            int cacheExpirationMinutes = 1;
            DateTime cacheExpirationTime = DateTime.Now.AddMinutes(cacheExpirationMinutes);
            CancellationChangeToken cacheExpirationToken = new CancellationChangeToken
            (
                new CancellationTokenSource(TimeSpan.FromMinutes(cacheExpirationMinutes + 0.01)).Token
            );

            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cacheExpirationTime)
                .SetPriority(CacheItemPriority.NeverRemove)
                .AddExpirationToken(cacheExpirationToken)
                .RegisterPostEvictionCallback(callback: RegisterCache, state: this);
        }

        private async void RegisterCache(object key, object value, EvictionReason reason, object state)
        {
            IEnumerable<ProductDomainModel> orderDomainModels = await _productsRepository.GetAllProductsAsync();
            _memoryCache.Set(key, orderDomainModels, GetMemoryCacheEntryOptions());
        }
    }
}
