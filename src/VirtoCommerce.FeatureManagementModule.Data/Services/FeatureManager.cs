using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.FeatureManagementModule.Core.Services;
using original = Microsoft.FeatureManagement;

namespace VirtoCommerce.FeatureManagementModule.Data.Services
{
    public class FeatureManager : IFeatureManager
    {
        private readonly original.IFeatureManager _featureManager;

        public FeatureManager(original.IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        public virtual IAsyncEnumerable<string> GetFeatureNamesAsync()
        {
            return _featureManager.GetFeatureNamesAsync();
        }

        public virtual async Task<bool> IsEnabledAsync(string feature)
        {
            return await _featureManager.IsEnabledAsync(feature);
        }

        public virtual async Task<bool> IsEnabledAsync<TContext>(string feature, TContext context)
        {
            return await _featureManager.IsEnabledAsync(feature, context);
        }
    }
}
