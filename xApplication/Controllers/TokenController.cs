using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xApplication.Security;
using xApplication.Models;

namespace xApplication.Controllers
{
    [Route("token")]
    [AllowAnonymous]
    public class TokenController : BaseApiController
    {
        [HttpPost]
        public IActionResult Create([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                using (Database db = new Database())
                {
                    if(IsPasswordValid(db, loginModel))
                    { 
                        string sql = "SELECT ID FROM USERS WHERE UPPER(USERNAME) = UPPER(@USERNAME)";

                        loginModel.ID = Int32.Parse(db.Query(sql, ExtractParams(sql, loginModel)).First()["ID"].ToString());

                        return Ok(CreateToken(loginModel).Value);
                    }
                    else
                    {
                        return BadRequest(new { USERNAME = new[] { "Wrong Username or Password" } });
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        public static JWTToken CreateToken(LoginModel loginModel)
        {
            return new JWTTokenBuilder()
                    .AddSecurityKey(JWTSecurityKey.Create("fiver-secret-key"))
                    .AddSubject(loginModel.USERNAME + " " + loginModel.PASSWORD)
                    .AddIssuer("Fiver.Security.Bearer")
                    .AddAudience("Fiver.Security.Bearer")
                    .AddClaim("ID", loginModel.ID.ToString())
                    .AddExpiry(5000)
                    .Build();
        }
    }
}