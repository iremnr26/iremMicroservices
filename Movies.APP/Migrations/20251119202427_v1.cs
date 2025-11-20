using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Movies.APP.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Id", "FirstName", "Guid", "IsRetired", "LastName" },
                values: new object[,]
                {
                    { 1, "Christopher", "cf873ce4-5527-4494-9df6-5dd93574d4f4", false, "Nolan" },
                    { 2, "Steven", "af661299-9c85-4a9a-8b48-dbf4843513ba", false, "Spielberg" },
                    { 3, "James", "ffb1027a-5415-46bc-a534-e35acb3b7169", false, "Cameron" },
                    { 4, "Quentin", "ef611825-04be-4242-824f-3d6e41b99c42", false, "Tarantino" },
                    { 5, "David", "e826c69b-6afb-43bc-a131-f158e3097d8e", false, "Fincher" },
                    { 6, "Peter", "4d1f4159-af23-42c4-b14e-5866071aeb9e", false, "Jackson" },
                    { 7, "Denis", "c0955c56-cd52-4ec4-b94d-3fde2d2fc432", false, "Villeneuve" },
                    { 8, "Jordan", "a8273e21-daef-4777-97b3-995ba122e5d7", false, "Peele" },
                    { 9, "Martin", "b480cc2f-20a6-4a06-8a4c-50afe5fdf81e", false, "Scorsese" },
                    { 10, "Ridley", "b8ca1786-f076-4d33-acad-dd0808daaa89", false, "Scott" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Guid", "Name" },
                values: new object[,]
                {
                    { 1, "b4b0ed87-5195-4d75-a76e-59598d590555", "Sci-Fi" },
                    { 2, "5238fe8a-84f0-4a9c-8fe5-d96b0f699e5e", "Drama" },
                    { 3, "f1e6f6c5-46ed-4bda-85ad-30fbdfbef2a3", "Thriller" },
                    { 4, "c710ec12-31aa-4102-a8a5-c7a08e95ed9a", "Adventure" },
                    { 5, "317c96b6-a3f0-4c83-80f4-4da6fc76ef34", "Action" },
                    { 6, "cd32223e-2f9f-4a9f-aef6-bf65ff57a226", "Fantasy" },
                    { 7, "d2b20344-03b5-4605-b2eb-7e6f0f29af02", "Mystery" },
                    { 8, "986e452d-cfcc-47eb-996f-342f85c827f2", "Comedy" },
                    { 9, "cf02985a-b6b2-4641-bc1d-27aa7565b8ff", "Horror" },
                    { 10, "149edf5d-6eb1-4b0e-a95a-220fb18afcbd", "Crime" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "DirectorId", "Guid", "Name", "ReleaseDate", "TotalRevenue" },
                values: new object[,]
                {
                    { 1, 1, "cf1353b4-5e86-4336-b9b7-3ec4d64f8765", "Inception", new DateTime(2010, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 829000000m },
                    { 2, 1, "8a87e06c-6a51-45f4-ab40-db658cfc357f", "Interstellar", new DateTime(2014, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 677000000m },
                    { 3, 3, "d7348dd2-21d8-430e-a3db-31303499a6f7", "Avatar", new DateTime(2009, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2923000000m },
                    { 4, 4, "d561b1de-9163-4964-a58a-eda32e4c0ac1", "Pulp Fiction", new DateTime(1994, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 214000000m },
                    { 5, 5, "220c718d-4058-42c6-a7a0-a5c17bb3a9fb", "Fight Club", new DateTime(1999, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 101000000m },
                    { 6, 6, "623cc0e4-ae12-4049-b90f-220d97b31001", "LOTR: Fellowship", new DateTime(2001, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 880000000m },
                    { 7, 8, "349c03d4-4fa3-459a-acd7-2c34f50327f7", "Get Out", new DateTime(2017, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 255000000m },
                    { 8, 9, "84a6e440-9a91-4567-9ffb-7146eba94f8e", "Shutter Island", new DateTime(2010, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 295000000m },
                    { 9, 10, "86d5e0fc-f897-4178-bfd4-bfd20f39562a", "Gladiator", new DateTime(2000, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 503000000m },
                    { 10, 2, "6406205b-a498-4949-b5db-1a7ed3822278", "Jurassic Park", new DateTime(1993, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1030000000m }
                });

            migrationBuilder.InsertData(
                table: "MovieGenres",
                columns: new[] { "Id", "GenreId", "Guid", "MovieId" },
                values: new object[,]
                {
                    { 1, 1, "3d811993-e4dc-417d-a715-096fa32f5a14", 1 },
                    { 2, 3, "8364c9d0-5227-455e-9bdf-f5a956059010", 1 },
                    { 3, 1, "d1a72790-bb00-4477-acb2-a784a41598ce", 2 },
                    { 4, 2, "57635c5f-b22e-464c-bc67-296e7864229c", 2 },
                    { 5, 1, "7e533623-1f6d-499e-a114-a51619dfba3a", 3 },
                    { 6, 5, "94efa5d6-78a6-49eb-ae60-a74b39ed8729", 3 },
                    { 7, 10, "ceae109a-5052-4c35-8c87-9836ac342cf9", 4 },
                    { 8, 10, "3dd6617d-561b-4e66-8e18-7deeeed0f09b", 5 },
                    { 9, 3, "4e0838a7-e03f-4da6-8938-a976f17efdff", 5 },
                    { 10, 6, "51839dde-a059-44d6-8d57-1c82b90c1792", 6 },
                    { 11, 4, "fe512a0e-757a-4f49-8b09-ad05742a35cb", 6 },
                    { 12, 9, "e84e457c-aa82-4766-859f-76ee7f580e1d", 7 },
                    { 13, 7, "c536c4af-1c17-43f6-a26c-13c10a43d769", 8 },
                    { 14, 5, "2218269a-70a3-4202-8000-bc7f0d8cade3", 9 },
                    { 15, 4, "30c38a23-acd0-442f-b27b-a5f252b6fc19", 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
