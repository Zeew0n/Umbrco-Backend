using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using umbraco.providers.members;
using Umbraco.Web.Security.Providers;

namespace uSome.Membership.CustomMemership
{
    public  class CustomMemberShipProvider :Umbraco.Web.Security.Providers.MembersMembershipProvider
    {
        public override bool ValidateUser(string username, string password)
        {
            string hashPwd = hashPassword(password);

            return base.ValidateUser(username, hashPwd);
        }
        public string hashPassword(string password)
        {
            HMACSHA1 hash = new HMACSHA1();
            hash.Key = Encoding.Unicode.GetBytes(password);

            string encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
            return encodedPassword;
        }
    }
}