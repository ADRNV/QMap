using System.Data;

namespace QMap.Core
{
    public abstract class QMapConnectionAdapterBase<C> : IQMapConnection where C: IDbConnection
    {
        protected C _connection;

        public QMapConnectionAdapterBase(C connection)
        {
            _connection = connection;
        }

        public virtual string ConnectionString 
        { 
            get => _connection.ConnectionString;
            
            set
            {
                _connection.ConnectionString = value;
            }
        }
        public virtual int ConnectionTimeout { get => _connection.ConnectionTimeout; }
        public virtual string Database { get => _connection.Database; }
        public virtual ConnectionState State { get => _connection.State; }

        public virtual IDbTransaction BeginTransaction()
        {
            return _connection.BeginTransaction();
        }
        public virtual IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _connection.BeginTransaction(il);
        }
        public virtual void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }
        public virtual void Close()
        {
            _connection.Close();
        }
        public virtual IDbCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }
        public abstract void Dispose();
        public virtual void Open()
        {
            _connection.Open();
        }
    }
}