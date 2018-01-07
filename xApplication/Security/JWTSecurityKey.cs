using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace xApplication.Security
{
    public sealed class JWTSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}