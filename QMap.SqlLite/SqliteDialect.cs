using QMap.Core.Dialects;
using System.Data;
using Microsoft.Data.Sqlite;

namespace QMap.Sqlite
{
    public class SqliteDialect : SqlDialectBase
    {
        public SqliteDialect()
        {
            _mappingTypes.Add(typeof(bool));
            _mappingTypes.Add(typeof(Boolean));
        }

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

        public override string? Map(object? obj)
        {
            try
            {
                return base.Map(obj);
            }
            catch (InvalidOperationException ex)
            {
                return obj switch
                {
                    bool boolObj => (boolObj ? 1 : 0).ToString(),
                    _ => throw new InvalidOperationException()
                };
            }

        }

        public override IDbDataParameter BuildParameter(ref IDbCommand dbCommand, string name, object value, params object[] options)
        {
            var parameter = ((SqliteCommand)dbCommand).CreateParameter();

            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        public override IDbCommand BuildParameters(IDbCommand dbCommand, Dictionary<string, object> namedParameters)
        {
            IDbCommand parametrizedCommand = (SqliteCommand)dbCommand;

            foreach (var parameterName in namedParameters.Keys)
            {
                var parameter = BuildParameter(ref dbCommand, parameterName, namedParameters[parameterName]);

                parameter = AssignValueWithType(ref parameter);

                parametrizedCommand.Parameters.Add(parameter);
            }

            return parametrizedCommand;
        }

        protected override SqliteParameter AssignValueWithType(ref IDbDataParameter parameter)
        {
            var sqlParam = ((SqliteParameter)parameter);

            var typedParam = sqlParam.Value switch
            {
                DateTime dateTimeObj => sqlParam.SqliteType = SqliteType.Text,
                string strObject => sqlParam.SqliteType = SqliteType.Text,
                Int16 => sqlParam.SqliteType = SqliteType.Integer,
                Int32 => sqlParam.SqliteType = SqliteType.Integer,
                Int64 => sqlParam.SqliteType = SqliteType.Integer,
                Decimal => sqlParam.SqliteType = SqliteType.Real,
                float => sqlParam.SqliteType = SqliteType.Real,
                double => sqlParam.SqliteType = SqliteType.Real,
                Guid => sqlParam.SqliteType = SqliteType.Text,
                byte => sqlParam.SqliteType = SqliteType.Blob,
                byte[] => sqlParam.SqliteType = SqliteType.Blob,
                bool => sqlParam.SqliteType = SqliteType.Text,
                _ => throw new NotImplementedException()
            };

            sqlParam.SqliteType = typedParam;

            return sqlParam;
        }

    }
}
