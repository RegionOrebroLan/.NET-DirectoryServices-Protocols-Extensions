# .NET-DirectoryServices-Protocols-Extensions

Additions and extensions for .NET directory-services protocols.

[![NuGet](https://img.shields.io/nuget/v/RegionOrebroLan.DirectoryServices.Protocols.svg?label=NuGet)](https://www.nuget.org/packages/RegionOrebroLan.DirectoryServices.Protocols)

## 1 Examples

### 1.1 Configuration

- [AppSettings.json](/Source/Tests/Integration-tests/AppSettings.json)
- [ConfigureServices](/Source/Tests/Integration-tests/DirectoryTest.cs#L142)

## 2 Development

### 2.1 Integration-tests

The integration-tests uses a public ldap-server "x500.bund.de". You may need to disconnect your VPN-connection to your company when running the integrations-tests. If you are on VPN you may get:

- System.DirectoryServices.Protocols.LdapException: The LDAP server is unavailable.

Your company may lock traffic to ldap-ports 389 and 636.

## 3 Information

- [Introduction to System.DirectoryServices.Protocols (S.DS.P)](https://docs.microsoft.com/en-us/previous-versions/dotnet/articles/bb332056(v=msdn.10))