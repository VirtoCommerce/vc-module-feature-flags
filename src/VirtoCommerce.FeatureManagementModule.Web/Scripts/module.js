// Call this to register your module to main application
var moduleName = "virtoCommerce.featureManagementModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config([function () {
        }
    ])
    .run([function () {
        }
    ]);
