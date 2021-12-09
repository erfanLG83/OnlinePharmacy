using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Utility
{
    public class SiteSetting
    {
        public SiteSetting(IConfiguration configuration)
        {
            GoogleOAuth = new GoogleOAuth2()
            {
                ClinetID = configuration["SiteSetting:GoogleOAuth2:ClinetID"],
                ClientSecret = configuration["SiteSetting:GoogleOAuth2:ClinetSecret"]
            };
        }
        public record GoogleOAuth2
        {
            public string ClinetID { get; init; }
            public string ClientSecret { get; init; }
        }
        public GoogleOAuth2 GoogleOAuth { get; set; }
    }
}
