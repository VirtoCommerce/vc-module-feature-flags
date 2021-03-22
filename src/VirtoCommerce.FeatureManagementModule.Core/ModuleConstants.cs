using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.FeatureManagementModule.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public const string Access = "virtoCommerceFeatureManagementModule:access";
                public const string Create = "virtoCommerceFeatureManagementModule:create";
                public const string Read = "virtoCommerceFeatureManagementModule:read";
                public const string Update = "virtoCommerceFeatureManagementModule:update";
                public const string Delete = "virtoCommerceFeatureManagementModule:delete";

                public static string[] AllPermissions { get; } = { Read, Create, Access, Update, Delete };
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static SettingDescriptor VirtoCommerceFeatureManagementModuleEnabled { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerceFeatureManagementModule.VirtoCommerceFeatureManagementModuleEnabled",
                    GroupName = "VirtoCommerceFeatureManagementModule|General",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false
                };

                public static SettingDescriptor VirtoCommerceFeatureManagementModulePassword { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerceFeatureManagementModule.VirtoCommerceFeatureManagementModulePassword",
                    GroupName = "VirtoCommerceFeatureManagementModule|Advanced",
                    ValueType = SettingValueType.SecureString,
                    DefaultValue = "qwerty"
                };

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return VirtoCommerceFeatureManagementModuleEnabled;
                        yield return VirtoCommerceFeatureManagementModulePassword;
                    }
                }
            }

            public static IEnumerable<SettingDescriptor> AllSettings
            {
                get
                {
                    return General.AllSettings;
                }
            }
        }
    }
}
