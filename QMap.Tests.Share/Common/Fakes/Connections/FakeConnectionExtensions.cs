using Moq;
using System.Data;
using System.Data.Common;

namespace QMap.Tests.Share.Common.Fakes.Connections
{
    public static class FakeConnectionExtensions
    {
        public static FakeQMapConnectionAdapter Create()
        {
            var fake = new Mock<DbConnection>();

            return new FakeQMapConnectionAdapter(fake.Object);
        }
    }
}
