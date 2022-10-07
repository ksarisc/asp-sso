namespace AspSso.Models.Config
{
    public class AuthConfig
    {
        //conf["Authentication:Google:ClientId"]
        public AuthConfigGoogle Google { get; set; }

        public static AuthConfig Parse(IConfiguration conf)
        {
            //conf["Authentication:Google:ClientSecret"];
            //Console.WriteLine(conf["Authentication:Google:ClientId"]);
            var section = conf.GetSection("Authentication");
            var instance = new AuthConfig();
            section.Bind(instance);
            return instance;
        }
    }

    public class AuthConfigGoogle
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
