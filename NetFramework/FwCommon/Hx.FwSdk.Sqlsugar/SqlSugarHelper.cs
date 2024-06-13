using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hx.FwSdk.Sqlsugar
{
    public class SqlSugarHelper //不能是泛型类
    {

        //用单例模式
        public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig()
        {
            ConnectionString = "Server=.xxxxx",//连接符字串
            DbType = DbType.SqlServer,//数据库类型
            IsAutoCloseConnection = true //不设成true要手动close
        },
        db => {
          //(A)全局生效配置点，一般AOP和程序启动的配置扔这里面 ，所有上下文生效
          //调试SQL事件，可以删掉
          db.Aop.OnLogExecuting = (sql, pars) =>
          {
              Console.WriteLine(sql);//输出sql,查看执行sql 性能无影响

              //获取原生SQL推荐 5.1.4.63  性能OK
              //UtilMethods.GetNativeSql(sql,pars)

              //获取无参数化SQL 对性能有影响，特别大的SQL参数多的，调试使用
              //UtilMethods.GetSqlString(DbType.SqlServer,sql,pars)

          };

          //多个配置就写下面
          //db.Ado.IsDisableMasterSlaveSeparation=true;

          //注意多租户 有几个设置几个
          //db.GetConnection(i).Aop
      });
    }

}
