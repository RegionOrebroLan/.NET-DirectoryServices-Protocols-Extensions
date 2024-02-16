using System.Text;

namespace RegionOrebroLan.DirectoryServices.Protocols.Configuration
{
	public class LdapDirectoryIdentifierOptions : LdapConnectionStringOptions, ICloneable
	{
		#region Fields

		private char? _serverSeparator;

		#endregion

		#region Properties

		public virtual bool? Connectionless { get; set; }
		protected internal virtual char DefaultServerSeparator => ',';
		public virtual bool? FullyQualifiedDnsHostName { get; set; }
		public virtual int? Port { get; set; }
		protected internal override string Prefix => "Identifier.";
		public virtual ISet<string> Servers { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// If we run an application in Kubernetes/OpenShift we may not be able to delimit/separate with a comma, ",".
		/// We may have to delimit/separate with e.g. a pipe, "|", instead.
		/// If we use a comma, ",", we have problems when we use "oc process": ##[error]warning: --param no longer accepts comma-separated lists of values. "CONNECTION_STRING=***" will be treated as a single key-value pair.
		/// </summary>
		public virtual char ServerSeparator
		{
			get
			{
				this._serverSeparator ??= this.DefaultServerSeparator;

				return this._serverSeparator.Value;
			}
			set => this._serverSeparator = value;
		}

		#endregion

		#region Methods

		protected internal override void AddItemsToDictionary(IDictionary<string, string> dictionary)
		{
			if(this.Connectionless != null)
				this.AddItemToDictionary(dictionary, nameof(this.Connectionless), this.Connectionless.Value.ToString().ToLowerInvariant());

			if(this.FullyQualifiedDnsHostName != null)
				this.AddItemToDictionary(dictionary, nameof(this.FullyQualifiedDnsHostName), this.FullyQualifiedDnsHostName.Value.ToString().ToLowerInvariant());

			if(this.Port != null)
				this.AddItemToDictionary(dictionary, nameof(this.Port), this.Port.Value.ToString((IFormatProvider?)null));

			if(this.Servers.Any())
				this.AddItemToDictionary(dictionary, nameof(this.Servers), string.Join(this.ServerSeparator.ToString(), this.Servers));

			if(this.ServerSeparator != this.DefaultServerSeparator)
				this.AddItemToDictionary(dictionary, nameof(this.ServerSeparator), this.ServerSeparator.ToString());
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public virtual LdapDirectoryIdentifierOptions Clone()
		{
			var memberwiseClone = (LdapDirectoryIdentifierOptions)this.MemberwiseClone();

			var clone = new LdapDirectoryIdentifierOptions
			{
				Connectionless = memberwiseClone.Connectionless,
				FullyQualifiedDnsHostName = memberwiseClone.FullyQualifiedDnsHostName,
				Port = memberwiseClone.Port,
				ServerSeparator = memberwiseClone.ServerSeparator
			};

			clone.Servers.Clear();

			foreach(var server in memberwiseClone.Servers)
			{
				clone.Servers.Add(new StringBuilder(server).ToString());
			}

			return clone;
		}

		#endregion
	}
}