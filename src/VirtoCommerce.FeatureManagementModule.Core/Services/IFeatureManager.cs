using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.FeatureManagementModule.Core.Services
{
    public interface IFeatureManager
    {
        IAsyncEnumerable<string> GetFeatureNamesAsync();

        Task<bool> IsEnabledAsync(string feature);

        Task<bool> IsEnabledAsync<TContext>(string feature, TContext context);
    }
}
