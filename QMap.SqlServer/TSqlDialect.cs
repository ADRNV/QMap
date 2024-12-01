using QMap.Core.Dialects;
using System.Data;
using System.Data.SqlClient;

namespace QMap.SqlServer
{
    public class TSqlDialect : SqlDialectBase
    {
        public TSqlDialect()
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
            catch(InvalidOperationException ex)
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
            var parameter = ((SqlCommand)dbCommand).CreateParameter();

            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        public override IDbCommand BuildParameters(IDbCommand dbCommand, Dictionary<string, object> namedParameters)
        {
            IDbCommand parametrizedCommand = (SqlCommand)dbCommand;

            foreach (var parameterName in namedParameters.Keys)
            {
                var parameter = BuildParameter(ref dbCommand, parameterName, namedParameters[parameterName]);

                parameter = AssignValueWithType(ref parameter);

                parametrizedCommand.Parameters.Add(parameter);
            }

            return parametrizedCommand;
        }

        protected override SqlParameter AssignValueWithType(ref IDbDataParameter parameter)
        {
            var sqlParam = ((SqlParameter)parameter);

            var typedParam = sqlParam.Value switch
            {
                DateTime dateTimeObj => sqlParam.SqlDbType = SqlDbType.DateTime,
                string strObject => sqlParam.SqlDbType = SqlDbType.VarChar,
                Int16 => sqlParam.SqlDbType = SqlDbType.SmallInt,
                Int32 => sqlParam.SqlDbType = SqlDbType.Int,
                Int64 => sqlParam.SqlDbType = SqlDbType.BigInt,
                Decimal => sqlParam.SqlDbType = SqlDbType.Decimal,
                float => sqlParam.SqlDbType = SqlDbType.Float,
                double => sqlParam.SqlDbType = SqlDbType.Real,
                Guid => sqlParam.SqlDbType = SqlDbType.UniqueIdentifier,
                byte => sqlParam.SqlDbType = SqlDbType.VarBinary,
                byte[] => sqlParam.SqlDbType = SqlDbType.VarBinary,
                bool => sqlParam.SqlDbType = SqlDbType.Bit,
                _ => throw new NotImplementedException()
            };

            sqlParam.SqlDbType = typedParam;

            return sqlParam;
        }

    }
}
