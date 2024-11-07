
namespace QMap.Core.Dialects
{
    public class SqlDialectBase : ISqlDialect
    {
        public string ParameterName { get => "@"; set => throw new NotImplementedException(); }
        public string Quotes { get => "'"; set => throw new NotImplementedException(); }

        protected List<Type> _mappingTypes = new()
        {
            typeof(string),
            typeof(DateTime)
        };

        public virtual string? Map(object? obj)
        {
            return obj switch
            {
                DateTime dateTimeObj => $"{dateTimeObj.Year}{dateTimeObj.Month}{dateTimeObj.Month}",
                string strObject => $"{Quotes}{strObject}{Quotes}",
                null => null,
                _ => throw new InvalidOperationException()
            };
        }

        public bool RequireMapping(object obj)
        {
            var t = obj.GetType();
            return _mappingTypes.Contains(t);
        }
    }
}
