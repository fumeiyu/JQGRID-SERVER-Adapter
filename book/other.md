# 其他主要方法 需要引用Common.UI.JQGrid命名空间


```
public static ToStringNoNull(this string str, string defaultstring = "")  
public static int ToInt(this string str, int defaultvalue=1)
public static T HttpRequestConvertToT<T>(this HttpRequest request, out object ID) where T : new() //httprequest转换成对象   
public static T HttpRequestConvertToT<T>(this HttpRequest request, List<String> filedList,bool fiterType, out object ID) where T : new() 转换对象，并过滤不需要转换的字段，或者只转换特定字段
```

