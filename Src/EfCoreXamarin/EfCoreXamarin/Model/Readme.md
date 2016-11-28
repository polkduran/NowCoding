Add portable project
Transform to Netstandard 1.4 (https://docs.nuget.org/ndocs/guides/create-net-standard-packages-vs2015)
Create model
Add packages:
    Microsoft.EntityFrameworkCore 1.1.0
    Microsoft.EntityFrameworkCore.Sqlite 1.1.0
Create DbContext
	override OnConfiguring
	use Sqlite
Add .NET 4.6.1 Console app 
Reference Model project
Add packages:
	Microsoft.EntityFrameworkCore.Tools 1.1.0-preview4-final
	Microsoft.EntityFrameworkCore.Sqlite 1.1.0
Generate the initial migration
	Add-Migration InitialMigration -OutputDir ../Model/Migrations -Project Model -StartupProject MigrationEntryPoint
Ohter Migrations
	- AddAnimalFeatures
	- AddPersonName
cf MigrationEntryPoint.Program to:
	- Db creation/Migration
	- data insertion
Add Blank Xaml App (Xamarin.Forms Portable)
Pass Client to Netstandard
	- Error when doing it by the properties page
	- https://oren.codes/2016/02/08/project-json-all-the-things/
		- delete packages.config 
		- Remove all references to 'package' in csproj
		- Retry to pass it by the properties page
	- Add Xamarin.Forms package
Remove Windows and IOS projects
Reference Model on Client
Set the db folder (droid and forms)
In Droid
Update packages if needed (Forms)
Add packages:
	Microsoft.EntityFrameworkCore 1.1.0
	Microsoft.EntityFrameworkCore.Sqlite 1.1.0

