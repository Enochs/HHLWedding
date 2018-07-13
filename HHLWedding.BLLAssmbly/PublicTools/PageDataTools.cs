using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHLWedding.BLLAssmbly
{
    public class PageDataTools<T> where T:new()
    {
        [Obsolete]
        /// <summary>
        /// 添加空白
        /// </summary>
        /// <param name="ObjDataList"></param>
        /// <returns></returns>
        public static List<T> AddtoPageSize(List<T> ObjDataList)
        {
            return ObjDataList;
            //if (ObjDataList.Count == 20)
            //{
            //    return ObjDataList;
            //}
            //else
            //{
            //    int NeedSize = 20;
            //    int SourceCount=ObjDataList.Count;
            //    for (int i = 0; i < NeedSize - SourceCount; i++)
            //    {

            //        ObjDataList.Add(new T());
            //    }
            //    return ObjDataList;

            //}
        }

        /// <summary>
        /// 给指定集合分页。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="SourceCount"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetPagedData(IEnumerable<T> collection, int PageSize, int PageIndex)
        {
            return collection.Skip(PageSize * (PageIndex - 1)).Take(PageSize);
        }

    }
}
