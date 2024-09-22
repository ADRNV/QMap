namespace QMap.SqlBuilder.Dialects
{
    public interface ISqlDialect
    {
        public string ParameterName { get; set; }

        public string Quotes { get; set; }

        public Dictionary<string, string> Constants { get; }
    }
}
