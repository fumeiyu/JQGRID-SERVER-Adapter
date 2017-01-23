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

##在数据绑定后执行数据转换

  

```
 Action<t_UserFav, JGOperItem<t_UserFav>> before = (x, y) =>
            {
                if (y.SaveT.MyFavID == frontUserID && x.FavType == 4)
                {
                    throw new Exception("不能关注自己");
                }
                y.SaveT.ID=2;
            };

            JGOperItem<t_UserFav> op = new JGOperItem<t_UserFav>(Context.Request, a, null, del, before, null);


                op.DoDataAction();
                result = 1;
                //输出json代码

```

