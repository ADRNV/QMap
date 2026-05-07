namespace QMap.Tests.Share.Helpers.Sql
{
    public interface IParser
    {
        object[] Options { get; }

        List<string> Parse(string sql);
    }
}
