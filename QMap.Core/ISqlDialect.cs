namespace QMap.Core.Dialects
{
    public interface ISqlDialect
    {
        public string ParameterName { get; }

        public string Quotes { get; }

        string? Map(object? obj);

        bool RequireMapping(object? obj);
    }
}
