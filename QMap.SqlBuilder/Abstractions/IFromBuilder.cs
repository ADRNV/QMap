namespace QMap.SqlBuilder.Abstractions
{
    public interface IFromBuilder : IQueryBuilder
    {
        public IFromBuilder BuildFrom(ISelectBuilder quryBuilder, Type entity, params Type[] entities);
    }
}
