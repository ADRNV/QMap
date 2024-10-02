using QMap.Core.Dialects;

namespace QMap.SqlServer
{
    public class TSqlDialect : ISqlDialect
    {
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
        
    }
}
