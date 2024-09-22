using QMap.SqlBuilder.Dialects;

namespace QMap.SqlServer
{
    public class TSqlDialect : ISqlDialect
    {
        public string ParameterName { get => "@"; set => throw new NotImplementedException(); }
        public string Quotes { get => "'"; set => throw new NotImplementedException(); }

        private Dictionary<string, string> _constants = new Dictionary<string, string>()
        { 
            { "true", "1" },
            { "false", "0" },
        };
        public Dictionary<string, string> Constants => new Dictionary<string, string>();
    }
}
