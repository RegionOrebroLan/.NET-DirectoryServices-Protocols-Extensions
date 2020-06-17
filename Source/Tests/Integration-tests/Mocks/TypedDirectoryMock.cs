using System;
using System.DirectoryServices.Protocols;
using System.Linq;
using Microsoft.Extensions.Options;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace RegionOrebroLan.DirectoryServices.Protocols.IntegrationTests.Mocks
{
	public class TypedDirectoryMock : Directory<IEntryMock>
	{
		#region Constructors

		public TypedDirectoryMock(ILdapConnectionFactory connectionFactory, IOptions<DirectoryOptions> options) : base(connectionFactory, options) { }
		public TypedDirectoryMock(ILdapConnectionFactory connectionFactory, Func<DirectoryOptions> optionsFunction) : base(connectionFactory, optionsFunction) { }

		#endregion

		#region Methods

		protected internal override IEntryMock ConvertSearchResultEntry(SearchResultEntry searchResultEntry)
		{
			if(searchResultEntry == null)
				throw new ArgumentNullException(nameof(searchResultEntry));

			var entry = new EntryMock {DistinguishedName = searchResultEntry.DistinguishedName};

			// ReSharper disable AssignNullToNotNullAttribute
			foreach(var attributeName in searchResultEntry.Attributes.AttributeNames.Cast<string>())
			{
				entry.Attributes.Add(attributeName, (DirectoryAttributeWrapper) searchResultEntry.Attributes[attributeName]);
			}
			// ReSharper restore AssignNullToNotNullAttribute

			return entry;
		}

		#endregion
	}
}