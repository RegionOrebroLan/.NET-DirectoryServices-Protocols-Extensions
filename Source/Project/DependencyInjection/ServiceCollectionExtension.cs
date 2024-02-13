using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace RegionOrebroLan.DirectoryServices.Protocols.DependencyInjection
{
	[CLSCompliant(false)]
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddDirectories(this IServiceCollection services, IConfiguration configuration)
		{
			return services.AddDirectories(configuration, ConfigurationKeys.DirectoriesPath);
		}

		public static IServiceCollection AddDirectories(this IServiceCollection services, IConfiguration configuration, string configurationKey)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			services.ConfigureDirectories(configuration, configurationKey);

			var directoriesSection = configuration.GetSection(configurationKey);

			services.AddSingleton<IDictionary<string, IDirectory>>(serviceProvider =>
			{
				var dictionary = new Dictionary<string, IDirectory>(StringComparer.OrdinalIgnoreCase);

				foreach(var directorySection in directoriesSection.GetChildren())
				{
					var key = directorySection.Key;

					dictionary.Add(
						key,
						new Directory(
							new LdapConnectionFactory(
								() => serviceProvider.GetRequiredService<IOptionsMonitor<LdapConnectionOptions>>().Get(key)
							),
							() => serviceProvider.GetRequiredService<IOptionsMonitor<DirectoryOptions>>().Get(key)
						)
					);
				}

				return dictionary;
			});

			return services;
		}

		public static IServiceCollection AddDirectories<TEntry, TImplementation>(this IServiceCollection services, IConfiguration configuration, Func<Func<DirectoryOptions>, Func<LdapConnectionOptions>, IServiceProvider, TImplementation> directoryFactory) where TImplementation : class, IDirectory<TEntry>
		{
			return services.AddDirectories<TEntry, TImplementation>(configuration, ConfigurationKeys.DirectoriesPath, directoryFactory);
		}

		public static IServiceCollection AddDirectories<TEntry, TImplementation>(this IServiceCollection services, IConfiguration configuration, string configurationKey, Func<Func<DirectoryOptions>, Func<LdapConnectionOptions>, IServiceProvider, TImplementation> directoryFactory) where TImplementation : class, IDirectory<TEntry>
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if(directoryFactory == null)
				throw new ArgumentNullException(nameof(directoryFactory));

			services.ConfigureDirectories(configuration, configurationKey);

			var directoriesSection = configuration.GetSection(configurationKey);

			services.AddSingleton<IDictionary<string, IDirectory<TEntry>>>(serviceProvider =>
			{
				var dictionary = new Dictionary<string, IDirectory<TEntry>>(StringComparer.OrdinalIgnoreCase);

				foreach(var directorySection in directoriesSection.GetChildren())
				{
					var key = directorySection.Key;

					dictionary.Add(
						key,
						directoryFactory(
							() => serviceProvider.GetRequiredService<IOptionsMonitor<DirectoryOptions>>().Get(key),
							() => serviceProvider.GetRequiredService<IOptionsMonitor<LdapConnectionOptions>>().Get(key),
							serviceProvider
						)
					);
				}

				return dictionary;
			});

			return services;
		}

		public static IServiceCollection AddDirectory(this IServiceCollection services, IConfiguration configuration)
		{
			return services.AddDirectory(configuration, ConfigurationKeys.DirectoryPath, ConfigurationKeys.DirectoryConnectionStringName);
		}

		public static IServiceCollection AddDirectory(this IServiceCollection services, IConfiguration configuration, string configurationKey, string connectionStringName)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			services.ConfigureDirectory(configuration, configurationKey, connectionStringName);

			services.TryAddSingleton<IDirectory, Directory>();
			services.AddDirectoryCore();

			return services;
		}

		public static IServiceCollection AddDirectory<TEntry, TImplementation>(this IServiceCollection services, IConfiguration configuration) where TImplementation : class, IDirectory<TEntry>
		{
			return services.AddDirectory<TEntry, TImplementation>(configuration, ConfigurationKeys.DirectoryPath, ConfigurationKeys.DirectoryConnectionStringName);
		}

		public static IServiceCollection AddDirectory<TEntry, TImplementation>(this IServiceCollection services, IConfiguration configuration, string configurationKey, string connectionStringName) where TImplementation : class, IDirectory<TEntry>
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			services.ConfigureDirectory(configuration, configurationKey, connectionStringName);

			services.TryAddSingleton<IDirectory<TEntry>, TImplementation>();
			services.AddDirectoryCore();

			return services;
		}

		public static IServiceCollection AddDirectoryCore(this IServiceCollection services)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			services.TryAddSingleton<ILdapConnectionFactory, LdapConnectionFactory>();

			return services;
		}

		public static IServiceCollection ConfigureDirectories(this IServiceCollection services, IConfiguration configuration)
		{
			return services.ConfigureDirectories(configuration, ConfigurationKeys.DirectoriesPath);
		}

		public static IServiceCollection ConfigureDirectories(this IServiceCollection services, IConfiguration configuration, string configurationKey)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			var directoriesSection = configuration.GetSection(configurationKey);

			foreach(var directorySection in directoriesSection.GetChildren())
			{
				var key = directorySection.Key;
				var connectionStringName = directorySection.GetSection("ConnectionStringName").Value;

				services.Configure<DirectoryOptions>(key, directorySection);
				services.Configure<LdapConnectionOptions>(key, options =>
				{
					var ldapConnectionOptions = new LdapConnectionStringParser().Parse(configuration.GetConnectionString(connectionStringName));
					options.AuthenticationType = ldapConnectionOptions.AuthenticationType;
					options.Credential = ldapConnectionOptions.Credential;
					options.DirectoryIdentifier = ldapConnectionOptions.DirectoryIdentifier;
					options.ProtocolVersion = ldapConnectionOptions.ProtocolVersion;
					options.Timeout = ldapConnectionOptions.Timeout;
				});
			}

			return services;
		}

		public static IServiceCollection ConfigureDirectory(this IServiceCollection services, IConfiguration configuration)
		{
			return services.ConfigureDirectory(configuration, ConfigurationKeys.DirectoryPath, ConfigurationKeys.DirectoryConnectionStringName);
		}

		public static IServiceCollection ConfigureDirectory(this IServiceCollection services, IConfiguration configuration, string configurationKey, string connectionStringName)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			services.Configure<DirectoryOptions>(configuration.GetSection(configurationKey));
			services.Configure<LdapConnectionOptions>(options =>
			{
				var connectionOptions = new LdapConnectionStringParser().Parse(configuration.GetConnectionString(connectionStringName));
				options.AuthenticationType = connectionOptions.AuthenticationType;
				options.Credential = connectionOptions.Credential;
				options.DirectoryIdentifier = connectionOptions.DirectoryIdentifier;
				options.ProtocolVersion = connectionOptions.ProtocolVersion;
				options.Timeout = connectionOptions.Timeout;
			});

			return services;
		}

		#endregion
	}
}