using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/*http://blogs.msdn.com/b/pfxteam/archive/2012/03/04/10277325.aspx
 * http://blogs.msdn.com/b/pfxteam/archive/2012/03/05/10278165.aspx
 */
namespace NB.Core.Web.Extensions
{
    static class Extensions
    {
        public static async Task<IEnumerable<O>> ForEachAsync<T,O>(this IEnumerable<T> source, Func<T, Task<O>> body)
        {
            List<Exception> exceptions = null;
            var results = new List<O>();
            foreach (var item in source)
            {
                try { 
                var result =  await body(item);
                    results.Add(result);                
                }
                catch (Exception exc)
                {
                    if (exceptions == null) exceptions = new List<Exception>();
                    exceptions.Add(exc);
                }
            }
            if (exceptions != null)
                throw new AggregateException(exceptions);
            return results;
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
            var bag = new ConcurrentBag<O>();// BlockingCollection<O>();
            var exceptions = new ConcurrentBag<Exception>();
           
            await Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(dop)
                select Task.Run(async delegate
                {
                    try
                    {
                        using (partition)
                            while (partition.MoveNext())
                            {
                                var outObj = await body(partition.Current);
                                bag.Add(outObj);
                            }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }));

            if (exceptions.Count != 0)
                throw new AggregateException(exceptions);
            //if ( typeof(O).GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IComparable<>)))
             //if((typeof(O) != typeof(FileInfo)) &&
             // typeof(O).GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IComparable<>)))                
             //   list.Sort();
            return bag.ToList<O>();//await Task.FromResult<IEnumerable<O>>(list.ToList<O>());
        }      
    }
}
