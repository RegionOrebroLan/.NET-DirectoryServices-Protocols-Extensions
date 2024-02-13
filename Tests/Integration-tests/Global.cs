using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests
{
	[TestClass]
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
	public static class Global
	{
		#region Fields

		private static IConfiguration _configuration;
		private static IHostEnvironment _hostEnvironment;

		// ReSharper disable PossibleNullReferenceException
		public static readonly string ProjectDirectoryPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
		// ReSharper restore PossibleNullReferenceException

		#endregion

		#region Properties

		public static IConfiguration Configuration => _configuration ??= CreateConfiguration("appsettings.json");
		public static IHostEnvironment HostEnvironment => _hostEnvironment ??= CreateHostEnvironment("Integration-tests");

		#endregion

		#region Methods

		[AssemblyCleanup]
		public static void Cleanup() { }

		public static IConfiguration CreateConfiguration(params string[] jsonFilePaths)
		{
			var configurationBuilder = CreateConfigurationBuilder();

			foreach(var path in jsonFilePaths ?? [])
			{
				configurationBuilder.AddJsonFile(path, true, true);
			}

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
				ApplicationName = typeof(Global).Assembly.GetName().Name,
				ContentRootFileProvider = new PhysicalFileProvider(ProjectDirectoryPath),
				ContentRootPath = ProjectDirectoryPath,
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
			//services.AddSingleton<ILoggerFactory, LoggerFactoryMock>();

			return services;
		}

		[AssemblyInitialize]
		public static void Initialize(TestContext testContext)
		{
			if(testContext == null)
				throw new ArgumentNullException(nameof(testContext));
		}

		#endregion
	}
}