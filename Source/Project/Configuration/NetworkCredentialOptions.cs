using System.Text;

namespace RegionOrebroLan.DirectoryServices.Protocols.Configuration
{
	public class NetworkCredentialOptions : LdapConnectionStringOptions, ICloneable
	{
		#region Fields

		private const string _masking = "**********";

		#endregion

		#region Properties

		public virtual string? Domain { get; set; }
		public virtual bool? JoinDomainAndUserName { get; set; }
		public virtual string? Password { get; set; }
		protected internal override string Prefix => "Credential.";
		public virtual string? UserName { get; set; }

		#endregion

		#region Methods

		protected internal override void AddItemsToDictionary(IDictionary<string, string> dictionary)
		{
			if(this.Domain != null)
				this.AddItemToDictionary(dictionary, nameof(this.Domain), this.Domain);

			if(this.JoinDomainAndUserName != null)
				this.AddItemToDictionary(dictionary, nameof(this.JoinDomainAndUserName), this.JoinDomainAndUserName.Value.ToString().ToLowerInvariant());

			if(this.Password != null)
				this.AddItemToDictionary(dictionary, nameof(this.Password), _masking);

			if(this.UserName != null)
				this.AddItemToDictionary(dictionary, nameof(this.UserName), _masking);
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public virtual NetworkCredentialOptions Clone()
		{
			var memberwiseClone = (NetworkCredentialOptions)this.MemberwiseClone();

			var clone = new NetworkCredentialOptions
			{
				Domain = this.Domain == null ? null : new StringBuilder(this.Domain).ToString(),
				JoinDomainAndUserName = memberwiseClone.JoinDomainAndUserName,
				Password = this.Password == null ? null : new StringBuilder(this.Password).ToString(),
				UserName = this.UserName == null ? null : new StringBuilder(this.UserName).ToString()
			};

			return clone;
		}

		#endregion
	}
}