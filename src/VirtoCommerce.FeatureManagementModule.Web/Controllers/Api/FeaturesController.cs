using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.FeatureManagementModule.Core.Services;

namespace VirtoCommerce.FeatureManagementModule.Web.Controllers.Api
{
    [Route("api/features")]
    [ApiController]
    public class FeaturesController : Controller
    {
        private readonly IFeatureManager _featureManager;

        public FeaturesController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet]
        [Route("{featureName}")]
        public async Task<ActionResult<bool>> IsFeatureEnabled(string featureName)
        {
            return Ok(await _featureManager.IsEnabledAsync(featureName));
        }

        [HttpGet]
        public ActionResult<IAsyncEnumerable<string>> GetFeatureNames()
        {
            return Ok(_featureManager.GetFeatureNamesAsync());
        }
    }
}
