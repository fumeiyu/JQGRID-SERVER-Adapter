# GET 进阶
## 后台定义查询条件


```

  private void TestType_Query()
        {
            ICriterion ic = GetNoDeleteEq(); //第一delete不等于NONE
            JqGridSearch<TestType> search = new JqGridSearch<TestType>(Context.Request, ic);
            var result = search.Search(base.NeedCount);
            SuccessGridResult(result);
        }
```
##输出字段
过滤输出字段 过滤字段name
 

```
 private void TestType_Query()
        {
            ICriterion ic = GetNoDeleteEq();
            JqGridSearch<TestType> search = new JqGridSearch<TestType>(Context.Request, ic);
            var result = search.Search(base.NeedCount);
            SuccessGridResult(result, new string[1] { "Name" }, true);

        }
```


    
只输出字段name
```
 private void TestType_Query()
        {
            ICriterion ic = GetNoDeleteEq();
            JqGridSearch<TestType> search = new JqGridSearch<TestType>(Context.Request, ic);
            var result = search.Search(base.NeedCount);
            SuccessGridResult(result, new string[1] { "Name" }, false);

        }
```



