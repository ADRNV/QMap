using System.Data;

namespace QMap.Core.Dialects
{
    public interface ISqlDialect
    {
        public string ParameterName { get; }

        public string Quotes { get; }

        string? Map(object? obj);

        bool RequireMapping(object? obj);

        /// <summary>
        /// Common parameters buiding for <see cref="IDbCommand"/>
        /// </summary>
        /// <param name="dbCommand">Command</param>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Value of parameter</param>
        /// <param name="options">Additional options for creating parameters</param>
        /// <returns>Command with parameter</returns>
        IDbDataParameter BuildParameter(ref IDbCommand dbCommand, string name, object value, params object[] options);

        /// <summary>
        /// Adds parameters to command
        /// </summary>
        /// <param name="dbCommand">Command</param>
        /// <param name="namedParameters">Dictionary of parameters</param>
        /// <returns>Parametrized command</returns>
        IDbCommand BuildParameters(IDbCommand dbCommand, Dictionary<string, object> namedParameters);
    }
}
