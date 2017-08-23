using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FirstMVC.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace FirstMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
             ViewData["Message"] = "This is About Page.";
             return View();
        }
      
        public async Task<GoogleUserInfo> GetGoogleData(string code)
        {
          var gmailAuth=new GmailAuth();
          gmailAuth.client_id="client_id";
          gmailAuth.client_secret="client_secret";
          gmailAuth.auth_uri="https://accounts.google.com/o/oauth2/auth";
          gmailAuth.token_uri="https://accounts.google.com/o/oauth2/token";
          var domainName="http://localhost:5000";
   
        //   var responseType="code";
        //   var scope="https://www.googleapis.com/auth/userinfo.profile";
          
          gmailAuth.redirect_uri=$"{domainName}/home/getgoogledata/sign-in";
            
            using(var client=new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var objJson = new GmailAuthHelper().GetUrlEncodedContent(gmailAuth, code);
                response = await client.PostAsync(gmailAuth.token_uri, objJson);
                if (response.IsSuccessStatusCode)
                {
                    var jsondata = response.Content.ReadAsStringAsync().Result;
                    GoogleData mytoken = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<GoogleData>(jsondata));
                   GoogleUserInfo userinfo=await GetUserDetails(mytoken.Access_token);
                   return userinfo;
                   
                }
                else
                {
                    return null;
                }
            }
            
        }

        private async Task<GoogleUserInfo> GetUserDetails(string access_token)
        {
            using(var client=new HttpClient())
            {
                var userinfo_request_uri="https://www.googleapis.com/oauth2/v2/userinfo";
                var url=$"{userinfo_request_uri}?access_token={access_token}";
                client.BaseAddress=new Uri(url);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response;
                response=await client.GetAsync(url);
                if(response.IsSuccessStatusCode)
                {
                    var result=response.Content.ReadAsStringAsync().Result;
                    GoogleUserInfo info=await Task.Factory.StartNew(()=>JsonConvert.DeserializeObject<GoogleUserInfo>(result));
                    return info;

                }
                else{
return null;
                }

            }
            
        }

        public IActionResult Google()
        {
        
          var gmailAuth=new GmailAuth();
          gmailAuth.client_id="client_id";
          gmailAuth.client_secret="client_secret";
          gmailAuth.auth_uri="https://accounts.google.com/o/oauth2/auth";
          gmailAuth.token_uri="https://accounts.google.com/o/oauth2/token";
          var domainName="http://localhost:5000";
   
          gmailAuth.response_type="code";
          gmailAuth.scope="https://www.googleapis.com/auth/userinfo.email";
          
          gmailAuth.redirect_uri=$"{domainName}/home/getgoogledata/sign-in";
          var urlLink=new GmailAuthHelper().GetUrlForAuthorizationCode(gmailAuth);

        //   var urlLink = $"{gmailAuth.auth_uri}?client_id={gmailAuth.client_id}&response_type={}&redirect_uri={gmailAuth.redirect_uri}&scope={gmailAuth.scope}";
         return Redirect(urlLink);
        }

       

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
