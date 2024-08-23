using Microsoft.Extensions.Configuration;
using QMap.Core;

namespace QMap.Tests.Common
{
    public abstract class IQMapConnectionFactoryBase
    {
        protected readonly IConfiguration _configuration;

        public IQMapConnectionFactoryBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public abstract IQMapConnection Create();

    }
}
