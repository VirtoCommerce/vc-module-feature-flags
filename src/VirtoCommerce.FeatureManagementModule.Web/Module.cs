using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using VirtoCommerce.FeatureManagementModule.Core;
using VirtoCommerce.FeatureManagementModule.Core.Services;
using VirtoCommerce.FeatureManagementModule.Data.Services;
using VirtoCommerce.FeatureManagementModule.Web.FeatureFilters;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.FeatureManagementModule.Web
{
    public class Module : IModule, IHasConfiguration
    {
        public ManifestModuleInfo ModuleInfo { get; set; }
        public IConfiguration Configuration { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.AddFeatureManagement().AddFeatureFilter<DevelopersFilter>();
            serviceCollection.AddSingleton<PlatformFeatureDefinitionProvider>();
            serviceCollection.AddSingleton<IFeatureStorage>(s => s.GetService<PlatformFeatureDefinitionProvider>());
            serviceCollection.AddSingleton<IFeatureDefinitionProvider>(s => s.GetService<PlatformFeatureDefinitionProvider>());
            serviceCollection.AddSingleton<Core.Services.IFeatureManager, FeatureManager>();
            serviceCollection.AddSingleton<Func<IUserNameResolver>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IUserNameResolver>());

        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            // register settings
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

            // register permissions
            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x =>
                new Permission()
                {
                    GroupName = "FeatureManagement",
                    ModuleId = ModuleInfo.Id,
                    Name = x
                }).ToArray());

            // register features with highest priority
            var featureStorage = (PlatformFeatureDefinitionProvider)appBuilder.ApplicationServices.GetService<IFeatureStorage>();
            var featuresSection = Configuration.GetSection("FeatureFlags");
            featureStorage.AddHighPriorityFeatureDefinition(featuresSection);

        }

        public void Uninstall()
        {
            // do nothing in here
        }
    }
}
