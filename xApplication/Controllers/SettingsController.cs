using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xApplication.Models;

namespace xApplication.Controllers
{
    [Produces("application/json")]
    public class SettingsController : BaseApiController
    {
        [HttpGet]
        [Authorize]
        [Route("api/settings")]
        public IActionResult Get()
        {
            using (Database db = new Database())
            {
                string sql = "SELECT * FROM USERS WHERE ID = @ID";

                var res = db.Query(sql, ExtractParams(sql, new { ID = GetCurrentUserId() })).First();

                return Ok(new
                {
                    USERNAME = res["USERNAME"].ToString(),
                    MAIL = res["MAIL"].ToString(),
                    IMGURL = res["IMGURL"].ToString()
                });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/settings")]
        public IActionResult Post([FromBody] SettingsModel settingsModel)
        {
            if (ModelState.IsValid)
            {
                using (Database db = new Database())
                {
                    if (IsPasswordValid(db, settingsModel))
                    {
                        if (!String.IsNullOrEmpty(settingsModel.NEWPASSWORD))
                        {
                            if (String.IsNullOrEmpty(settingsModel.CONFIRMPASSWORD))
                            {
                                return BadRequest(new { CONFIRMPASSWORD = new[] { "Confirm-Password cannot be left empty" } });
                            }

                            if (settingsModel.NEWPASSWORD != settingsModel.CONFIRMPASSWORD)
                            {
                                return BadRequest(new { CONFIRMPASSWORD = new[] { "Confirm-Password does not match your new password" } });
                            }

                            settingsModel.PASSWORD = OneWayEncrypt(settingsModel.NEWPASSWORD);
                        }

                        string sql = "UPDATE USERS SET USERNAME = @USERNAME, MAIL = @MAIL, PASSWORD = @PASSWORD WHERE ID = @ID";

                        settingsModel.ID = GetCurrentUserId();

                        db.NonQuery(sql, ExtractParams(sql, settingsModel));

                        return Ok(new
                        {
                            USERNAME = settingsModel.USERNAME,
                            MAIL = settingsModel.MAIL
                        });
                    }
                    else
                    {
                        return BadRequest(new { PASSWORD = new[] { "Wrong password" } });
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/settings/avatar")]
        public IActionResult PostImage([FromBody] dynamic data)
        {
            byte[] strim = Convert.FromBase64String(data.image.ToString());

            string newFileName = Guid.NewGuid().ToString() + ".png";

            System.IO.File.WriteAllBytes(Startup.HostingEnvironment.ContentRootPath + @"\wwwroot\media\images\" + newFileName, strim);
            
            using(Database db = new Database())
            {
                string sql = "UPDATE USERS SET IMGURL = @IMGURL WHERE ID = @ID";

                db.NonQuery(sql, ExtractParams(sql, new { IMGURL = @"\media\images\" + newFileName, ID = GetCurrentUserId() })); 
            }

            return Ok("");
        }
    }
}