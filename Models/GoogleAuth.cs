using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FirstMVC.Models
{
    public class JsonContent : StringContent
{
    public JsonContent(object obj) :
        base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
    {

     }
}

public class GmailAuthHelper
{
      public string GetUrlForAuthorizationCode(GmailAuth gmailAuth)
    {
        var url=new StringBuilder(gmailAuth.auth_uri);
        url.Append("?client_id=").Append(gmailAuth.client_id)
            .Append("&response_type=").Append(gmailAuth.response_type)
            .Append("&redirect_uri=").Append(gmailAuth.redirect_uri)
            .Append("&scope=").Append(gmailAuth.scope);

        // var urlLink = $"{gmailAuth.auth_uri}?client_id={gmailAuth.client_id}&response_type={gmailAuth.response_type}&redirect_uri={gmailAuth.redirect_uri}&scope={gmailAuth.scope}";
        return url.ToString();
    }

    public FormUrlEncodedContent GetUrlEncodedContent(GmailAuth gmailAuth, string code)
    {
         var objJson = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("grant_type", "authorization_code")
                            ,new KeyValuePair<string, string>("code", code)
                            ,new KeyValuePair<string, string>("redirect_uri", gmailAuth.redirect_uri)
                            ,new KeyValuePair<string, string>("client_id", gmailAuth.client_id)              //consider sending via basic authentication header
                            ,new KeyValuePair<string, string>("client_secret", gmailAuth.client_secret)
                        });
        return objJson;
    }
}
    public class GmailAuth
    {
        public  string client_id { get; set; }
        public  string client_secret { get; set; }
        public  string auth_uri { get; set; }
        public  string token_uri { get; set; }
        public string redirect_uri{get;set;}
        public string scope{get;set;}
        public string response_type{get;set;}
        public string userinfo_request_uri{get;set;}
       
      
    }
    public class GoogleData
    {
        public string Access_token { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class GoogleUserInfo
    {
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string id { get; set; }
        public string picture { get; set; }

    }

    

    


}