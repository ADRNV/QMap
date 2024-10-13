using QMap.Core.Mapping;
using System.Data;

namespace QMap.Mapping.Enumerable
{
    internal class ReaderEnumerator<T> : IEnumerator<T> where T : class, new()
    {
        private T? _current;

        private bool _canMoveNext;

        private IDataReader _dataReader;

        private IEntityMapper _entityMapper;

        public ReaderEnumerator(IDataReader dataReader, IEntityMapper entityMapper)
        {
            _dataReader = dataReader;

            _entityMapper = entityMapper;
        }

        public object Current => _current;

        T IEnumerator<T>.Current => _current;

        public void Dispose()
        {
            if (_canMoveNext)
            {
                _dataReader.Dispose();
            }
        }

        public bool MoveNext()
        {
            var canReadNext = _dataReader.Read();

            _canMoveNext = canReadNext;

            if (canReadNext)
            {
                _current = _entityMapper.Map<T>(_dataReader);
            }
            else
            {
                return false;
            }

            return canReadNext;
        }

        public void Reset()
        {

        }
    }
}
