using System.DirectoryServices.Protocols;
using Microsoft.Extensions.Options;
using RegionOrebroLan.DirectoryServices.Protocols;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace IntegrationTests.Mocks
{
	public class TypedDirectoryMock : Directory<IEntryMock>
	{
		#region Constructors

		public TypedDirectoryMock(ILdapConnectionFactory connectionFactory, Func<DirectoryOptions> optionsFunction) : base(connectionFactory, optionsFunction) { }
		public TypedDirectoryMock(ILdapConnectionFactory connectionFactory, IOptionsMonitor<DirectoryOptions> optionsMonitor) : base(connectionFactory, optionsMonitor) { }

		#endregion

		#region Methods

		protected internal override IEntryMock ConvertSearchResultEntry(SearchResultEntry searchResultEntry)
		{
			if(searchResultEntry == null)
				throw new ArgumentNullException(nameof(searchResultEntry));

			var entry = new EntryMock { DistinguishedName = searchResultEntry.DistinguishedName };

			// ReSharper disable AssignNullToNotNullAttribute
			foreach(var attributeName in searchResultEntry.Attributes.AttributeNames.Cast<string>())
			{
				entry.Attributes.Add(attributeName, (DirectoryAttributeWrapper)searchResultEntry.Attributes[attributeName]);
			}
			// ReSharper restore AssignNullToNotNullAttribute

			return entry;
		}

		#endregion
	}
}