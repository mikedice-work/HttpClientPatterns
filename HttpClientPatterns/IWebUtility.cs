using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientPatterns
{
    public interface IWebUtility
    {
        Task<string> GetResourceAsync(string url);
    }
}
