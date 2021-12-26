using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;
using System.Linq;
using IntegrationTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.DirectoryServices.Protocols;
using RegionOrebroLan.DirectoryServices.Protocols.DependencyInjection;
using RegionOrebroLan.DirectoryServices.Protocols.Extensions;

namespace IntegrationTests
{
	[TestClass]
	public class DirectoryTest
	{
		#region Fields

		private static IServiceProvider _serviceProvider;

		#endregion

		#region Properties

		protected internal virtual IServiceProvider ServiceProvider => _serviceProvider;

		#endregion

		#region Methods

		protected internal virtual LdapConnection CreateLdapConnection()
		{
			return new LdapConnection("x500.bund.de")
			{
				AuthType = AuthType.Anonymous,
				SessionOptions =
				{
					ProtocolVersion = 3
				}
			};
		}

		[TestMethod]
		public void Find_Extension_NamedDirectories_Test()
		{
			var directoryDictionary = this.ServiceProvider.GetRequiredService<IDictionary<string, IDirectory>>();

			Assert.AreEqual(3, directoryDictionary.Count);

			var expectedEntries = this.GetSearchResult(attributeList: "objectClass");
			var directoryEntry = directoryDictionary.ElementAt(0);
			Assert.AreEqual("Directory", directoryEntry.Key);
			Assert.AreEqual(expectedEntries.Count, directoryEntry.Value.Find().Count());

			expectedEntries = this.GetSearchResult(ldapFilter: "(objectClass=person)", attributeList: "objectClass");
			directoryEntry = directoryDictionary.ElementAt(1);
			Assert.AreEqual("Person-directory", directoryEntry.Key);
			Assert.AreEqual(expectedEntries.Count, directoryEntry.Value.Find().Count());

			expectedEntries = this.GetSearchResult(ldapFilter: "(objectClass=organizationalUnit)", attributeList: "objectClass");
			directoryEntry = directoryDictionary.ElementAt(2);
			Assert.AreEqual("Unit-directory", directoryEntry.Key);
			Assert.AreEqual(expectedEntries.Count, directoryEntry.Value.Find().Count());
		}

		[TestMethod]
		public void Find_Extension_NamedTypedDirectories_Test()
		{
			var directoryDictionary = this.ServiceProvider.GetRequiredService<IDictionary<string, IDirectory<IEntryMock>>>();

			Assert.AreEqual(3, directoryDictionary.Count);

			var expectedEntries = this.GetSearchResult(attributeList: "objectClass");
			var directoryEntry = directoryDictionary.ElementAt(0);
			Assert.AreEqual("Typed-directory", directoryEntry.Key);
			Assert.AreEqual(expectedEntries.Count, directoryEntry.Value.Find().Count());

			expectedEntries = this.GetSearchResult(ldapFilter: "(objectClass=person)", attributeList: "objectClass");
			directoryEntry = directoryDictionary.ElementAt(1);
			Assert.AreEqual("Typed-person-directory", directoryEntry.Key);
			Assert.AreEqual(expectedEntries.Count, directoryEntry.Value.Find().Count());

			expectedEntries = this.GetSearchResult(ldapFilter: "(objectClass=organizationalUnit)", attributeList: "objectClass");
			directoryEntry = directoryDictionary.ElementAt(2);
			Assert.AreEqual("Typed-unit-directory", directoryEntry.Key);
			Assert.AreEqual(expectedEntries.Count, directoryEntry.Value.Find().Count());
		}

		[TestMethod]
		public void Find_Extension_SingleDirectory_Test()
		{
			var expectedEntries = this.GetSearchResult(attributeList: "objectClass");

			var directory = this.ServiceProvider.GetRequiredService<IDirectory>();

			Assert.AreEqual(expectedEntries.Count, directory.Find().Count());
		}

		[TestMethod]
		public void Find_Extension_SingleTypedDirectory_Test()
		{
			var expectedEntries = this.GetSearchResult(attributeList: "objectClass");

			var directory = this.ServiceProvider.GetRequiredService<IDirectory<IEntryMock>>();

			Assert.AreEqual(expectedEntries.Count, directory.Find().Count());
		}

		[TestMethod]
		public void Find_SingleDirectory_Test()
		{
			var directory = this.ServiceProvider.GetRequiredService<IDirectory>();

			var entries = directory.Find(options =>
			{
				options.Attributes.Add("objectClass");
				options.FilterBuilder.Filters.Clear();
				options.FilterBuilder.Filters.Add("!objectClass=organizationalUnit");
				options.FilterBuilder.Filters.Add("!objectClass=person");
			}).ToArray();

			var expectedEntries = this.GetSearchResult(ldapFilter: "(&(!objectClass=organizationalUnit)(!objectClass=person))", attributeList: "objectClass");

			Assert.AreEqual(expectedEntries.Count, entries.Length);
			foreach(var entry in entries)
			{
				Assert.AreEqual(1, entry.Attributes.Count);
				var attribute = entry.Attributes["objectClass"];

				Assert.AreEqual("objectClass", attribute.Name);
				Assert.IsTrue(attribute.Count > 0);
				var values = attribute.GetValues(typeof(string)).Cast<string>().ToArray();
				Assert.IsTrue(values.Any());
				Assert.AreEqual(attribute.Count, values.Length);
			}
		}

		[TestMethod]
		public void Find_SingleTypedDirectory_Test()
		{
			var directory = this.ServiceProvider.GetRequiredService<IDirectory<IEntryMock>>();

			var entries = directory.Find(options =>
			{
				options.Attributes.Add("objectClass");
				options.FilterBuilder.Filters.Clear();
				options.FilterBuilder.Filters.Add("!objectClass=organizationalUnit");
				options.FilterBuilder.Filters.Add("!objectClass=person");
			}).ToArray();

			var expectedEntries = this.GetSearchResult(ldapFilter: "(&(!objectClass=organizationalUnit)(!objectClass=person))", attributeList: "objectClass");

			Assert.AreEqual(expectedEntries.Count, entries.Length);
			foreach(var entry in entries)
			{
				Assert.AreEqual(1, entry.Attributes.Count);
				var attribute = entry.Attributes["objectClass"];

				Assert.AreEqual("objectClass", attribute.Name);
				Assert.IsTrue(attribute.Count > 0);
				var values = attribute.GetValues(typeof(string)).Cast<string>().ToArray();
				Assert.IsTrue(values.Any());
				Assert.AreEqual(attribute.Count, values.Length);
				Assert.AreEqual(attribute.Count, entry.ObjectClasses.Count);
			}
		}

		protected internal virtual IList<SearchResultEntry> GetSearchResult(string distinguishedName = "o=Bund,c=DE", string ldapFilter = null, SearchScope searchScope = SearchScope.Subtree, params string[] attributeList)
		{
			SearchResponse searchResponse = null;
			var searchResult = new List<SearchResultEntry>();

			using(var ldapConnection = this.CreateLdapConnection())
			{
				ldapConnection.SessionOptions.ReferralChasing = ReferralChasingOptions.None;

				var pageResultRequestControl = new PageResultRequestControl(1000);

				var searchRequest = new SearchRequest(distinguishedName, ldapFilter, searchScope, attributeList);

				searchRequest.Controls.Add(pageResultRequestControl);

				while(searchResponse == null || pageResultRequestControl.Cookie.Length > 0)
				{
					searchResponse = (SearchResponse)ldapConnection.SendRequest(searchRequest);

					// ReSharper disable PossibleNullReferenceException
					var pageResultResponseControl = searchResponse.Controls.OfType<PageResultResponseControl>().FirstOrDefault();
					// ReSharper restore PossibleNullReferenceException

					if(pageResultResponseControl != null)
						pageResultRequestControl.Cookie = pageResultResponseControl.Cookie;

					searchResult.AddRange(searchResponse.Entries.Cast<SearchResultEntry>());
				}
			}

			return searchResult;
		}

		[ClassInitialize]
		public static void Initialize(TestContext testContext)
		{
			if(testContext == null)
				throw new ArgumentNullException(nameof(testContext));

			var services = new ServiceCollection();

			// Register single directory.
			services.AddDirectory(Global.Configuration);

			// Register single typed directory.
			services.AddDirectory<IEntryMock, TypedDirectoryMock>(Global.Configuration, "Typed-directory", "Directory");

			// Register named directories.
			services.AddDirectories(Global.Configuration);

			// Register named typed directories.
			services.AddDirectories<IEntryMock, TypedDirectoryMock>(
				Global.Configuration,
				"Typed-directories",
				(directoryOptionsFunction, connectionOptionsFunction, serviceProvider) =>
					new TypedDirectoryMock(new LdapConnectionFactory(connectionOptionsFunction), directoryOptionsFunction)
			);

			_serviceProvider = services.BuildServiceProvider();
		}

		[TestMethod]
		[SuppressMessage("Style", "IDE0004:Remove Unnecessary Cast")]
		public void Prerequisite_Test()
		{
			// ReSharper disable All
			using(var ldapConnection = this.CreateLdapConnection())
			{
				var searchRequest = new SearchRequest("o=Bund,c=DE", (string)null, SearchScope.Base);
				var searchResponse = (SearchResponse)ldapConnection.SendRequest(searchRequest);
				Assert.AreEqual(1, searchResponse.Entries.Count);
				Assert.AreEqual("o=Bund,c=DE", searchResponse.Entries[0].DistinguishedName);
				Assert.AreEqual(2, searchResponse.Entries[0].Attributes.Count);
				var attributeNames = searchResponse.Entries[0].Attributes.AttributeNames.Cast<string>();
				Assert.IsTrue(attributeNames.Contains("o"));
				Assert.IsTrue(attributeNames.Contains("objectclass"));
			}
			// ReSharper restore All
		}

		#endregion
	}
}