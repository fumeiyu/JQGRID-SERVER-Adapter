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
  前台请求json
      


            

