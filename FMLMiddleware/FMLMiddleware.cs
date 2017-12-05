using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FMLMiddleware
{
    public class FMLMiddleware
	{
		readonly RequestDelegate _next;
		readonly IContentRepository _content;

		public FMLMiddleware(RequestDelegate next, IContentRepository content)
		{
			_next = next;
			_content = content;
		}

		public async Task Invoke(HttpContext context)
		{
			var contentData = _content.GetContentForPath(context);
			if (contentData == null)
			{
				await _next(context);
				return;
			}

			await RenderData(context, contentData);
			return;
		}

		internal Task RenderData(HttpContext context, object contentData)
		{
			return context.Response.WriteAsync(contentData.ToString());
		}
    }
}

public static class FMLMiddlewareExtensions
{
	public static IApplicationBuilder UseFMLMiddleware(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<FMLMiddleware.FMLMiddleware>();
	}
}
