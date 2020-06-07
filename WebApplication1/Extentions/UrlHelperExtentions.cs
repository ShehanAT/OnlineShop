using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/* extentions methdods can be used as addons for interfaces */
namespace Microsoft.WebApplication1.Extentions
{
    public static class UrlHelperExtentions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string verificationCode, string scheme)
        {
            return urlHelper.Action(
                action: "GET",
                controller: "ConfirmEmail",
                values: new { userId, verificationCode},
                protocol: scheme
                );
        }
    }
}
