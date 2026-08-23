using Microsoft.AspNetCore.Mvc;
using auth.Models;


[ApiController]
[Route("auth")]
public class AuthUser:ControllerBase
{
    private readonly IHttpClientFactory _IClient;

    public AuthUser(IHttpClientFactory iclientFactory)
    {
        _IClient = iclientFactory;
    }
    
}