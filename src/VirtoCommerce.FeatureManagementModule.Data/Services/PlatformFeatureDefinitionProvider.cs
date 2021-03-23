using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using VirtoCommerce.FeatureManagementModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.FeatureManagementModule.Data.Services
{
    public class PlatformFeatureDefinitionProvider : IFeatureDefinitionProvider, IFeatureStorage
    {
        private readonly ICollection<FeatureDefinition> _featureDefinitions;

        public PlatformFeatureDefinitionProvider()
        {
            _featureDefinitions = new List<FeatureDefinition>();
        }

        public virtual async Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
        {
            using (await AsyncLock.GetLockByKey(nameof(GetFeatureDefinitionAsync)).LockAsync())
            {
                var result = _featureDefinitions.FirstOrDefault(x => x.Name.EqualsInvariant(featureName));

                return result;
            }
        }

        public virtual IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
        {
            return _featureDefinitions.ToAsyncEnumerable();
        }

        public virtual void AddHighPriorityFeatureDefinition(IConfigurationSection configuration)
        {
            var featureConfigurations = configuration.GetChildren().ToList();

            foreach (var featureConfiguration in featureConfigurations)
            {
                var featureDefinition = new FeatureDefinition
                {
                    Name = featureConfiguration.Key,
                };
                if (!string.IsNullOrEmpty(featureConfiguration.Value) &&
                    bool.TryParse(featureConfiguration.Value, out var isEnabled))
                {
                    featureDefinition.EnabledFor = isEnabled ?
                        new[] { new FeatureFilterConfiguration { Name = "AlwaysOn", } } :
                        Array.Empty<FeatureFilterConfiguration>();
                }
                else
                {
                    featureDefinition.EnabledFor = new[] { new FeatureFilterConfiguration { Name = featureConfiguration.Value, } };
                }

                // Need to remove defined features from collection
                if (_featureDefinitions.Any(x => x.Name.EqualsInvariant(featureDefinition.Name)))
                {
                    var definedFeature = _featureDefinitions.First(x => x.Name.EqualsInvariant(featureDefinition.Name));

                    _featureDefinitions.Remove(definedFeature);
                }

                _featureDefinitions.Add(featureDefinition);
            }
        }

        public virtual void TryAddFeatureDefinition(string featureName, string enabledFor)
        {
            var featureDefinition = ConvertToFeatureDefinition(featureName, enabledFor);

            if (_featureDefinitions.Any(x => x.Name.EqualsInvariant(featureDefinition.Name)))
            {
                return;
            }

            _featureDefinitions.Add(featureDefinition);
        }

        public virtual void TryAddFeatureDefinition(string featureName, bool isEnabled)
        {
            var featureDefinition = ConvertToFeatureDefinition(featureName, isEnabled);

            if (_featureDefinitions.Any(x => x.Name.EqualsInvariant(featureDefinition.Name)))
            {
                return;
            }

            _featureDefinitions.Add(featureDefinition);
        }


        protected virtual FeatureDefinition ConvertToFeatureDefinition(string featureName, string enabledFor)
        {
            var result = new FeatureDefinition
            {
                Name = featureName,
                EnabledFor = new[]
                {
                    new FeatureFilterConfiguration
                    {
                        Name = enabledFor,
                    }
                }
            };

            return result;
        }

        protected virtual FeatureDefinition ConvertToFeatureDefinition(string featureName, bool isEnabled)
        {
            var result = new FeatureDefinition
            {
                Name = featureName,
                EnabledFor = isEnabled ?
                    new[] { new FeatureFilterConfiguration { Name = "AlwaysOn", } } :
                    Array.Empty<FeatureFilterConfiguration>()
            };

            return result;
        }
    }
}
