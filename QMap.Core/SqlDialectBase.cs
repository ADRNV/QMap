
using System.Data;

namespace QMap.Core.Dialects
{
    public class SqlDialectBase : ISqlDialect
    {
        public string ParameterName { get => "@"; set => throw new NotImplementedException(); }
        public string Quotes { get => "'"; set => throw new NotImplementedException(); }

        IQMapConnection connection { get; set; }

        protected List<Type> _mappingTypes = new()
        {
            typeof(string),
            typeof(DateTime)
        };

        public virtual string? Map(object? obj)
        {
            return obj switch
            {
                DateTime dateTimeObj => $"{dateTimeObj.Year}{dateTimeObj.Month}{dateTimeObj.Month}",
                string strObject => $"{Quotes}{strObject}{Quotes}",
                null => null,
                _ => throw new InvalidOperationException()
            };
        }

        public bool RequireMapping(object obj)
        {
            var t = obj.GetType();
            return _mappingTypes.Contains(t);
        }

        public virtual IDbDataParameter BuildParameter(ref IDbCommand dbCommand, string name, object value, params object[] options)
        {
            var parameter = dbCommand.CreateParameter();

            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        public virtual IDbCommand BuildParameters(IDbCommand dbCommand, Dictionary<string, object> namedParameters)
        {
            IDbCommand parametrizedCommand = dbCommand;

            foreach (var parameterName in namedParameters.Keys)
            {
                var parameter = BuildParameter(ref dbCommand, parameterName, null);

                parameter = AssignValueWithType(ref parameter);
            }

            return dbCommand;
        }

        /// <summary>
        /// Base parameters type to DBType mapping
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual IDbDataParameter AssignValueWithType(ref IDbDataParameter parameter)
        {
            var typedParam = parameter.Value switch
            {
                DateTime dateTimeObj => parameter.DbType = DbType.DateTime,
                string strObject => parameter.DbType = DbType.String,
                Int16 => parameter.DbType = DbType.Int16,
                Int32 => parameter.DbType = DbType.Int32,
                Int64 => parameter.DbType = DbType.Int64,
                Decimal => parameter.DbType = DbType.Decimal,
                float => parameter.DbType = DbType.Double,
                double => parameter.DbType = DbType.Double,
                Guid => parameter.DbType = DbType.Guid,
                _ => throw new NotImplementedException()
            };

            parameter.DbType = typedParam;

            return parameter;
        }
    }
}
