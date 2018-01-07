using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xApplication.Models;

namespace xApplication.Controllers
{
    [Produces("application/json")]
    public class AccountController : BaseApiController
    {
        [HttpPost]
        [Route("api/account/register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                using (Database db = new Database())
                {
                    string sql = "SELECT * FROM USERS WHERE UPPER(USERNAME) = UPPER(@USERNAME)";

                    var result = db.Query(sql, ExtractParams(sql, registerModel));

                    if(result != null)
                    {
                        return BadRequest(new { USERNAME = new[] { "Username is already taken" } });
                    }

                    if (!String.IsNullOrEmpty(registerModel.MAIL))
                    {
                        sql = "SELECT * FROM USERS WHERE UPPER(MAIL) = UPPER(@MAIL)";

                        if (db.Query(sql, ExtractParams(sql, registerModel)) != null)
                        {
                            return BadRequest(new { MAIL = new[] { "E-Mail is already taken" } });
                        }
                    }

                    registerModel.PASSWORD = OneWayEncrypt(registerModel.PASSWORD);

                    sql = "INSERT INTO USERS(USERNAME, MAIL, PASSWORD)VALUES(@USERNAME,@MAIL,@PASSWORD)";

                    db.NonQuery(sql, ExtractParams(sql, registerModel));

                    sql = "SELECT ID FROM USERS WHERE UPPER(USERNAME) = UPPER(@USERNAME)";

                    registerModel.ID = Int32.Parse(db.Query(sql, ExtractParams(sql, registerModel)).First()["ID"].ToString());

                    return Ok(TokenController.CreateToken(registerModel).Value);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/account/context")]
        public IActionResult GetContextForUser()
        {
            using (Database db = new Database())
            {
                string sql = "SELECT IMGURL FROM USERS WHERE ID = @ID";

                return Ok(db.Query(sql, ExtractParams(sql, new { ID = GetCurrentUserId() })));
            }
        }
    }
}