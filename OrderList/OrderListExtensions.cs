using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderList
{
    public static class OrderListExtensions
    {
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> handler)
        {
            foreach (T item in enumerable)
                handler(item);
        }
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
        {
            int idx = 0;
            foreach (T item in enumerable)
                handler(item, idx++);
        }

        public static void EachRecursive<T>(this IEnumerable<T> source, Action<T, int> handler)
        {
            var tSource = source.ToList();
            var typeSource = tSource.GetType();
            int idx = 0;
            foreach (var item in tSource)
            {
                var thereIsNetedList = item.GetType().GetProperties().FirstOrDefault(p => p.PropertyType.Equals(typeof(IList<T>)));
                handler(item, idx++);
                if (thereIsNetedList == null) continue;
                var valuesNesteds = thereIsNetedList.GetValue(item) as IList<T>;
                valuesNesteds.EachRecursive(handler);
            }
        }
        public static bool InRange(this int source, int start, int end)
        {
            return source >= start & source <= end;
        }
        public static void Move<T>(this IList<T> source, int oldIndex, int newIndex, Action<T, int> handler = null)
        {
            var count = source.Count;
            if (!oldIndex.InRange(0, count) || !newIndex.InRange(0, count))
                throw new IndexOutOfRangeException();
            if (oldIndex == newIndex) return;

            var item = source.ElementAt(oldIndex);
            source.RemoveAt(oldIndex);
            if (newIndex > oldIndex) newIndex--;
            source.Insert(newIndex, item);
            if (handler != null)
                source.Each(handler);
        }

        public static List<TSource> ToFullList<TSource>(this IEnumerable<TSource> source)
        {
            var tSource = source.ToList();
            if (!tSource.Any()) return tSource;

            var newSource = new List<TSource>();
            var typeSource = tSource.GetType().FullName;

            foreach (var item in tSource)
            {
                var newItem = item;
                var internalSource = newItem.GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(IList<TSource>));
                if (internalSource == null)
                    throw new ArgumentNullException("There is no property of type as list " + typeof(TSource).Name);
                newSource.Add(newItem);

                var internalValue = internalSource.GetValue(item) as IList<TSource>;
                if (internalValue == null || !internalValue.Any()) continue;

                newSource.AddRange(internalValue.ToFullList());
            }
            return newSource.ToList();
        }

        public static List<TSource> ToFitList<TSource>(this IEnumerable<TSource> source, object property = null)
        {
            var typeTSouce = typeof(TSource);

            var sourcelevel = source.Where(p => p.GetType().GetProperties().First(v => v.PropertyType.Equals(typeTSouce)).GetValue(p) == property).ToList();
            if (!sourcelevel.Any()) return sourcelevel;

            var newSource = new List<TSource>();
            newSource.AddRange(sourcelevel);

            foreach (var item in newSource)
            {
                var internalSource = item.GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(IList<TSource>));
                if (internalSource == null)
                    throw new ArgumentNullException("There is no property of type as list " + typeTSouce.Name);

                var internaList = source.ToFitList(item);
                if (internaList == null || !internaList.Any()) continue;

                internalSource.SetValue(item, internaList.ToList());
            }
            return newSource.ToList();
        }
    }
}
