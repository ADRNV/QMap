using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace QMap.Mapping
{
    public class EntityMapper : EntityMapperBase
    {
        public ReadOnlyCollection<PropertyInfo>? ReflectedPropertyNames
        {
            get; 
            
            private set;
        }

        private Delegate MapDelegate;
        
        public override T Map<T>(IDataReader dataReader)
        {
            var typeInfo = typeof(T);

            if(ReflectedPropertyNames is null)
            {
                ReflectedPropertyNames = typeInfo.GetProperties(
                BindingFlags.Public
                | BindingFlags.GetProperty
                | BindingFlags.SetProperty
                | BindingFlags.Instance)
                    .AsReadOnly();
            }

            if(MapDelegate is null)
            {
                MapDelegate = (Func<IDataReader, T>)BuildMapExpression<T>(dataReader);
            }

            return ((Func<IDataReader, T>)(MapDelegate)).Invoke(dataReader);    
        }

        private Func<IDataReader, T> BuildMapExpression<T>(IDataReader dataReader)
        {
            var readerParam = Expression.Parameter(typeof(IDataReader));

            var newExp = Expression.New(typeof(T));
            var memberInit = Expression.MemberInit(newExp, typeof(T).GetProperties()
              .Select(x => Expression.Bind(x, BuildReadColumnExpression(readerParam, dataReader, x))));

            return Expression.Lambda<Func<IDataReader, T>>(memberInit, readerParam)
                .Compile();
        }

        private static Expression BuildReadColumnExpression(Expression readerExpression, IDataReader dataReader, PropertyInfo prop)
        {
            var method = typeof(IDataReaderExtensions)
                .GetMethods()
                .Where(m => m.Name == (nameof(IDataReaderExtensions.GetFromColumn)))
                .FirstOrDefault();

            return Expression.Call(type: typeof(IDataReaderExtensions),
               methodName: method.Name,
               typeArguments: new Type[] { prop.PropertyType },
               arguments: new Expression[] { Expression.Constant(dataReader), Expression.Constant(prop.Name) });
        }
    }
}
