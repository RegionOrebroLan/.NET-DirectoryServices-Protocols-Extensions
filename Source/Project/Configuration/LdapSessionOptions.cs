using System.DirectoryServices.Protocols;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RegionOrebroLan.DirectoryServices.Protocols.Configuration
{
	public class LdapSessionOptions : LdapConnectionStringOptions, ICloneable
	{
		#region Fields

		private char? _flagSeparator;

		#endregion

		#region Properties

		public virtual bool? AutoReconnect { get; set; }
		protected internal virtual char DefaultFlagSeparator => '|';
		public virtual string? DomainName { get; set; }

		public virtual char FlagSeparator
		{
			get
			{
				this._flagSeparator ??= this.DefaultFlagSeparator;

				return this._flagSeparator.Value;
			}
			set => this._flagSeparator = value;
		}

		public virtual string? HostName { get; set; }
		public virtual LocatorFlags? Locators { get; set; }
		public virtual TimeSpan? PingKeepAliveTimeout { get; set; }
		public virtual int? PingLimit { get; set; }
		public virtual TimeSpan? PingWaitTimeout { get; set; }
		protected internal override string Prefix => "Session.";
		public virtual int? ProtocolVersion { get; set; }
		public virtual Func<LdapConnection, byte[][], X509Certificate>? QueryClientCertificateFunction { get; set; }
		public virtual ReferralCallback? ReferralCallback { get; set; }
		public virtual ReferralChasingOptions? ReferralChasing { get; set; }
		public virtual int? ReferralHopLimit { get; set; }
		public virtual bool? RootDseCache { get; set; }
		public virtual string? SaslMethod { get; set; }
		public virtual bool? Sealing { get; set; }
		public virtual bool? SecureSocketLayer { get; set; }
		public virtual TimeSpan? SendTimeout { get; set; }
		public virtual bool? Signing { get; set; }
		public virtual int? Sspi { get; set; }
		public virtual bool? TcpKeepAlive { get; set; }
		public virtual bool? TransportLayerSecurity { get; set; }
		public virtual Func<LdapConnection, X509Certificate, bool>? VerifyServerCertificateFunction { get; set; }

		#endregion

		#region Methods

		protected internal override void AddItemsToDictionary(IDictionary<string, string> dictionary)
		{
			if(this.AutoReconnect != null)
				this.AddItemToDictionary(dictionary, nameof(this.AutoReconnect), this.AutoReconnect.Value.ToString().ToLowerInvariant());

			if(this.DomainName != null)
				this.AddItemToDictionary(dictionary, nameof(this.DomainName), this.DomainName);

			if(this.FlagSeparator != this.DefaultFlagSeparator)
				this.AddItemToDictionary(dictionary, nameof(this.FlagSeparator), this.FlagSeparator.ToString());

			if(this.HostName != null)
				this.AddItemToDictionary(dictionary, nameof(this.HostName), this.HostName);

			if(this.Locators != null)
				this.AddItemToDictionary(dictionary, nameof(this.Locators), this.ToFlagString(this.Locators));

			if(this.PingKeepAliveTimeout != null)
				this.AddItemToDictionary(dictionary, nameof(this.PingKeepAliveTimeout), this.PingKeepAliveTimeout.Value.ToString());

			if(this.PingLimit != null)
				this.AddItemToDictionary(dictionary, nameof(this.PingLimit), this.PingLimit.Value.ToString((IFormatProvider?)null));

			if(this.PingWaitTimeout != null)
				this.AddItemToDictionary(dictionary, nameof(this.PingWaitTimeout), this.PingWaitTimeout.Value.ToString());

			if(this.ProtocolVersion != null)
				this.AddItemToDictionary(dictionary, nameof(this.ProtocolVersion), this.ProtocolVersion.Value.ToString((IFormatProvider?)null));

			if(this.ReferralChasing != null)
				this.AddItemToDictionary(dictionary, nameof(this.ReferralChasing), this.ToFlagString(this.ReferralChasing));

			if(this.ReferralHopLimit != null)
				this.AddItemToDictionary(dictionary, nameof(this.ReferralHopLimit), this.ReferralHopLimit.Value.ToString((IFormatProvider?)null));

			if(this.RootDseCache != null)
				this.AddItemToDictionary(dictionary, nameof(this.RootDseCache), this.RootDseCache.Value.ToString().ToLowerInvariant());

			if(this.SaslMethod != null)
				this.AddItemToDictionary(dictionary, nameof(this.SaslMethod), this.SaslMethod);

			if(this.Sealing != null)
				this.AddItemToDictionary(dictionary, nameof(this.Sealing), this.Sealing.Value.ToString().ToLowerInvariant());

			if(this.SecureSocketLayer != null)
				this.AddItemToDictionary(dictionary, nameof(this.SecureSocketLayer), this.SecureSocketLayer.Value.ToString().ToLowerInvariant());

			if(this.SendTimeout != null)
				this.AddItemToDictionary(dictionary, nameof(this.SendTimeout), this.SendTimeout.Value.ToString());

			if(this.Signing != null)
				this.AddItemToDictionary(dictionary, nameof(this.Signing), this.Signing.Value.ToString().ToLowerInvariant());

			if(this.Sspi != null)
				this.AddItemToDictionary(dictionary, nameof(this.Sspi), this.Sspi.Value.ToString((IFormatProvider?)null));

			if(this.TcpKeepAlive != null)
				this.AddItemToDictionary(dictionary, nameof(this.TcpKeepAlive), this.TcpKeepAlive.Value.ToString().ToLowerInvariant());

			if(this.TransportLayerSecurity != null)
				this.AddItemToDictionary(dictionary, nameof(this.TransportLayerSecurity), this.TransportLayerSecurity.Value.ToString().ToLowerInvariant());
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public virtual LdapSessionOptions Clone()
		{
			var memberwiseClone = (LdapSessionOptions)this.MemberwiseClone();

			var clone = new LdapSessionOptions
			{
				AutoReconnect = memberwiseClone.AutoReconnect,
				DomainName = this.DomainName == null ? null : new StringBuilder(this.DomainName).ToString(),
				FlagSeparator = memberwiseClone.FlagSeparator,
				HostName = this.HostName == null ? null : new StringBuilder(this.HostName).ToString(),
				Locators = memberwiseClone.Locators,
				PingKeepAliveTimeout = memberwiseClone.PingKeepAliveTimeout,
				PingLimit = memberwiseClone.PingLimit,
				PingWaitTimeout = memberwiseClone.PingWaitTimeout,
				ProtocolVersion = memberwiseClone.ProtocolVersion,
				ReferralChasing = memberwiseClone.ReferralChasing,
				ReferralHopLimit = memberwiseClone.ReferralHopLimit,
				RootDseCache = memberwiseClone.RootDseCache,
				SaslMethod = this.SaslMethod == null ? null : new StringBuilder(this.SaslMethod).ToString(),
				Sealing = memberwiseClone.Sealing,
				SecureSocketLayer = memberwiseClone.SecureSocketLayer,
				SendTimeout = memberwiseClone.SendTimeout,
				Signing = memberwiseClone.Signing,
				Sspi = memberwiseClone.Sspi,
				TcpKeepAlive = memberwiseClone.TcpKeepAlive,
				TransportLayerSecurity = memberwiseClone.TransportLayerSecurity
			};

			return clone;
		}

		protected internal virtual string ToFlagString(Enum flag)
		{
			return flag.ToString().Replace(" ", string.Empty).Replace(',', this.FlagSeparator);
		}

		#endregion
	}
}