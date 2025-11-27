using Microsoft.EntityFrameworkCore;
using QMap.Core;
using QMap.Tests.Share.Common.DataBase;

namespace QMap.Tests.Share.Common
{
    public interface IQMapConnectionFactory
    {
        IQMapConnection Create();

        T GetDbContext<T>() where T : BaseDbContext;
    }
}
