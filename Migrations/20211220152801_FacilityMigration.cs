using Microsoft.EntityFrameworkCore.Migrations;

namespace test_app.Migrations
{
    public partial class FacilityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FacilityStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facility_FacilityStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FacilityStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

          
            migrationBuilder.CreateIndex(
                name: "IX_Facility_StatusId",
                table: "Facility",
                column: "StatusId");


      
            migrationBuilder.InsertData(
                table: "FacilityStatus",
            columns: new[] {"Id" ,"Name" },
            values: new object[,]
            {
                { "1","Active" },
                { "2","Inactive" },
                { "3","OnHold" }
        });



            migrationBuilder.Sql(@"    CREATE PROCEDURE GetCountOfFacilities 
                                   @itemsCount INTEGER OUTPUT
                                       AS
                                           SELECT @itemsCount = COUNT(*) FROM Facility;
                                       RETURN 0;
                           "
  );


       migrationBuilder.Sql(@"CREATE PROCEDURE GetFacilityPage
                                   @page    INTEGER,
                                   @perPage INTEGER
                                   AS
                                       SELECT * FROM Facility
                                       ORDER BY Facility.Id
                                               OFFSET ( @page * @perPage ) ROWS FETCH NEXT @perPage ROWS ONLY;
                                   
       "
           );


            //        LEFT JOIN FacilityStatus ON Facility.StatusId = FacilityStatus.Id


        }

        /*
         *    
         *   
         * 
         */

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "FacilityStatus");

            migrationBuilder.Sql(@"DROP PROCEDURE GetCountOfFacilities;");
            migrationBuilder.Sql(@"DROP PROCEDURE GetFacilityPage;");
        }
    }
}
