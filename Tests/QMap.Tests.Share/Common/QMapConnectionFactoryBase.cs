using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QMap.Core;
using QMap.Tests.Share.Common;
using QMap.Tests.Share.Common.DataBase;
using System.Reflection;

namespace QMap.Tests.Share
{
    public abstract class QMapConnectionFactoryBase : IQMapConnectionFactory
    {
        protected readonly IConfiguration _configuration;

        private readonly DbContext _dbContext;

        private readonly DbContextOptions _options;

        public QMapConnectionFactoryBase(IConfiguration configuration, DbContextOptions options)
        {
            _options = options;

            _configuration = configuration;
        }

        public virtual T GetDbContext<T>() where T: BaseDbContext
        {
            var type = typeof(T);

            var ctor = type.GetConstructor(new[] { typeof(DbContextOptions) });

            var dbContextInstance = (T)ctor.Invoke(new object[] { _options });

            return dbContextInstance;
        }

        public abstract IQMapConnection Create();

    }
}
