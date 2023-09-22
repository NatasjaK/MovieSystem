using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieSystem.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LikedGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Movie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikedGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikedGenres_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Explore futuristic worlds, advanced technology, and the impact of scientific advancements on society and humanity.", "Science Fiction" },
                    { 2, "Immerse yourself in magical realms, mythical creatures, and epic adventures beyond the boundaries of reality.", "Fantasy" },
                    { 3, "Solve puzzles, unravel enigmatic plots, and uncover hidden secrets in thrilling and suspenseful narratives.", "Mystery" },
                    { 4, "Experience the passion, love, and emotional rollercoaster of relationships and romantic encounters.", "Romance" },
                    { 5, "Face your deepest fears as you confront supernatural entities, psychological terrors, and the darkest aspects of human nature.", "Horror" },
                    { 6, "Embark on daring quests, explore uncharted territories, and engage in thrilling escapades across various landscapes.", "Adventure" },
                    { 7, "Travel through time and delve into the past, experiencing historical events and periods through the eyes of fictional characters.", "Historical Fiction" },
                    { 8, "Experience suspense, tension, and high-stakes situations in fast-paced narratives that keep you on the edge of your seat.", "Thriller" },
                    { 9, "Find humor in everyday situations, witty dialogues, and humorous characters that make you laugh out loud.", "Comedy" },
                    { 10, "Explore complex emotions, interpersonal relationships, and the human condition in emotionally charged and thought-provoking stories.", "Drama" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikedGenres_GenreId",
                table: "LikedGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedGenres_PersonId",
                table: "LikedGenres",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikedGenres");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
