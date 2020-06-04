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
using SlicingPieAPI.Models;
using SlicingPieAPI.Repository;

namespace SlicingPieAPI.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SWD_SlicingPieProjectContext _context;
        private IStackHolderRepository _userRepository;
        private IConfiguration _config;

        public LoginController(SWD_SlicingPieProjectContext context, IConfiguration config)
        {
            _context = context;
            _userRepository = new StackHolderRepository(context);
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

            if(user != null)
            {
                var info = _userRepository.GetUserInfo(user);
                var tokenString = GenerateJSONWebToken(info);

                response = Ok(new { token = tokenString });

            }
            return response;
        }

        private string GenerateJSONWebToken(UserLoginDto userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.StackHolderID),
                new Claim("Status", userInfo.StatusID),
                new Claim("RoleID", userInfo.RoleID + ""),
                new Claim("CompanyID", userInfo.CompanyID)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], claims, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
