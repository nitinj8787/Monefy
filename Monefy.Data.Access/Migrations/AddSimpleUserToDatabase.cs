using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Monefy.Data.Access.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using Monefy.Api.Common.Extentions;

namespace Monefy.Data.Access.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("CustomMigration_002_AddSimpleUserToDatabase")]
    public class AddSimpleUserToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var userPassword = "user".WithBCrypt();
            migrationBuilder.Sql($@"
DECLARE @password NVARCHAR(MAX) = N'{userPassword}'

INSERT INTO dbo.Users
        ( FirstName ,
          LastName ,
          Password ,
          Username ,
          IsDeleted
        )
VALUES  ( N'user' , -- FirstName - nvarchar(max)
          N'user' , -- LastName - nvarchar(max)
          @password, -- Password - nvarchar(max)
          N'user' , -- Username - nvarchar(max)
          0  -- IsDeleted - bit
        )

DECLARE @adminId INT = @@IDENTITY

INSERT INTO dbo.UserRoles
        ( RoleId, UserId )
SELECT Id, @adminId FROM dbo.Roles
WHERE Name = 'User'
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE UR
FROM dbo.Users U
JOIN dbo.UserRoles UR ON UR.UserId = U.Id
WHERE U.Username = 'user'

DELETE U
FROM dbo.Users U
WHERE U.Username = 'user'
");
        }
    }
}