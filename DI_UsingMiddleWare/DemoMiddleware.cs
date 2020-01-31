using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DI_UsingMiddleWare
{
    public class DemoMiddleware
    {
        private readonly RequestDelegate _next;

        public DemoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //使用 InvokeAsync 方法在中间件中做某事,但下面的方法只适用于单个服务实例，IDemoService有两个类实现了其接口，如果这样使用的话，只能最后注册的那个实例生效。
        //public async Task InvokeAsync(HttpContext context,IDemoService sv)
        //{
        //    await context.Response.WriteAsync(sv.Version);
        //    sv.Run();
            
        //    //执行完中间件后，应使用下面语句把控制权放回到下个中间件，如果没有的话，就会中止请求，不执行以后的中间件了
        //    await _next.Invoke(context);
        //}

        public async Task InvokeAsync(HttpContext context, IEnumerable<IDemoService> svs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var sv in svs)
            {
                sb.Append($"{sv.Version}<br/>");
                sv.Run();
            }
            await context.Response.WriteAsync(sb.ToString());

            //执行完中间件后，应使用下面语句把控制权放回到下个中间件，如果没有的话，就会中止请求，不执行以后的中间件了
            await _next.Invoke(context);
        }
    }
}
