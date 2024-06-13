using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用Swagger进行api文档的展示
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

                //根据版本名称倒序 遍历展示
                var ApiName = "Hx.Sdk";
                var version = "V1";
                c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                //c.SwaggerEndpoint($"https://petstore.swagger.io/v2/swagger.json", $"{ApiName} pet");
                //// 将swagger首页，设置成我们自定义的页面，记得这个字符串的写法：{项目名.index.html}
                //if (streamHtml.Invoke() == null)
                //{
                //    var msg = "index.html的属性，必须设置为嵌入的资源";
                //    throw new Exception(msg);
                //}
                //c.IndexStream = streamHtml;


                //路径配置，设置为空，表示直接访问该文件，
                //路径配置，设置为空，表示直接在根域名（http://localhost:52909）访问该文件,注意http://localhost:52909/swagger是访问不到的，
                //这个时候去launchSettings.json中把"launchUrl": "swagger/index.html"去掉， 然后直接访问http://localhost:52909/index.html即可
                c.RoutePrefix = "swagger";
            });
        }
    }
}
