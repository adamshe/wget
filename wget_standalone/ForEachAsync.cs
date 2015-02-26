using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/*http://blogs.msdn.com/b/pfxteam/archive/2012/03/04/10277325.aspx
 * http://blogs.msdn.com/b/pfxteam/archive/2012/03/05/10278165.aspx
 */
namespace wget
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

        //public static Task ForEachAsync<TSource, TResult>(
        // this IEnumerable<TSource> source,
        //    Func<TSource, Task<TResult>> taskSelector, 
        //    Action<TSource, TResult> resultProcessor)
        //{
        //    var oneAtATime = new SemaphoreSlim(initialCount: 1, maxCount: 1);
        //    return Task.WhenAll(
        //        from item in source
        //        select ProcessAsync(item, taskSelector, resultProcessor, oneAtATime));
        //}

        //private static async Task ProcessAsync<TSource, TResult>(
        //    TSource item,
        //    Func<TSource, Task<TResult>> taskSelector, Action<TSource, TResult> resultProcessor,
        //    SemaphoreSlim oneAtATime)
        //{
        //    TResult result = await taskSelector(item);
        //    await oneAtATime.WaitAsync();
        //    try { resultProcessor(item, result); }
        //    finally { oneAtATime.Release(); }
        //}

        public static Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body)
        {
            return Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(dop)
                select Task.Run(async delegate
                {
                    using (partition)
                        while (partition.MoveNext())
                            await body(partition.Current);
                }));
        }       
    }
}
