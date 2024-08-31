namespace QMap.SqlBuilder
{
    /// <summary>
    /// Represents base visitor logic
    /// </summary>
    public interface IVisitor
    {
        IEnumerable<IVisitor> Visit();
    }
}
