using System.Collections;

namespace QMap.Mapping.Enumerable
{
    internal class EnumerableReader<T> : IEnumerable<T> where T : class, new()
    {
        private ReaderEnumerator<T> _readerEnumerator;

        public EnumerableReader(ReaderEnumerator<T> readerEnumerator)
        {
            _readerEnumerator = readerEnumerator;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _readerEnumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _readerEnumerator;
        }
    }
}
