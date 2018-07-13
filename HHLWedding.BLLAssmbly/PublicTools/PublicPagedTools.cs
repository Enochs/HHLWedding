using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHLWedding.BLLAssmbly
{
    public class PublicPagedTools
    {
        /// <summary>
        /// 给指定集合分页。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="collection"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="SourceCount"></param>
        /// <returns></returns>
        public List<TSource> GetPagedData<TSource>(IEnumerable<TSource> collection, int PageSize, int PageIndex, out int SourceCount) where TSource : class,new()
        {
            SourceCount = collection.Count();
            List<TSource> result = collection.Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList();
            if (SourceCount == 0)
            {
                result = new List<TSource>();
            }
            return result;
        }
    }
}
