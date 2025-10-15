using System.Data;

namespace QMap.Mapping
{
    internal static class IDataReaderExtensions
    {
        //https://github.com/podkolzzzin/ORM.Stream/blob/master/ORM.Stream/DataReaderExtensions.cs#L7C12-L7C13
        private static bool TryGetOrdinal(this IDataReader reader, string column, out int order)
        {
            order = -1;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == column)
                {
                    order = i;
                    return true;
                }
            }
            return false;
        }

        public static T GetFromColumn<T>(this IDataReader dataReader, string columnName)
        {
            if (dataReader.TryGetOrdinal(columnName, out int order))
            {
                var value = dataReader.GetValue(order);

                //TODO: replace with Convert.ChangeType() expression in EntityMapper
                return (T)(value);
            }
            else
            {
                throw new InvalidOperationException($"Column {columnName} has not mathing in property type {typeof(T)}");
            }
        }

        public static object GetFromColumn(this IDataReader dataReader, Type type, string columnName)
        {
            if (dataReader.TryGetOrdinal(columnName, out int order))
            {
                return dataReader.GetValue(order);
            }
            else
            {
                throw new InvalidOperationException($"Culumn has not mathing to type {type.FullName}");
            }
        }
    }
}