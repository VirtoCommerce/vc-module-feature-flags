var moduleName = "virtoCommerce.featureManagementModule";

angular.module(moduleName)
    .factory('virtoCommerce.featureManagerSubscriber', ['$rootScope', 'virtoCommerce.featureManager', '$window', '$state', function ($rootScope, featureManager, $window, $state) {
        var result = {};

        result.callbacksGroupedByFeatureName = [];
        result.onLoginStatusChanged = (featureName, callback) => {
            if (!result.callbacksGroupedByFeatureName[featureName]) {
                result.callbacksGroupedByFeatureName[featureName] = [];
            }
            if (typeof callback === 'function') {
                result.callbacksGroupedByFeatureName[featureName].push(callback);
            }
        };

        function initialize() {
            //This function runs only on 'login'/'sign out' event, but the featureManager calls 'isFeatureEnabled' only at the time of login
            //and then features are registered only if they are available to the user,
            //when we sign out - we are just reloading
            $rootScope.$on('loginStatusChanged',
                (_, authContext) => {
                    if (!authContext.isAuthenticated) {
                        $window.location.reload();
                        return;
                    }

                    for (const [featureName, callbacks] of Object.entries(result.callbacksGroupedByFeatureName)) {
                        featureManager.isFeatureEnabled(featureName).then(() => {
                            angular.forEach(callbacks,
                                callback => {
                                    callback();
                                });
                        });
                    }

                    //We need this reload to make sure that our new toolbar commands are visible inside blade
                    $state.reload();
                });
        }

        initialize();

        return result;
    }]);
