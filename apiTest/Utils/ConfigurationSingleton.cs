namespace Voalaft.API.Utils
{
    public class ConfigurationSingleton
    {
        private static readonly Lazy<IConfigurationRoot> _lazyConfig = new Lazy<IConfigurationRoot>(() =>
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",
     optional: false, reloadOnChange: true);
            return builder.Build();
        });

        public
     static IConfigurationRoot Instance => _lazyConfig.Value;
    }
}
