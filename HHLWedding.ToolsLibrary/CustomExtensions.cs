using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace HHLWedding.ToolsLibrary
{
    public static class CustomExtensions
    {
        #region System.DateTime

        /// <summary>
        /// 将日期和时间的指定字符串表示形式转换为其 System.DateTime 等效项，转换失败返回指定日期。
        /// </summary>
        /// <param name="s">包含要转换的日期和时间的的字符串。</param>
        /// <returns>转换后的 System.DateTime 的等效项。</returns>
        public static System.DateTime ToDateTime(this System.String s, System.DateTime defaultDateTime)
        {
            if (!System.String.IsNullOrWhiteSpace(s))
            {
                System.DateTime result = defaultDateTime;
                if (System.DateTime.TryParse(s, out result))
                {
                    return result;
                }
            }
            return defaultDateTime;
        }

        /// <summary>
        /// 获取指定日期的起始时间。（将时间部分设置为最小 00：00：00）。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static System.DateTime Start(this System.DateTime s)
        {
            return new System.DateTime(s.Year, s.Month, s.Day, 0, 0, 0, 000);
        }

        /// <summary>
        /// 获取指定日期的结束时间。（将时间部分设置为最大 23：59：59）。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static System.DateTime End(this System.DateTime s)
        {
            return new System.DateTime(s.Year, s.Month, s.Day, 23, 59, 59, 999);
        }

        #endregion

        #region System.Reflection
        /// <summary>
        /// 调用对象的公共实例方法。
        /// </summary>
        /// <param name="obj">被调用方法的对象。</param>
        /// <param name="name">方法名。</param>
        /// <param name="parameters">方法的参数</param>
        public static void Invoke(this object obj, string name, System.Type[] array, object[] parameters)
        {
            obj.GetType().GetMethod(name, array).Invoke(obj, parameters);
        }

        /// <summary>
        /// 调用对象的方法
        /// </summary>
        /// <param name="obj">被调用方法的对象。</param>
        /// <param name="name">方法名。</param>
        /// <param name="parameters">方法的参数</param>
        public static void Invoke(this object obj, string name, object[] parameters)
        {
            obj.GetType().GetMethod(name).Invoke(obj, parameters);
        }

        /// <summary>
        /// 获取对象属性名为 name 的值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="name">属性名。</param>
        /// <returns>属性值</returns>
        public static object GetPropertyValue(this object obj, string name)
        {
            return obj.GetType().GetProperty(name).GetValue(obj);
        }

        /// <summary>
        /// 设置对象属性名为 name 的值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="name">属性名。</param>
        /// <param name="value">值。</param>
        /// <returns>属性值</returns>
        public static void SetPropertyValue(this object obj, string name, object value)
        {
            obj.GetType().GetProperty(name).SetValue(obj, value);
        }

        /// <summary>
        /// 获取对象字段名为 name 值。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="name">字段名。</param>
        /// <returns></returns>
        public static object GetFieldValue(this object obj, string name)
        {
            return obj.GetType().GetField(name).GetValue(obj);
        }

        /// <summary>
        /// 获取对象字段名为 name 值。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="name">字段名。</param>
        /// <param name="value">值。</param>
        /// <returns></returns>
        public static void SetFieldValue(this object obj, string name, object value)
        {
            obj.GetType().GetField(name).SetValue(obj, value);
        }

        /// <summary>
        /// 获取对象的所有属性。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns></returns>
        public static System.Reflection.PropertyInfo[] GetProperties(this object obj)
        {
            return obj.GetType().GetProperties();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public static System.Reflection.PropertyInfo[] GetProperties(this object obj, System.Reflection.BindingFlags bindingAttr)
        {
            return obj.GetType().GetProperties(bindingAttr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static System.Reflection.FieldInfo[] GetFields(this object obj)
        {
            return obj.GetType().GetFields();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public static System.Reflection.FieldInfo[] GetFields(this object obj, System.Reflection.BindingFlags bindingAttr)
        {
            return obj.GetType().GetFields(bindingAttr);
        }

        /// <summary>
        /// 获取与该对象类型关联的 GUID。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns></returns>
        public static System.Guid GUID(this object obj)
        {
            return obj.GetType().GUID;
        }

        #endregion

        #region System.Type

        /// <summary>
        /// 创建指定类型的实例。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <returns></returns>
        public static object CreateInstance(this System.Type type)
        {
            return System.Activator.CreateInstance(type);
        }

        /// <summary>
        /// 指示 type 类型是否为可空类型（Nullable<>）。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <returns></returns>
        public static bool IsNullableType(this System.Type type)
        {
            return (((type != null) && type.IsGenericType) &&
                (type.GetGenericTypeDefinition() == typeof(System.Nullable<>)));
        }

        /// <summary>
        /// 指示 type 类型是否为集合类型（IEnumerable<>）。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <returns></returns>
        public static bool IsEnumerableType(this System.Type type)
        {
            return (FindGenericType(typeof(System.Collections.Generic.IEnumerable<>), type) != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static System.Type GetNonNullableType(this System.Type type)
        {
            if (IsNullableType(type))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerableType"></param>
        /// <returns></returns>
        public static System.Type GetElementType(this System.Type enumerableType)
        {
            System.Type type = FindGenericType(typeof(System.Collections.Generic.IEnumerable<>), enumerableType);
            if (type != null)
            {
                return type.GetGenericArguments()[0];
            }
            return enumerableType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        public static bool IsKindOfGeneric(this System.Type type, System.Type definition)
        {
            return (FindGenericType(definition, type) != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static System.Type FindGenericType(this System.Type definition, System.Type type)
        {
            while ((type != null) && (type != typeof(object)))
            {
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == definition))
                {
                    return type;
                }
                if (definition.IsInterface)
                {
                    foreach (System.Type type2 in type.GetInterfaces())
                    {
                        System.Type type3 = FindGenericType(definition, type2);
                        if (type3 != null)
                        {
                            return type3;
                        }
                    }
                }
                type = type.BaseType;
            }
            return null;
        }

        #endregion

        #region System.Convert

        /// <summary>
        /// 返回 start 和 end 在 min 和 max 之间的上下限数组。
        /// </summary>
        /// <typeparam name="TSource">要比较的对象类型。</typeparam>
        /// <param name="start">待比较的上限。</param>
        /// <param name="end">待比较的下限。</param>
        /// <param name="min">比较范围的最小下限。</param>
        /// <param name="max">比较范围的最大上限。</param>
        /// <returns></returns>
        public static TSource[] CompareTo<TSource>(TSource start, TSource end, TSource min, TSource max) where TSource : struct
        {
            return new TSource[] 
            {
                typeof(TSource).GetMethod("CompareTo", new System.Type[] { typeof(TSource) }).Invoke(start, new object[] { min }).Equals(1) ? start : min, 
                typeof(TSource).GetMethod("CompareTo", new System.Type[] { typeof(TSource) }).Invoke(end, new object[] { max }).Equals(1) ? max : end 
            };
        }

        /// <summary>
        /// 返回 start 和 end 在 min 和 max 之间的上下限数组。
        /// </summary>
        /// <typeparam name="TSource">要比较的对象类型。</typeparam>
        /// <param name="start">待比较的上限。</param>
        /// <param name="end">待比较的下限。</param>
        /// <param name="min">比较范围的最小下限。</param>
        /// <param name="max">比较范围的最大上限。</param>
        /// <returns></returns>
        public static TSource[] CompareTo<TSource>(this object obj, TSource start, TSource end, TSource min, TSource max) where TSource : struct
        {
            return new TSource[] 
            {
                typeof(TSource).GetMethod("CompareTo", new System.Type[] { typeof(TSource) }).Invoke(start, new object[] { min }).Equals(1) ? start : min, 
                typeof(TSource).GetMethod("CompareTo", new System.Type[] { typeof(TSource) }).Invoke(end, new object[] { max }).Equals(1) ? max : end 
            };
        }

        [Obsolete]
        /// <summary>
        /// <para>转换为指定值类型对象，转换失败返回默认值。</para>
        /// <para>bool：null、0、false 转换为 false；1 等非 0 整数、小数、true 转换为 true。string.Empty，DateTime 转换失败。</para>
        /// <para>byte~int~decimal：null 转换为 0。超出该类型范围和转换异常将导致转换失败。</para>
        /// <para>string：null 转换为 string.Empty。都将转换成功，defaultValue 无效。</para>
        /// <para>DateTime：null 转换为 DateTime.MinValue。转换异常将导致转换失败。</para>
        /// <para>注：只支持能被 Convert.ToXXX(object obj) 构架转换的类型。暂不支持转换为 Guid 类型。</para>
        /// </summary>
        /// <typeparam name="TSource">将转换的值类型。</typeparam>
        /// <param name="obj">要转换的对象。</param>
        /// <param name="defaultValue">转换失败返回的默认值。</param>
        /// <returns></returns>
        public static TSource To<TSource>(this object obj, TSource defaultValue) where TSource : struct
        {
            try
            {
                return (TSource)typeof(System.Convert).GetMethod(string.Format("To{0}", typeof(TSource).Name), new System.Type[] { typeof(object) }).Invoke(null, new object[] { obj });
            }
            catch { return defaultValue; }
        }
        [Obsolete]
        /// <summary>
        /// <para>转换为指定值类型对象，转换失败抛出异常。</para>
        /// <para>bool：null、0、false 转换为 false；1 等非 0 整数、小数、true 转换为 true。string.Empty，DateTime 转换失败。</para>
        /// <para>byte~int~decimal：null 转换为 0。超出该类型范围和转换异常将导致转换失败。</para>
        /// <para>string：null 转换为 string.Empty。都将转换成功，defaultValue 无效。</para>
        /// <para>DateTime：null 转换为 DateTime.MinValue。转换异常将导致转换失败。</para>
        /// <para>注：只支持能被 Convert.ToXXX(object obj) 构架转换的类型。暂不支持转换为 Guid 类型。</para>
        /// </summary>
        /// <typeparam name="TSource">将转换的值类型。</typeparam>
        /// <param name="obj">要转换的对象。</param>
        /// <param name="defaultValue">转换失败返回的默认值。</param>
        /// <returns></returns>
        public static TSource To<TSource>(this object obj) where TSource : struct
        {
            return (TSource)typeof(System.Convert).GetMethod(string.Format("To{0}", typeof(TSource).Name), new System.Type[] { typeof(object) }).Invoke(null, new object[] { obj });
        }

        /// <summary>
        ///  将 GUID 的字符串表示形式转换为等效的 System.Guid 结构。转换失败返回 guid。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="guid">转换失败时返回的 GUID。</param>
        /// <returns></returns>
        public static System.Guid ToGuid(object obj, System.Guid guid)
        {
            if (!object.ReferenceEquals(obj, null))
            {
                System.Guid result;
                if (System.Guid.TryParse(obj.ToString(), out result))
                {
                    return result;
                }
            }
            return guid;
        }

        #endregion

        #region System.Data.Entity

        /// <summary>
        /// 只有 condition 为 true 时，才将参数转换成 ObjectParameter 对象添加到 System.Collection.Generic.List＜ObjectParameter＞ 的结尾处。
        /// </summary>
        /// <param name="list">参数列表。</param>
        /// <param name="condition">为 true 时，才将参数添加到 list。</param>
        /// <param name="name">参数名。</param>
        /// <param name="value">参数的初始值数组。</param>
        public static void Add(this System.Collections.Generic.List<System.Data.Entity.Core.Objects.ObjectParameter> list, bool condition, string name, params object[] value)
        {
            if (condition)
            {
                list.Add(new System.Data.Entity.Core.Objects.ObjectParameter(name, value.Length > 1 ? string.Join(",", value) : value[0]));
            }
        }


        /// <summary>
        /// 添加查询参数
        /// </summary>
        /// <param name="list"></param>
        /// <param name="?"></param>
        public static void Add(this System.Collections.Generic.List<PMSParameters> list, string name, object value, NSqlTypes type = NSqlTypes.Equal, bool isContainsManagedEmployee = false)
        {
            list.Add(new PMSParameters() { IsContainsManagedEmployee = isContainsManagedEmployee, Name = name, Value = value, Type = type });
        }


        /// <summary>
        /// 添加查询参数
        /// </summary>
        /// <param name="list"></param>
        /// <param name="?"></param>
        public static void Add(this System.Collections.Generic.List<PMSParameters> list, bool IsEffective, string name, object value, NSqlTypes type = NSqlTypes.Equal, bool isContainsManagedEmployee = false)
        {
            if (IsEffective)
            {
                list.Add(new PMSParameters() { IsContainsManagedEmployee = isContainsManagedEmployee, Name = name, Value = value, Type = type });
            }
        }

        /// <summary>
        /// 将参数转换成 ObjectParameter 对象添加到 System.Collection.Generic.List＜ObjectParameter＞ 的结尾处。
        /// </summary>
        /// <param name="list">参数列表。</param>
        /// <param name="name">参数名。</param>
        /// <param name="value">参数的初始值数组。</param>
        public static void Add(this System.Collections.Generic.List<System.Data.Entity.Core.Objects.ObjectParameter> list, string name, params object[] value)
        {
            list.Add(new System.Data.Entity.Core.Objects.ObjectParameter(name, value.Length > 1 ? string.Join(",", value) : value[0]));
        }

        #endregion

        #region System.Collections

        /// <summary>
        /// 对值进行比较返回序列中的非重复元素。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">要去重复的对象。</param>
        /// <param name="condition">用于区分重复的委托对象。</param>
        /// <returns></returns>
        public static System.Collections.Generic.List<TSource> Distinct<TSource>(this System.Collections.Generic.List<TSource> source, System.Func<TSource, TSource, bool> condition)
        {
            if (!object.ReferenceEquals(source, null) && source.Count > 1)
            {
                System.Collections.Generic.List<TSource> result = new System.Collections.Generic.List<TSource>(source.Count);

                for (int i = 0; i < source.Count; i++)
                {
                    int count = 0;
                    for (int j = i + 1; j < source.Count; j++)
                    {
                        if (condition.Invoke(source[i], source[j]))
                        {
                            count++;
                            break;
                        }
                    }
                    if (count == 0)
                    {
                        result.Add(source[i]);
                    }
                }
                return result;
            }
            return source;
        }

        /// <summary>
        /// 返回属性包含在指定枚举中的指定属性的集合。
        /// </summary>
        /// <typeparam name="TSource">返回枚举类型。</typeparam>
        /// <typeparam name="TCompare">筛选枚举集合类型。</typeparam>
        /// <typeparam name="TResult">比较元素的类型。</typeparam>
        /// <param name="source"></param>
        /// <param name="sPropertyName">属性名称。</param>
        /// <param name="compare">筛选枚举集合。</param>
        /// <param name="cPropertyName">筛选枚举集合的属性值的集合。</param>
        /// <returns></returns>
        public static IEnumerable<TSource> In<TSource, TCompare, TResult>(this IEnumerable<TSource> source, string sPropertyName, IEnumerable<TCompare> compare, string cPropertyName)
            where TSource : class
            where TCompare : class
            where TResult : struct
        {
            IEnumerable<TResult> tmp = compare.Select(C => (TResult)C.GetPropertyValue(cPropertyName));
            return source.Where(C => tmp.Contains((TResult)C.GetPropertyValue(sPropertyName)));
        }

        /// <summary>
        /// 返回属性包含在指定枚举中的指定属性的集合。
        /// </summary>
        /// <typeparam name="TSource">返回枚举类型。</typeparam>
        /// <typeparam name="TCompare">筛选枚举集合类型。</typeparam>
        /// <param name="source"></param>
        /// <param name="sPropertyName">属性名称。</param>
        /// <param name="compare">筛选枚举集合。</param>
        /// <param name="cPropertyName">筛选枚举集合的属性值的集合。</param>
        /// <returns></returns>
        public static IEnumerable<TSource> In<TSource, TCompare>(this IEnumerable<TSource> source, string sPropertyName, IEnumerable<TCompare> compare, string cPropertyName)
            where TSource : class
            where TCompare : class
        {
            IEnumerable<string> tmp = compare.Select(C => C.GetPropertyValue(cPropertyName).ToString());
            return source.Where(C => tmp.Contains(C.GetPropertyValue(sPropertyName).ToString()));
        }

        /// <summary>
        /// 返回属性包含在指定枚举中的具有相同属性名称的集合。
        /// </summary>
        /// <typeparam name="TSource">返回枚举类型。</typeparam>
        /// <typeparam name="TCompare">筛选枚举集合类型。</typeparam>
        /// <typeparam name="TResult">比较元素的类型。</typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName">属性名称。</param>
        /// <param name="compare">筛选枚举集合。</param>
        /// <returns></returns>
        public static IEnumerable<TSource> In<TSource, TCompare, TResult>(this IEnumerable<TSource> source, string propertyName, IEnumerable<TCompare> compare)
            where TSource : class
            where TCompare : class
            where TResult : struct
        {
            IEnumerable<TResult> tmp = compare.Select(C => (TResult)C.GetPropertyValue(propertyName));
            return source.Where(C => tmp.Contains((TResult)C.GetPropertyValue(propertyName)));
        }

        /// <summary>
        /// 返回属性包含在指定枚举中的具有相同属性名称的集合。
        /// </summary>
        /// <typeparam name="TSource">返回枚举类型。</typeparam>
        /// <typeparam name="TCompare">筛选枚举集合类型。</typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName">属性名称。</param>
        /// <param name="compare">筛选枚举集合。</param>
        /// <returns></returns>
        public static IEnumerable<TSource> In<TSource, TCompare>(this IEnumerable<TSource> source, string propertyName, IEnumerable<TCompare> compare)
            where TSource : class
            where TCompare : class
        {
            IEnumerable<string> tmp = compare.Select(C => C.GetPropertyValue(propertyName).ToString());
            return source.Where(C => tmp.Contains(C.GetPropertyValue(propertyName).ToString()));
        }

        /// <summary>
        /// 返回属性包含在指定枚举中的属性的集合。
        /// </summary>
        /// <typeparam name="TSource">返回枚举类型。</typeparam>
        /// <typeparam name="TCompare">筛选枚举集合类型。</typeparam>
        /// <typeparam name="TResult">比较元素的类型。</typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName">属性名称。</param>
        /// <param name="compare">筛选枚举集合。</param>
        /// <param name="selector">筛选枚举集合的属性值的集合。</param>
        /// <returns></returns>
        public static IEnumerable<TSource> In<TSource, TCompare, TResult>(this IEnumerable<TSource> source, string propertyName, IEnumerable<TCompare> compare, System.Func<TCompare, TResult> selector)
            where TSource : class
            where TCompare : class
        {
            IEnumerable<TResult> tmp = compare.Select(selector);
            return source.Where(C => tmp.Contains((TResult)C.GetPropertyValue(propertyName)));
        }

        #endregion
    }
}
