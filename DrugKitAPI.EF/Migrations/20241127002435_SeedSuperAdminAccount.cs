using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugKitAPI.EF.Migrations
{
    /// <inheritdoc />
    public partial class SeedSuperAdminAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Hash the password for the new SuperAdmin user
            var hasher = new PasswordHasher<object>();
            string hashedPassword = hasher.HashPassword(null, "P@ssword12");

            // Insert the SuperAdmin user into AspNetUsers
            var insertUserQuery = $@"
                INSERT INTO AspNetUsers 
                (Id, UserName, Name, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
                VALUES 
                (NEWID(), 'superadmin', 'Sherif Ibrahim Hassan', 'SUPERADMIN', 'sherif.super-admin@drugkit.com', 'SHERIF.SUPER-ADMIN@DRUGKIT.COM', 1, '{hashedPassword}', NEWID(), NEWID(), 0, 0, 1, 0);

                -- Fetch the Id of the newly inserted user
                DECLARE @UserId NVARCHAR(450);
                SELECT TOP 1 @UserId = Id FROM AspNetUsers WHERE UserName = 'superadmin';

                -- Dynamically fetch the RoleId for SuperAdmin
                DECLARE @RoleId NVARCHAR(450);
                SELECT TOP 1 @RoleId = Id FROM AspNetRoles WHERE Name = 'SuperAdmin';

                -- Assign the user to the SuperAdmin role if RoleId is found
                IF @RoleId IS NOT NULL
                BEGIN
                    INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);
                END

                -- Insert the user into SystemAdmins if UserId is found
                IF @UserId IS NOT NULL
                BEGIN
                    INSERT INTO SystemAdmins (ApplicationUserId) VALUES (@UserId);
                END
            ";

            // Execute the query
            migrationBuilder.Sql(insertUserQuery);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the SuperAdmin user and associated data
            var deleteUserQuery = @"
                DECLARE @UserId NVARCHAR(450);
                SELECT TOP 1 @UserId = Id FROM AspNetUsers WHERE UserName = 'superadmin';

                -- Delete from SystemAdmins
                DELETE FROM SystemAdmins WHERE ApplicationUserId = @UserId;

                -- Remove the user from AspNetUserRoles
                DELETE FROM AspNetUserRoles WHERE UserId = @UserId;

                -- Delete the user from AspNetUsers
                DELETE FROM AspNetUsers WHERE Id = @UserId;
            ";

            migrationBuilder.Sql(deleteUserQuery);
        }
    }
}