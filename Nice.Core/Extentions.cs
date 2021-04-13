using Newtonsoft.Json;
using Nice;
using Nice.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 常用扩展方法
/// </summary>
public static class Extentions
{
    /// <summary>
    /// 获取只有ID和名称的列表,通常用于向前端返回下拉框数据
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static List<NamedItem> ToItems(this IEnumerable<INamedItem> source)
    {
        return source.Select(o => new NamedItem()
        {
            ID = o.ID,
            Name = o.Name
        }).ToList();
    }
    public static string ToJson<T>(this T data) where T : class, new()
    {
        return JsonConvert.SerializeObject(data);
    }

    public static T FromJson<T>(this string json) where T : class, new()
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static object FromJson(this string json, Type type)
    {
        var res = JsonConvert.DeserializeObject(json, type);
        return res;
    }
}