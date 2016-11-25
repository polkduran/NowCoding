Add portable project
Transform to Netstandard 1.4
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