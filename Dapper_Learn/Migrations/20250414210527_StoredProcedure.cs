using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dapper_Learn.Migrations
{
    /// <inheritdoc />
    public partial class StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.Sql(@"
             CREATE PROC usp_GetCompany
                @CompanyId INT
                AS
                BEGIN
                SELECT * FROM Companies WHERE CompanyId = @CompanyId
                END
                GO
                                ");

            migrationBuilder.Sql(@"
            CREATE PROC usp_GetAllCompanies
            AS
            BEGIN
            SELECT * FROM Companies
            END
            GO
            ");

            migrationBuilder.Sql(@"
            CREATE PROC usp_AddCompany
            @CompanyId INT OUTPUT,
            @Name NVARCHAR(MAX),
            @Address NVARCHAR(MAX),
            @City NVARCHAR(MAX),
            @State NVARCHAR(MAX),
            @PostalCode NVARCHAR(MAX)
            AS
            BEGIN
            INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES (@Name, @Address, @City, @State, @PostalCode);
            SELECT @CompanyId = SCOPE_IDENTITY()
            END
            GO
            ");

            migrationBuilder.Sql(@"
            CREATE PROC usp_UpdateCompany
            @CompanyId INT,
            @Name NVARCHAR(MAX),
            @Address NVARCHAR(MAX),
            @City NVARCHAR(MAX),
            @State NVARCHAR(MAX),
            @PostalCode NVARCHAR(MAX)
            AS
            BEGIN
            UPDATE Companies
            SET Name = @Name, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode
            WHERE CompanyId = @CompanyId
            END
            GO
            ");

            migrationBuilder.Sql(@"
            CREATE PROC usp_DeleteCompany
            @CompanyId INT
            AS
            BEGIN
            DELETE FROM Companies WHERE CompanyId = @CompanyId
            END
            GO
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
