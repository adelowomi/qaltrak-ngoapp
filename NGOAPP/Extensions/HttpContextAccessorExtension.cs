namespace NGOAPP.Extensions;

public static class HttpContextAccessorExtension
{
    public static bool IsMobileRequest(this IHttpContextAccessor httpContextAccessor)
    {
        var isMobile = httpContextAccessor.HttpContext.Request.Headers["IsMobile"].ToString();
        return isMobile == "true";
    }
}
