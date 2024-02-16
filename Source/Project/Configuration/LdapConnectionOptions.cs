using System.DirectoryServices.Protocols;

namespace RegionOrebroLan.DirectoryServices.Protocols.Configuration
{
	public class LdapConnectionOptions : LdapConnectionStringOptions, ICloneable
	{
		#region Properties

		public virtual AuthType? AuthenticationType { get; set; }
		public virtual bool? AutomaticBind { get; set; }
		public virtual NetworkCredentialOptions Credential { get; set; } = new();
		public virtual LdapDirectoryIdentifierOptions Identifier { get; set; } = new();
		public virtual LdapSessionOptions Session { get; set; } = new();
		public virtual TimeSpan? Timeout { get; set; }

		#endregion

		#region Methods

		protected internal override void AddItemsToDictionary(IDictionary<string, string> dictionary)
		{
			if(this.AuthenticationType != null)
				this.AddItemToDictionary(dictionary, nameof(this.AuthenticationType), this.AuthenticationType.Value.ToString());

			if(this.AutomaticBind != null)
				this.AddItemToDictionary(dictionary, nameof(this.AutomaticBind), this.AutomaticBind.Value.ToString().ToLowerInvariant());

			this.Credential.AddItemsToDictionary(dictionary);
			this.Identifier.AddItemsToDictionary(dictionary);
			this.Session.AddItemsToDictionary(dictionary);

			if(this.Timeout != null)
				this.AddItemToDictionary(dictionary, nameof(this.Timeout), this.Timeout.Value.ToString());
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public virtual LdapConnectionOptions Clone()
		{
			var memberwiseClone = (LdapConnectionOptions)this.MemberwiseClone();

			var clone = new LdapConnectionOptions
			{
				AuthenticationType = memberwiseClone.AuthenticationType,
				AutomaticBind = memberwiseClone.AutomaticBind,
				Credential = this.Credential.Clone(),
				Identifier = this.Identifier.Clone(),
				Session = this.Session.Clone(),
				Timeout = memberwiseClone.Timeout
			};

			return clone;
		}

		#endregion
	}
}