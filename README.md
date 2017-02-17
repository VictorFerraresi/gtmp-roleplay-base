# ProjetoRP
Projeto RP para desbancar o Trevizani, o famoso.

## Attention

Pushes to the `master` branch are protected by pull requests. Those will be accepted once a new feature is fully developed and tested. In order to keep your development progress updated within the remote repository, create a branch with any name format you may wish and push to it.

## Installing and running

1. Have Visual Studio 2017 installed

2. Clone the repository

   > https://help.github.com/articles/cloning-a-repository/

3. Open the solution and build it

4. Check `server/GTANetworkServer.exe.config` for correct MySQL connection info.

   It should look like this:
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
  </configSections>
    <connectionStrings>
      <add name="GameDb" providerName="MySql.Data.MySqlClient"
          connectionString="Server=YOUR_HOST;Database=YOUR_DB_NAME;Uid=USER_NAME;Pwd=USER_PASSWORD;"/>
    </connectionStrings>
    <entityFramework>
      <defaultConnectionFactory type="System.Data.Entity.Infrastructure.SqlConnectionFactory, EntityFramework"/>
      <providers>
        <provider invariantName="MySql.Data.MySqlClient"
            type="MySql.Data.MySqlClient.MySqlProviderServices, MySql.Data.Entity.EF6"/>
        <provider invariantName="System.Data.SqlClient"
            type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer"/>
      </providers>
    </entityFramework>
  </configuration>
  ```
5. Go to the Package Manager Console and run `Update_Database`. It should create all the schema.

6. Run `server/GTANetworkServer.exe` and see the results for yourself!

