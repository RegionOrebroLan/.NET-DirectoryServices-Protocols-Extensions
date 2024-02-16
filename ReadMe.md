# .NET-DirectoryServices-Protocols-Extensions

Additions and extensions for .NET directory-services protocols.

[![NuGet](https://img.shields.io/nuget/v/RegionOrebroLan.DirectoryServices.Protocols.svg?label=NuGet)](https://www.nuget.org/packages/RegionOrebroLan.DirectoryServices.Protocols)

## 1 Information

Don't think LDAPS (Secure Socket Layer) will work on Linux with this version.

### 1.1 Connection-string

- Example 1: **AuthenticationType=Basic;Credential.Domain=example;Credential.Password=P@ssword12;Credential.UserName=Alice;Identifier.Port=636;Identifier.ServerSeparator=|;Identifier.Servers=dc-01.example.org|dc-02.example.org|dc-03.example.org|dc-04.example.org;Session.ProtocolVersion=3;Session.SecureSocketLayer=true;Timeout=00:01:00**
- Example 2: **AuthenticationType=Basic;Credential.Domain=example;Credential.JoinDomainAndUserName=true;Credential.Password=P@ssword12;Credential.UserName=Alice;Identifier.Connectionless=false;Identifier.FullyQualifiedDnsHostName=true;Identifier.Port=636;Identifier.Servers=dc-01.example.org,dc-02.example.org,dc-03.example.org,dc-04.example.org;Session.AutoReconnect=true;Session.DomainName=example.org;Session.HostName=host.example.org;Session.Locators=DirectoryServicesPreferred|GCRequired|PdcRequired|KdcRequired|OnlyLdapNeeded|ReturnDnsName;Session.PingKeepAliveTimeout=00:10:00;Session.PingLimit=10;Session.PingWaitTimeout=00:00:10;Session.ProtocolVersion=3;Session.ReferralChasing=All;Session.RootDseCache=false;Session.Sealing=true;Session.SecureSocketLayer=true;Session.Signing=false;Session.Sspi=20;Session.TcpKeepAlive=false;Timeout=00:05:00**

### 1.2 Links

- [Introduction to System.DirectoryServices.Protocols (S.DS.P)](https://docs.microsoft.com/en-us/previous-versions/dotnet/articles/bb332056%28v=msdn.10%29)

## 2 Development

### 2.1 Signing

Drop the "StrongName.snk" file in the repository-root. The file should not be included in source control.