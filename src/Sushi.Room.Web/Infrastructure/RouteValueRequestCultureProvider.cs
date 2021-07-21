using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sushi.Room.Web.Infrastructure
{
    public class RouteValueRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            string cultureCode = null;

            if (httpContext.Request.Path.HasValue && httpContext.Request.Path.Value == "/")
                cultureCode = this.GetDefaultCultureCode();

            else if (httpContext.Request.Path.Value != null && (httpContext.Request.Path.HasValue && httpContext.Request.Path.Value.Length >= 4 && httpContext.Request.Path.Value[0] == '/' && httpContext.Request.Path.Value[3] == '/'))
            {
                cultureCode = httpContext.Request.Path.Value.Substring(1, 2);

                if (!this.CheckCultureCode(cultureCode))
                    cultureCode = this.GetDefaultCultureCode();
            }

            else cultureCode = this.GetDefaultCultureCode();

            var requestCulture = new ProviderCultureResult(cultureCode);

            return Task.FromResult(requestCulture);
        }

        private string GetDefaultCultureCode()
        {
            return this.Options.DefaultRequestCulture.Culture.TwoLetterISOLanguageName;
        }

        private bool CheckCultureCode(string cultureCode)
        {
            return this.Options.SupportedCultures.Select(c => c.TwoLetterISOLanguageName).Contains(cultureCode);
        }
    }
}
