using System.Data;

namespace QMap.Core.Command
{
    public abstract class QMapCommandAdapterBase<T> : IQMapCommand where T : IDbCommand
    {
        protected T _command;

        public QMapCommandAdapterBase(T command)
        {
            _command = command;
        }

        public virtual string CommandText
        {
            get => _command.CommandText;
            set => _command.CommandText = value;
        }
        public virtual int CommandTimeout
        {

            get => _command.CommandTimeout;

            set
            {
                _command.CommandTimeout = value;
            }
        }
        public virtual CommandType CommandType
        {
            get => _command.CommandType;

            set
            {
                _command.CommandType = value;
            }
        }
        public virtual IDbConnection? Connection
        {
            get => _command.Connection;

            set
            {
                _command.Connection = value;
            }
        }

        public virtual IDataParameterCollection Parameters
        {
            get => _command.Parameters;
        }
        public virtual IDbTransaction? Transaction
        {
            get => _command.Transaction;

            set
            {
                _command.Transaction = value;
            }
        }
        public virtual UpdateRowSource UpdatedRowSource
        {
            get => _command.UpdatedRowSource;

            set
            {
                _command.UpdatedRowSource = value;
            }
        }

        public virtual void Cancel()
        {
            _command.Cancel();
        }

        public virtual IDbDataParameter CreateParameter()
        {
            return _command.CreateParameter();
        }

        public virtual void Dispose()
        {
            _command.Dispose();
        }

        public virtual int ExecuteNonQuery()
        {
            return _command.ExecuteNonQuery();
        }

        public abstract Task<IDataReader> ExecuteReader(CancellationToken cancellationToken);
        public virtual IDataReader ExecuteReader()
        {
            return _command.ExecuteReader();
        }
        public virtual IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return _command.ExecuteReader(behavior);
        }
        public abstract Task<IDataReader> ExecuteReaderAsync(CancellationToken cancellationToken);
        public virtual object? ExecuteScalar()
        {
            return _command.ExecuteScalar();
        }
        public virtual void Prepare()
        {
            _command.Prepare();
        }
    }
}
