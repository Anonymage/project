using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Security.Cryptography;
using xApplication.Models;

namespace xApplication.Controllers
{
    public class BaseApiController : Controller
    {
        public int GetCurrentUserId()
        {
            return Int32.Parse(User.Claims.Where(c => c.Type == "ID").First().Value);
        }

        public static SqliteParameter[] ExtractParams(string sql, dynamic data)
        {
            MatchCollection mc = Regex.Matches(sql, @"\@[A-Z]*");

            List<SqliteParameter> parameters = new List<SqliteParameter>();

            foreach (var prop in data.GetType().GetProperties())
            {
                if (mc.Where(m => m.Value.Replace("@", "") == prop.Name).Count() > 0)
                {
                    parameters.Add(new SqliteParameter() { ParameterName = prop.Name, Value = prop.GetValue(data, null) });
                }
            }

            return parameters.ToArray();
        }

        public static string OneWayEncrypt(string s)
        {
            return System.Text.Encoding.Default.GetString(new SHA256Managed().ComputeHash(new byte[s.Length]));
        }

        public static bool IsPasswordValid(Database db, LoginModel loginModel)
        {
            loginModel.PASSWORD = OneWayEncrypt(loginModel.PASSWORD);

            string sql = "SELECT * FROM USERS WHERE UPPER(USERNAME) = UPPER(@USERNAME) AND PASSWORD = @PASSWORD";

            var res = db.Query(sql, ExtractParams(sql, loginModel));

            if (res != null)
            {
                if (res.Count == 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}