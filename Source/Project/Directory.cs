using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using Microsoft.Extensions.Options;
using RegionOrebroLan.DirectoryServices.Protocols.Configuration;

namespace RegionOrebroLan.DirectoryServices.Protocols
{
	public class Directory : Directory<IEntry>, IDirectory
	{
		#region Constructors

		[CLSCompliant(false)]
		public Directory(ILdapConnectionFactory connectionFactory, Func<DirectoryOptions> optionsFunction) : base(connectionFactory, optionsFunction) { }

		[CLSCompliant(false)]
		public Directory(ILdapConnectionFactory connectionFactory, IOptionsMonitor<DirectoryOptions> optionsMonitor) : base(connectionFactory, optionsMonitor) { }

		#endregion

		#region Methods

		protected internal override IEntry ConvertSearchResultEntry(SearchResultEntry searchResultEntry)
		{
			if(searchResultEntry == null)
				throw new ArgumentNullException(nameof(searchResultEntry));

			var entry = new Entry { DistinguishedName = searchResultEntry.DistinguishedName };

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

	public abstract class Directory<TEntry> : IDirectory<TEntry>
	{
		#region Constructors

		private Directory(ILdapConnectionFactory connectionFactory)
		{
			this.ConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
		}

		[CLSCompliant(false)]
		protected Directory(ILdapConnectionFactory connectionFactory, Func<DirectoryOptions> optionsFunction) : this(connectionFactory)
		{
			this.OptionsFunction = optionsFunction ?? throw new ArgumentNullException(nameof(optionsFunction));
		}

		[CLSCompliant(false)]
		protected Directory(ILdapConnectionFactory connectionFactory, IOptionsMonitor<DirectoryOptions> optionsMonitor) : this(connectionFactory)
		{
			if(optionsMonitor == null)
				throw new ArgumentNullException(nameof(optionsMonitor));

			this.OptionsFunction = () => optionsMonitor.CurrentValue;
		}

		#endregion

		#region Properties

		protected internal virtual ILdapConnectionFactory ConnectionFactory { get; }

		[CLSCompliant(false)]
		protected internal virtual Func<DirectoryOptions> OptionsFunction { get; }

		#endregion

		#region Methods

		protected internal abstract TEntry ConvertSearchResultEntry(SearchResultEntry searchResultEntry);

		protected internal virtual LdapConnection CreateConnection()
		{
			return this.ConnectionFactory.Create();
		}

		protected internal virtual FindOptions CreateFindOptions()
		{
			var options = this.OptionsFunction();

			var findOptions = new FindOptions(this.CreateConnection())
			{
				SearchScope = SearchScope.Subtree,
				RootDistinguishedName = options.RootDistinguishedName
			};

			if(!string.IsNullOrWhiteSpace(options.BaseFilter))
				findOptions.FilterBuilder.Filters.Add(options.BaseFilter);

			// ReSharper disable InvertIf
			if(options.Paging != null)
			{
				if(options.Paging.Enabled != null)
					findOptions.Paging.Enabled = options.Paging.Enabled.Value;

				if(options.Paging.PageSize != null)
					findOptions.Paging.PageSize = options.Paging.PageSize.Value;
			}
			// ReSharper restore InvertIf

			return findOptions;
		}

		public virtual IEnumerable<TEntry> Find(Action<IFindOptions> configureOptions)
		{
			if(configureOptions == null)
				throw new ArgumentNullException(nameof(configureOptions));

			return this.Find(configureOptions, this.CreateFindOptions());
		}

		protected internal virtual IEnumerable<TEntry> Find(Action<IFindOptions> configureOptions, IFindOptions options)
		{
			if(configureOptions == null)
				throw new ArgumentNullException(nameof(configureOptions));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			configureOptions(options);

			var entries = new List<TEntry>();

			using(var connection = options.Connection)
			{
				var searchResult = this.GetSearchResult(connection, options);

				this.PopulateFindResult(entries, searchResult);
			}

			return entries.ToArray();
		}

		protected internal virtual IEnumerable<SearchResultEntry> GetSearchResult(LdapConnection connection, IFindOptions options)
		{
			if(connection == null)
				throw new ArgumentNullException(nameof(connection));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var searchResult = new List<SearchResultEntry>();

			var searchRequest = new SearchRequest(options.RootDistinguishedName, options.FilterBuilder.Build(), options.SearchScope, options.Attributes.ToArray());
			SearchResponse searchResponse = null;

			if(options.Paging.Enabled)
			{
				connection.SessionOptions.ReferralChasing = ReferralChasingOptions.None;

				var pageResultRequestControl = new PageResultRequestControl(options.Paging.PageSize);

				searchRequest.Controls.Add(pageResultRequestControl);

				while(searchResponse == null || pageResultRequestControl.Cookie.Length > 0)
				{
					searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

					// ReSharper disable PossibleNullReferenceException
					var pageResultResponseControl = searchResponse.Controls.OfType<PageResultResponseControl>().FirstOrDefault();
					// ReSharper restore PossibleNullReferenceException

					if(pageResultResponseControl != null)
						pageResultRequestControl.Cookie = pageResultResponseControl.Cookie;

					searchResult.AddRange(searchResponse.Entries.Cast<SearchResultEntry>());
				}
			}
			else
			{
				searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

				// ReSharper disable PossibleNullReferenceException
				searchResult.AddRange(searchResponse.Entries.Cast<SearchResultEntry>());
				// ReSharper restore PossibleNullReferenceException
			}

			return searchResult;
		}

		protected internal virtual void PopulateFindResult(IList<TEntry> entries, IEnumerable<SearchResultEntry> searchResult)
		{
			if(entries == null)
				throw new ArgumentNullException(nameof(entries));

			if(searchResult == null)
				throw new ArgumentNullException(nameof(searchResult));

			foreach(var searchResultEntry in searchResult)
			{
				entries.Add(this.ConvertSearchResultEntry(searchResultEntry));
			}
		}

		#endregion
	}
}