using AMS.API.Filter;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers
{
    /// <summary>
    /// 继承此类校验校区ID  Bson= BaseSchoolNo
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [SchoolIdValidator]
    public class BsnoController : BaseController
    {

    }
}
