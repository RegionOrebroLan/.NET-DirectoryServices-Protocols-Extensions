using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;

namespace IntegrationTests
{
	public static class Global
	{
		#region Fields

		private static IConfiguration? _configuration;
		private static IHostEnvironment? _hostEnvironment;
		public static readonly DirectoryInfo ProjectDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent!.Parent!.Parent!;

		#endregion

		#region Properties

		public static IConfiguration Configuration => _configuration ??= CreateConfiguration("appsettings.json");
		public static IHostEnvironment HostEnvironment => _hostEnvironment ??= CreateHostEnvironment("Integration-tests");

		#endregion

		#region Methods

		public static IConfiguration CreateConfiguration(params string[] jsonFilePaths)
		{
			var configurationBuilder = CreateConfigurationBuilder();

			foreach(var path in jsonFilePaths)
			{
				configurationBuilder.AddJsonFile(path, true, true);
			}

			configurationBuilder.AddUserSecrets(typeof(Global).Assembly);
			configurationBuilder.AddEnvironmentVariables();

			return configurationBuilder.Build();
		}

		public static IConfigurationBuilder CreateConfigurationBuilder()
		{
			var configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.Properties.Add("FileProvider", HostEnvironment.ContentRootFileProvider);
			return configurationBuilder;
		}

		public static IHostEnvironment CreateHostEnvironment(string environmentName)
		{
			return new HostingEnvironment
			{
				ApplicationName = typeof(Global).Assembly.GetName().Name!,
				ContentRootFileProvider = new PhysicalFileProvider(ProjectDirectory.FullName),
				ContentRootPath = ProjectDirectory.FullName,
				EnvironmentName = environmentName
			};
		}

		public static IServiceCollection CreateServices()
		{
			return CreateServices(Configuration);
		}

		public static IServiceCollection CreateServices(IConfiguration configuration)
		{
			var services = new ServiceCollection();

			services.AddSingleton(configuration);
			services.AddSingleton(HostEnvironment);
			services.AddSingleton<ILoggerFactory, LoggerFactory>();

			return services;
		}

		#endregion
	}
}