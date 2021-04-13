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
}