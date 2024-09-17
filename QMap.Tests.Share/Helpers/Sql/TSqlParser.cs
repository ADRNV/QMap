using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace QMap.Tests.Share.Helpers.Sql
{
    public class TSqlParser : IParser
    {
        public object[] Options { get; private set; }

        public TSqlParser(params object[] options)
        {
            this.Options = options;
        }

        public TSqlParser()
        {

        }

        public List<string> Parse(string sql)
        {
            TSql100Parser parser = new TSql100Parser(false);

            IList<ParseError> errors;

            var d = parser.Parse(new StringReader(sql), out errors);

            if (errors != null && errors.Count > 0)
            {
                List<string> errorList = new List<string>();
                foreach (var error in errors)
                {
                    errorList.Add(error.Message);
                }
                return errorList;
            }

            return null;
        }
    }
}
