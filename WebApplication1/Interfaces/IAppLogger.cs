using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Interfaces
{
    public interface IAppLogger<T>
    { /* the <T> indicates the interface is a generic type*/
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);

    }
}
