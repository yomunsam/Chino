using System.Threading.Tasks;
using AutoMapper;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Dtos.Account;
using Chino.IdentityServer.Dtos.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chino.IdentityServer.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ChinoUser> m_SignInManager;
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly IMapper m_Mapper;

        public AccountController(SignInManager<ChinoUser> signInManager,
            UserManager<ChinoUser> userManager,
            IMapper mapper)
        {
            this.m_SignInManager = signInManager;
            this.m_UserManager = userManager;
            this.m_Mapper = mapper;
        }

        //[HttpPost("login")]
        //public async Task<ActionResult<LoginResultDto>> AccountLogin(LoginDto dto)
        //{
        //    ChinoUser userEntity = null;
        //    if (dto.IdentityString.Contains('@'))
        //    {
        //        userEntity = await m_UserManager.FindByEmailAsync(dto.IdentityString);
        //    }

        //    if (userEntity == null)
        //        userEntity = await m_UserManager.FindByNameAsync(dto.IdentityString);

        //    if (userEntity == null)
        //        return NotFound(ApiErrorBaseDto.CreateError(0, "User not found");

            
        //}

        [HttpGet("info")]
        public async Task<ActionResult<UserInfoDto>> AccountInfo()
        {
            var user = await m_UserManager.GetUserAsync(this.User);
            if (user == null)
                return NotFound(ApiErrorBaseDto.CreateError(0, "User info not found"));
            else
                return m_Mapper.Map<UserInfoDto>(user);
        }

    }
}
