
namespace QMap.Core.Dialects
{
    public class SqlDialectBase : ISqlDialect
    {
        public string ParameterName { get => "@"; set => throw new NotImplementedException(); }
        public string Quotes { get => "'"; set => throw new NotImplementedException(); }

        public Dictionary<string, string> Constants => new Dictionary<string, string>() 
        { 
            {"True", "true" },
            {"False", "false" }
        };
    }
}
