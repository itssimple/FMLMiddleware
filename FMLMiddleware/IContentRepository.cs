using Microsoft.AspNetCore.Http;

namespace FMLMiddleware
{
    public interface IContentRepository
    {
		object GetContentForPath(HttpContext context);
    }
}
