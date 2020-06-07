using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Interfaces
{
    public interface IUriComposer
    {
        string ComposePictureUri(string uriTemplate);
    }
}
