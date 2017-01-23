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

##在数据绑定前执行数据转换
用来确定前端没有传过来正常的ID,或者需要转换后才能获取的ID,获取其他功能

  

```
 Action<t_UserFav, JGOperItem<t_UserFav>> before = (x, y) =>
            {
                if (y.SaveT.MyFavID == frontUserID && x.FavType == 4)
                {
                    throw new Exception("不能关注自己");
                }
                y.SaveT.ID=2;
            };
            
              Action<t_User, JGOperItem<t_User>> setPassword = (a1, s1) =>
            {
                s1.ID = Userid;
            }; //获取需要在操作前，重新修改数据ID,来修改特定的记录数

            JGOperItem<t_UserFav> op = new JGOperItem<t_UserFav>(Context.Request, a, null, del, before, null);


                op.DoDataAction();
                result = 1;
                //输出json代码

```

