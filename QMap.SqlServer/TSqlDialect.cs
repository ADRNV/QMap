using QMap.Core.Dialects;

namespace QMap.SqlServer
{
    public class TSqlDialect : SqlDialectBase
    {
        public TSqlDialect()
        {
            _mappingTypes.Add(typeof(bool));
            _mappingTypes.Add(typeof(Boolean));
        }

        public string ParameterName { get => "@"; }

        public string Quotes { get => "'"; }

        public Dictionary<string, string> Constants
        {
            get => new Dictionary<string, string>()
            {
                { "True", "1" },
                { "False", "0" },
            };
        }

        public override string? Map(object? obj)
        {
            try
            {
                return base.Map(obj);
            }
            catch (InvalidOperationException ex)
            {
                return obj switch
                {
                    bool boolObj => (boolObj ? 1 : 0).ToString(),
                    _ => throw new InvalidOperationException()
                };
            }

        }

    }
}
