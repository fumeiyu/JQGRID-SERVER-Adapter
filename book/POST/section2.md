# POST进阶

## 过滤添加修改字段 

    

```

            List<string> fiter = new List<string>();
            fiter.Add("Noedit"); //过滤这2个字段
            fiter.Add("NoEdit1");
            JGOperItem<Test> jg = new JGOperItem<Test>(Context.Request, null, null, null, null, fiter, true);
            jg.DoDataAction();
            SuccessResut(jg.SaveT, "", "");
```

##添加修改删除前执行操作



```
          Action<Test> add = (a) =>
            {
                a.UserId = base.UserId;
            };
            Action<Test> update = (a) => { a.UserId = base.UserId; };
            Action<Test> del = (a) => { a.UserId = base.UserId; };
            JGOperItem<Test> jg = new JGOperItem<Test>(Context.Request, add, update, del);
            jg.DoDataAction();
            SuccessResut(jg.SaveT, "", "");
```

##通过postAop过滤操作类型 信息请见【ACE模板】

