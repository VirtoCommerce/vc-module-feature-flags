namespace VirtoCommerce.FeatureManagementModule.Core.Services
{
    public interface IFeatureStorage
    {
        void TryAddFeatureDefinition(string featureName, string enabledFor);

        void TryAddFeatureDefinition(string featureName, bool isEnabled);

    }
}
