using System.Linq.Expressions;

namespace QMap.Core.Mapping
{
    public interface ISqlBuilder
    {
        public FormattableString Build(Expression expression);

        public FormattableString Build<T>(Expression expression);
    }
}
