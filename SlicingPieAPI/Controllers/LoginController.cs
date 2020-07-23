using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Enums;
using SlicingPieAPI.Models;
using SlicingPieAPI.Services;

namespace SlicingPieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IStakeHolderService _shService;
        private IConfiguration _config;

        public LoginController(IAccountService accountService, IStakeHolderService shService, IConfiguration config)
        {
            _accountService = accountService;
            _shService = shService;
            _config = config;
        }

        // GET: api/Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetStackHolders()
        {
            IActionResult response = Unauthorized();

            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                .VerifyIdTokenAsync(Request.Headers[HeaderNames.Authorization]);
            string uid = decodedToken.Uid;

            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
            var user = userRecord.Email;

            if (user != null)
            {
                var info = _accountService.getAccountInfo(user);
                if (info.Result == null) return Unauthorized();
                else
                {
                    if (info.Result.RoleID.Equals(Role.ADMIN))
                    {
                        var tokenString = GenerateJSONWebTokenAdmin(info.Result);
                        return Ok(new { token = tokenString, Name = info.Result.NameAccount, Role = info.Result.RoleID });
                    }
                    else if (info.Result.RoleID.Equals(Role.USER))
                    {
                        string companyId = _shService.getStakeHolderCompanyID(info.Result.AccountID).Result;

                        if (companyId == null)
                        {
                            response = Unauthorized();
                        }
                        else
                        {
                            var shInfo = _shService.getStakeHolderLoginInoByID(info.Result.AccountID);
                            var tokenString = GenerateJSONWebToken(shInfo.Result);

                            response = Ok(new { token = tokenString, StakeHolderID = shInfo.Result.SHID, CompanyId = shInfo.Result.CompanyID, Role = shInfo.Result.RoleID, companyName = shInfo.Result.CompanyName, ShImage = shInfo.Result.Shimage, shName = shInfo.Result.SHName });
                        }
                    }
                }
            }
            return response;
        }

        // GET: api/Login
        [Authorize]
        [HttpPost("switch-account/{accountid}/{companyid}")]
        public async Task<IActionResult> GetStackHoldersSwitch(string accountid, string companyid)
        {
            var shInfo = await _shService.SwithcAccountSV(accountid, companyid).FirstOrDefaultAsync();
            if (shInfo != null)
            {
                var tokenString = GenerateJSONWebToken(shInfo);

                return Ok(new { token = tokenString, StakeHolderID = shInfo.SHID, CompanyId = shInfo.CompanyID, Role = shInfo.RoleID, companyName = shInfo.CompanyName, ShImage = shInfo.Shimage, shName = shInfo.SHName });
            }
            else return NotFound();
        }

        private string GenerateJSONWebToken(StakeHolderDto userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.SHID),
                new Claim("RoleID", userInfo.RoleID + ""),
                new Claim("CompanyID", userInfo.CompanyID),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], claims, expires: DateTime.Now.AddDays(60), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateJSONWebTokenAdmin(UserLoginDto userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.AccountID),
                new Claim("RoleID", userInfo.RoleID + ""),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], claims, expires: DateTime.Now.AddDays(60), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
