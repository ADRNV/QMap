namespace QMap.SqlBuilder.Abstractions
{
    public interface IInsertBuilder : IQueryBuilder
    {
        IInsertBuilder BuildInsert<T>(T entity);

    }
}
