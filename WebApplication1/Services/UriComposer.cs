using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Services
{
    public class UriComposer : IUriComposer
    {
        private readonly CatalogSettings _catalogSettings;
        public UriComposer(CatalogSettings catalogSettings)
        {
            this._catalogSettings = catalogSettings;
        }
        /*Below constructor is equivalent to above constructor
         * ```pubilc UriComposer(CatalogSettings catalogSettings) => _catalogSettings```
         * */

        public string ComposePictureUri(string picturURI)
        {
            return picturURI.Replace("http://catalogbaseurltobereplaced", _catalogSettings.CatalogBaseUrl);
        }


    }
}
