using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VolunteerHub.Model;

namespace VolunteerHub.ViewModel
{
    public class EmailValidator
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string domainName = Regex.Match(email, @"(@)(.+)$").Groups[2].Value;

                domainName = new IdnMapping().GetAscii(domainName);

                email = Regex.Replace(email, @"(@)(.+)$", $"@{domainName}");

                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static bool CheckUniqueEmail(string email)
        {
            using (var db = new dbVolunteerHubEntities())
            {
                var valiedEmail = db.Users.Count(u => u.Email == email);
                if (valiedEmail > 0)
                    return false;
                else
                    return true;
            }
        }

        private static string DomainMapper(Match match)
        {
            var idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                throw;
            }

            return match.Groups[1].Value + domainName;
        }
    }
}
