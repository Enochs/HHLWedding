using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHLWedding.ToolsLibrary
{
    public class PMSParameters
    {
        //查询类型
        public NSqlTypes Type
        {
            get;
            set;
        }
        //字段
        public string Name
        {
            get;
            set;
        }
        //值范围
        public object Value
        {
            get;
            set;
        }
        //本字段是否需要查询下级
        public bool IsContainsManagedEmployee
        {
            get;
            set;
        }


    }


    //查询条件逻辑类型
    public enum NSqlTypes
    {
        AndLike,        //模糊查询
        OrsLike,        //模糊查询
        LIKE,//模糊查询
        OR,//一般性OR查询字符串OR
        NumGreaterthan,//大于某个数字
        Greaterthan,//大于小于之间
        NumLessThan,//小于某个数字
        NumIn,//数字OR
        DateBetween,//Detatime类型 范围 数字 范围 
        OrDateBetween,
        NumBetween,//范围 数字 范围 
        IN,
        OrIn,
        Equal,
        Bit,
        NotIN,
        StringNotIN,    //字符串 包含
        StringEquals,//字符串等于
        IsNull,
        ORLike,
        IsNotNull,
        OrNotEquals,
        ColumnOr,
        NotEquals,
        OrInt,
        LessThan,        //小于      
        OrInts,
        DateEquals,
    }

    public enum OrderType
    {
        Desc,
        Asc
    }
}
