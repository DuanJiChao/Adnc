﻿using Adnc.Shared.RpcServices.Rtos;
using Adnc.Shared.RpcServices.Services;
using Adnc.WebApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Adnc.Maint.WebApi.Controllers
{
    [Route("maint/session")]
    [ApiController]
    public class AccountController : AdncControllerBase
    {
        private readonly IAuthRpcService _authRpcService;

        public AccountController(IAuthRpcService authRpcService)
            =>_authRpcService = authRpcService;

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] LoginRto input)
        {
            var result = await _authRpcService.LoginAsync(input);

            if (result.IsSuccessStatusCode)
                return Ok(result.Content);

            return Problem(result.Error);
        }
    }
}