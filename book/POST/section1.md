# Post 基础添加删除修改操作
## 后台代码


```
  private void Test_Edit()
        {
        
          
            JGOperItem<Test> jg = new JGOperItem<Test>(Context.Request );
            jg.DoDataAction();
            SuccessResut(jg.SaveT, "", "");
        }
```
##前台代码JSON

//添加

```
PageName:213
AbsoluteUri:123
FromString:123
CreateTime:2017-01-11
SessionID:12
UserID:2
IP:12
LevelInfo:12
Result:213
oper:add
```
//修改

```
ID:1
PageName:213
AbsoluteUri:123
FromString:123
CreateTime:2017-01-11
SessionID:12
UserID:2
IP:12
LevelInfo:12
Result:213
oper:edit
```

//删除

```
ID:1

oper:del
```



