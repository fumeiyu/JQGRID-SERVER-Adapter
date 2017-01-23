# GET 获取数据
    
## 获取列表
    
 后台代码


```
   private void User_Query()
        {
           
            JqGridSearch<User> search = new JqGridSearch<User>(Context.Request);
            var result = search.Search(base.NeedCount);//是否需要输出总记        录条数
            SuccessGridResult(result); //输出jison
        }
```
输出json内容


```
{
    "success": true,
    "msg": "",
    "url": "",
    "page": "1",
    "total": "1",
    "records": "1",
    "rows": [
        {
            "ID": 12,
            "UserName": "admin",
            "RealName": "admin",
            "UserRoles": 15,
            "ModifyDate": "2016-09-22T21:08:02",
            "CreateDate": "2016-09-22T21:08:02",
            "StatusId": 0
        }
    ]
}
```


  前台请求json
  


```
rows:10 //数据行数
page:1 //第几页
sidx: //排序字段
sord:asc //排序方式
```




            

