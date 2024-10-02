namespace QMap.Core.Dialects
{
    public interface ISqlDialect
    {
        public string ParameterName { get; }

        public string Quotes { get; }

        public Dictionary<string, string> Constants { get; }
    }
}
