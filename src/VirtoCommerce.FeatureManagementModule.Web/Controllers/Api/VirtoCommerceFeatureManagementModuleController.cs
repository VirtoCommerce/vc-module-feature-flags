using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.FeatureManagementModule.Core;


namespace VirtoCommerce.FeatureManagementModule.Web.Controllers.Api
{
    [Route("api/VirtoCommerceFeatureManagementModule")]
    public class VirtoCommerceFeatureManagementModuleController : Controller
    {
        // GET: api/VirtoCommerceFeatureManagementModule
        /// <summary>
        /// Get message
        /// </summary>
        /// <remarks>Return "Hello world!" message</remarks>
        [HttpGet]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public ActionResult<string> Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
