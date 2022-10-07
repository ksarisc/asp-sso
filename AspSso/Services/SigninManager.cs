using AspSso.Models.Config;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace AspSso.Services
{
    public class SigninManager
    {
        private readonly System.Text.Json.JsonSerializerOptions joptions = new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
        };

        private readonly IHttpClientFactory httpFactory;
        private readonly string loginUrl;
        private readonly AuthConfigGoogle google;

        public SigninManager(AuthConfig authConfig, IServer server, IHttpClientFactory httpClientFactory)
        {
            httpFactory = httpClientFactory;
            loginUrl = "https://localhost:7264/account/google"; //signin-google"; // /";
            var addresses = server?.Features.Get<IServerAddressesFeature>();
            if (addresses != null)
            {
                loginUrl = addresses.Addresses.Last();
            }
            //return addresses?.Addresses ?? Array.Empty<string>();
            //authorizeUrl = "https://localhost:7264/account/google-authorize"; //signin-google"; // /";
            google = authConfig.Google;
        }

        private const string googleUrl = @"https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri={0}&" +
            "scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&client_id={1}";
        public string GetGoogleUrl()
        {
            //conf["Authentication:Google:ClientId"]
            return string.Format(googleUrl, loginUrl, google.ClientId);
        }

        private Models.SigninProvider[] providers = Array.Empty<Models.SigninProvider>();
        public IEnumerable<Models.SigninProvider> GetProviders()
        {
            if (providers.Length == 0)
            {
                providers = new[]
                {
                    new Models.SigninProvider("Google", GetGoogleUrl()),
                };
            }

            return providers;
        } // END GetProviders

        private static FormUrlEncodedContent CreateCodeContent(string authCode, string clientId, string clientSecret,
            string redirectUrl = "urn:ietf:wg:oauth:2.0:oob")
        {
            return new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", authCode),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", redirectUrl),
            });
            //var post = string.Format("code={0}&client_id={1}&client_secret={2}&grant_type={4}&redirect_uri={3}",
            //authCode, google.ClientId, google.ClientSecret, "urn:ietf:wg:oauth:2.0:oob", "authorization_code");
        }

        private const string googleTokenUrl = "https://www.googleapis.com/oauth2/v4/token/";
        public async Task<Models.TokenData> ExchangeCodeAsync(string authCode)
        {
            var form = CreateCodeContent(authCode, google.ClientId, google.ClientSecret, loginUrl);
            var req = httpFactory.CreateClient();
            //content-type: application/x-www-form-urlencoded
            //user-agent: google-oauth-playground
            var rsp = await req.PostAsync(googleTokenUrl, form);
            //var req = WebRequest.Create() as HttpWebRequest;
            //req.Method = "POST";
            //req.ContentType = "application/x-www-form-urlencoded";
            //byte[] data = Encoding.UTF8.GetBytes(paras);
            //req.ContentLength = data.Length;
            //using (Stream stream = req.GetRequestStream())
            //    stream.Write(data, 0, data.Length);
            //req.GetResponse();
            if (!rsp.IsSuccessStatusCode)
            {
                // handle error
                return new Models.TokenData { Error = rsp.StatusCode.ToString() };
            }

            var resultFile = $@"C:\temp\google\data\{DateTime.Now:yyyyMMdd_HHmmss_fff}.json";
            var json = await rsp.Content.ReadFromJsonAsync<Models.Sso.GoogleToken>();

            using var output = new System.IO.FileStream(resultFile,
                FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);

            if (json == null)
            {
                using var content = await rsp.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(content);
                using var writer = new StreamWriter(output);
                //System.Diagnostics.Debug.WriteLine("\t\t!! DATA !!");
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (line != null) await writer.WriteLineAsync(line);
                }
                return new Models.TokenData { Error = "Unable to retrieve token data", };
            }

            await System.Text.Json.JsonSerializer.SerializeAsync(output, json, joptions);

            return json.ToTokenData();
        } // END ExchangeCodeAsync
    }
}
