using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/*http://blogs.msdn.com/b/pfxteam/archive/2012/03/04/10277325.aspx
 * http://blogs.msdn.com/b/pfxteam/archive/2012/03/05/10278165.aspx
 */
namespace NB.Core.Web.Utility
{
    static class Extensions
    {
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body)
        {
            List<Exception> exceptions = null;
            foreach (var item in source)
            {
                try { await body(item); }
                catch (Exception exc)
                {
                    if (exceptions == null) exceptions = new List<Exception>();
                    exceptions.Add(exc);
                }
            }
            if (exceptions != null)
                throw new AggregateException(exceptions);
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body)
        {
            await Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(dop)
                select Task.Run(async delegate
                {
                    using (partition)
                    while (partition.MoveNext())
                        await body(partition.Current);
                }));
        }

        public static async Task<IEnumerable<O>> ForEachAsync<I,O>(this IEnumerable<I> source, int dop, Func<I, Task<O>> body)
        {
            var list = new List<O>();// BlockingCollection<O>();
            object sync = new object();
            await Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(dop)
                select Task.Run(async delegate
                {
                    using (partition)
                    while (partition.MoveNext())
                    {
                        var outObj =  await body(partition.Current);
                        lock (sync)
                        {
                            list.Add(outObj);
                        }
                    }
                }));
            //if ( typeof(O).GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IComparable<>)))
             if((typeof(O) != typeof(FileInfo)) &&
              typeof(O).GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IComparable<>)))                
                list.Sort();
            return list.ToList<O>();//await Task.FromResult<IEnumerable<O>>(list.ToList<O>());
        }      
    }
}
