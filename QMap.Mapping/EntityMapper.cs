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

            if (ReflectedPropertyNames is null)
            {
                ReflectedPropertyNames = typeInfo.GetProperties(
                BindingFlags.Public
                | BindingFlags.GetProperty
                | BindingFlags.SetProperty
                | BindingFlags.Instance)
                    .AsReadOnly();
            }

            IsMatchToTable(dataReader, ReflectedPropertyNames);

            //if (MapDelegate is null)
            //{
            //    MapDelegate = 
            //}
            //return ((Func<IDataReader, T>)BuildMapExpression<T>(dataReader)).Invoke(dataReader);
            return ((Func<IDataReader, T>)BuildMapExpression<T>(dataReader)).Invoke(dataReader);
        }

        private Func<IDataReader, T> BuildMapExpression<T>(IDataReader dataReader)
        {
            var readerParam = Expression.Parameter(typeof(IDataReader));

            var newExp = Expression.New(typeof(T));
            var memberInit = Expression.MemberInit(newExp, typeof(T).GetProperties()
              //unnecessary type converting ?
              .Select(x => Expression.Bind(x, Expression.Convert(BuildReadColumnExpression(readerParam, dataReader, x), x.PropertyType)))
              );

            return Expression.Lambda<Func<IDataReader, T>>(memberInit, readerParam)
                .Compile();
        }

        private static Expression BuildReadColumnExpression(Expression readerExpression, IDataReader dataReader, PropertyInfo prop)
        {
            var method = typeof(IDataReaderExtensions)
                .GetMethods()
                .Where(m => m.Name == (nameof(IDataReaderExtensions.GetFromColumn)))
                .FirstOrDefault();

            var value = dataReader.GetValue(dataReader.GetOrdinal(prop.Name));

            var valueType = value.GetType();

            if (valueType != prop.PropertyType) 
            { 
                var type = typeof(Convert);

                var changeTypeMethod = type.GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) });

                var parseExpression = Expression.Call(
                    changeTypeMethod,
                    Expression.Constant(value, typeof(object)),
                    Expression.Constant(prop.PropertyType)
                );

                return parseExpression;                         
            }
            else
            {
                return Expression.Call(type: typeof(IDataReaderExtensions),
              methodName: method.Name,
              typeArguments: new Type[] { prop.PropertyType },
              arguments: new Expression[] { readerExpression, Expression.Constant(prop.Name) });
            }  
        }
    }
}
