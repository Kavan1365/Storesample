using BaseCore.Helper.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseCore.Configuration
{
    /// <summary>
    /// For Contorlleres to useing IdentityServer
    /// </summary>
    [ApiController]
    [ApiResultFilter]
    [Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
    public class BaseIdentityServerController : ControllerBase
    {
        //public bool UserIsAutheticated => HttpContext.User.Identity.IsAuthenticated;
    }


    ///// <summary>
    ///// Defult Contorller Is use MediatR 
    ///// </summary>
    //[ApiController]
    //[ApiResultFilter]
    //[Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
    //public class BaseMediatRController : ControllerBase
    //{
    //    public readonly IMediator _mediator;
    //    public BaseMediatRController(IMediator mediator)
    //    {
    //        _mediator = mediator;
    //    }
    //    public bool UserIsAutheticated => HttpContext.User.Identity.IsAuthenticated;
    //}

    /// <summary>
    /// Defult Contorller Is use normall 
    /// </summary>
    [ApiController]
    [ApiResultFilter]
    [Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
    [Authorize]
    public class BaseController : ControllerBase
    {
    }



    [ApiController]
    [ApiResultFilter]
    [Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
    public class BaseNotMappController : ControllerBase
    {
    }
}
