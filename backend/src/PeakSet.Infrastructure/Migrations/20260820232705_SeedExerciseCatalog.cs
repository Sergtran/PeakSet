using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PeakSet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedExerciseCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ExerciseCatalogEntries",
                columns: new[] { "Id", "DefaultLaterality", "ExerciseType", "Name" },
                values: new object[,]
                {
                    { new Guid("009a3bcd-daf6-5197-ab4c-86e5dc7814e1"), "Bilateral", "Time", "Caminadora" },
                    { new Guid("01154a22-56a2-5fa4-963f-69329aabc964"), "Bilateral", "Bodyweight", "Ab Wheel" },
                    { new Guid("02f3f6fe-c951-5e2c-8193-f23688f501ce"), "Bilateral", "Standard", "Press Inclinado con Mancuernas" },
                    { new Guid("0807d52a-bace-5734-b016-2658215b99fb"), "Bilateral", "Bodyweight", "Flexiones Diamante" },
                    { new Guid("08224b0a-cc35-5572-8758-3966c60d8cf1"), "Bilateral", "Standard", "Curl Martillo" },
                    { new Guid("0b56946a-da6c-5d3f-94e0-2ddc0d06d25c"), "Bilateral", "Standard", "Push Press" },
                    { new Guid("0d1b0f28-f46a-5a83-ae17-466988e0c456"), "Bilateral", "Standard", "Prensa de Piernas 45°" },
                    { new Guid("0e73b905-0027-530d-b8dc-c1f2be0f0531"), "Bilateral", "Standard", "Jalón Unilateral" },
                    { new Guid("0ea1c6ed-02cf-5dc9-bc68-e7345870b695"), "Bilateral", "Standard", "Aperturas en Máquina" },
                    { new Guid("1bc014d9-1ba1-5591-8baa-53694b2da198"), "Bilateral", "Standard", "Sentadilla Goblet" },
                    { new Guid("1fbca1cc-fe5d-5a51-8859-d7249faf9a5e"), "Bilateral", "Standard", "Extensión de Tríceps con Cuerda" },
                    { new Guid("263dd739-a740-5656-85c2-e017c98cbf16"), "Bilateral", "Time", "Plancha Abdominal" },
                    { new Guid("27503eee-cabc-5b52-92ce-d0d21fe31c23"), "Bilateral", "Standard", "Press de Banca Inclinado" },
                    { new Guid("283984eb-10a5-548d-a703-cf5123627106"), "Bilateral", "Standard", "Fondos en Máquina Asistida" },
                    { new Guid("28c73b58-9fe7-5b4e-9075-a1fcdcf74586"), "Bilateral", "Standard", "Hip Thrust con Barra" },
                    { new Guid("2a0f772d-506b-570c-a645-7629b689aca7"), "Bilateral", "Standard", "Hip Thrust en Máquina" },
                    { new Guid("2a48b934-4c06-53c0-afc3-bccf579a0e81"), "Bilateral", "Standard", "Curl de Bíceps con Mancuernas" },
                    { new Guid("2f447b3a-5051-56ec-82bf-cc078cdf385f"), "Bilateral", "Standard", "Press Hammer Strength" },
                    { new Guid("363df271-b263-597b-beff-e1659a38893e"), "Bilateral", "Standard", "Remo con Barra" },
                    { new Guid("3b472797-55d5-5313-b923-1cd009fd3120"), "Bilateral", "Standard", "Press Cerrado" },
                    { new Guid("3d3fa499-fa54-534f-9f97-9891de0b772a"), "Bilateral", "Standard", "Dominadas en Máquina Asistida" },
                    { new Guid("3e01e0bd-63c7-5792-bfd1-380df349b01b"), "Bilateral", "Standard", "Curl Femoral de Pie" },
                    { new Guid("3eb3361a-b306-5e7a-8613-47fd4783297a"), "Bilateral", "Standard", "Sentadilla Smith" },
                    { new Guid("3fa770e1-b7d8-5879-b855-df34da9b2426"), "Bilateral", "Standard", "Remo Hammer Strength" },
                    { new Guid("40051f28-4773-57e7-96c9-477a2b8dc513"), "Bilateral", "Standard", "Extensión de Tríceps en Polea" },
                    { new Guid("459e5221-44b3-537b-bbf5-c35d3c8638e5"), "Bilateral", "Standard", "Aductores en Máquina" },
                    { new Guid("4714d208-6042-544f-939d-c2bdd7851df4"), "Bilateral", "Standard", "Jalón Agarre Cerrado" },
                    { new Guid("47f27e7d-2e12-561b-a545-aad1aad886d8"), "Bilateral", "Standard", "Peso Muerto Convencional" },
                    { new Guid("4b204d3a-4a3a-524a-b093-331644e5a200"), "Bilateral", "Standard", "Jalón al Pecho" },
                    { new Guid("4c726686-8113-5c21-a78e-b33a740d9079"), "Bilateral", "Standard", "Prensa de Piernas Unilateral" },
                    { new Guid("4c9a7b16-23b9-5ecf-a501-998005c5a0fa"), "Bilateral", "Standard", "Crunch en Polea" },
                    { new Guid("4e1b2f99-499a-5adc-8f10-d61653133ae9"), "Bilateral", "Standard", "Chest Press en Máquina" },
                    { new Guid("4e7caacc-c5f0-5edb-9297-ebf420c259ac"), "Bilateral", "Standard", "Aperturas con Mancuernas" },
                    { new Guid("4f902f28-8351-5ed1-afe1-95ad20efda04"), "Bilateral", "Standard", "Face Pull" },
                    { new Guid("52a4d1da-d696-5c5f-89b2-752a125e623c"), "Bilateral", "Bodyweight", "Elevaciones de Piernas" },
                    { new Guid("558a1dca-f170-5483-8f05-8d0dc0556119"), "Bilateral", "Standard", "Pullover en Polea" },
                    { new Guid("5808b076-a2da-5848-8a93-d316337d1161"), "Bilateral", "Standard", "Curl en Polea" },
                    { new Guid("5928c72b-1667-5592-a48c-102b17a72fba"), "Bilateral", "Time", "Dead Hang" },
                    { new Guid("5a464ab5-5ab7-51a3-a2a7-e5cc16b78580"), "Bilateral", "Standard", "Zancadas Reversas" },
                    { new Guid("5f00b897-be5a-573b-a732-b07e8c585a73"), "Bilateral", "Standard", "Press Militar con Barra" },
                    { new Guid("60c68901-081b-5e80-b28f-f329441fe182"), "Bilateral", "Standard", "Curl en Máquina" },
                    { new Guid("62e2a5b6-9564-5c7d-84cf-4b171decca62"), "Bilateral", "Time", "Plancha Lateral" },
                    { new Guid("631c1db1-d4b1-59cc-b0b9-192c7feb499c"), "Bilateral", "Bodyweight", "Fondos en Banco" },
                    { new Guid("63b74f57-f1e4-5771-a7ee-dcbec1aa9bc0"), "Bilateral", "Standard", "Press Militar en Máquina" },
                    { new Guid("66205a06-2a3a-51f9-95c2-09e64cd1db2d"), "Bilateral", "Standard", "Press Militar con Mancuernas" },
                    { new Guid("690b6019-86f1-5c51-a573-6077ead0a1b9"), "Bilateral", "Standard", "Peso Muerto en Máquina" },
                    { new Guid("6919cc95-f3f9-5337-a5e6-b4c23fcb8904"), "Bilateral", "Standard", "Aperturas en Poleas" },
                    { new Guid("6c32ed4a-2fda-5cec-aa5e-05b25807be04"), "Bilateral", "Bodyweight", "Elevaciones de Piernas Colgado" },
                    { new Guid("6cc59bb5-cc15-57b3-ad5c-f2145de3c829"), "Bilateral", "Time", "Remo Ergómetro" },
                    { new Guid("6d452d26-1055-5b3e-90a0-fa8782b35d66"), "Bilateral", "Time", "Wall Sit" },
                    { new Guid("6d79b895-e8b1-500e-9dba-a75f24871949"), "Bilateral", "Bodyweight", "Burpees" },
                    { new Guid("706b3c15-bedc-5e42-98e4-2398c751cdb5"), "Bilateral", "Standard", "Elevaciones Laterales" },
                    { new Guid("7526326f-bac5-5ffd-a0b1-dc17aae06df8"), "Bilateral", "Bodyweight", "Jump Squats" },
                    { new Guid("755b18a8-f5a1-55dd-8ba9-baacb1e649ec"), "Bilateral", "Standard", "Zancadas Estáticas" },
                    { new Guid("7a267408-d662-51ea-9ecf-d14768e06b4d"), "Bilateral", "Standard", "Curl de Bíceps con Barra" },
                    { new Guid("7d58aad8-ac8c-5ef4-b750-85bf9fd1bdb5"), "Bilateral", "Standard", "Peck Deck" },
                    { new Guid("8214e37d-3268-5a67-bdec-f82895fe5cdd"), "Bilateral", "Standard", "Remo Pendlay" },
                    { new Guid("83b43adc-dcdd-5a2f-9b87-06a9e40ccb31"), "Bilateral", "Standard", "Crunch en Máquina" },
                    { new Guid("85d63843-52ef-5397-a62f-41258782959a"), "Bilateral", "Standard", "Remo Alto en Polea" },
                    { new Guid("8c7c2375-0b50-52ea-ace7-a43fc7e15bd8"), "Bilateral", "Standard", "Pullover en Máquina" },
                    { new Guid("8cdbab6c-e87c-54b1-97dc-1f9ef1c20c77"), "Bilateral", "Standard", "Curl Scott" },
                    { new Guid("8ceac6d6-aca0-5d63-be61-114fa5915841"), "Bilateral", "Standard", "Sentadilla Hack Inversa" },
                    { new Guid("90519e77-6d7e-56c3-b73a-a314a383144a"), "Bilateral", "Standard", "Curl Femoral Sentado" },
                    { new Guid("97332d61-2584-5ed0-9cef-066c23d26221"), "Bilateral", "Standard", "Elevación de Gemelos de Pie" },
                    { new Guid("99a331ae-c76b-58e5-b977-8f0cd1254130"), "Bilateral", "Standard", "Remo T-Bar" },
                    { new Guid("9a5ae25d-8135-56e7-9f1c-fa013198fb3b"), "Bilateral", "Bodyweight", "Crunch Abdominal" },
                    { new Guid("9dda4b65-a421-582c-810f-810777014163"), "Bilateral", "Standard", "Remo con Mancuerna" },
                    { new Guid("9dda5755-d509-50b7-8eb4-bf8e64920077"), "Bilateral", "Standard", "Curl Femoral Acostado" },
                    { new Guid("a13ba155-5060-5768-ba7d-a68963e20495"), "Bilateral", "Standard", "Press con Mancuernas" },
                    { new Guid("a1b02ab3-8208-54df-94bf-896ddee7d808"), "Bilateral", "Standard", "Elevaciones Frontales" },
                    { new Guid("a2f5ab27-11dc-5b37-ac87-04552ca9ec66"), "Bilateral", "Time", "Farmer Walk" },
                    { new Guid("a4c20eee-267d-5193-95f9-481a8176a6a3"), "Bilateral", "Standard", "Remo en Polea Baja" },
                    { new Guid("a50a3fa5-7d82-5a9d-bb0f-add7b6a7d543"), "Bilateral", "Standard", "Pájaros (Deltoide Posterior)" },
                    { new Guid("a7126405-a988-57b5-a3ab-eda55a4699bd"), "Bilateral", "Standard", "Sentadilla Frontal" },
                    { new Guid("a902f83f-29a7-5500-9449-48db5adf8086"), "Bilateral", "Time", "Escaladora" },
                    { new Guid("aa27d0c5-0d71-5157-9bab-e476234d5b09"), "Bilateral", "Standard", "Peso Muerto Rumano" },
                    { new Guid("adff15a8-cfb8-57ae-9b5f-d1581f961a1e"), "Bilateral", "Standard", "Zancadas Caminando" },
                    { new Guid("b175cd6e-2fb8-5991-9f11-4940ad28bf91"), "Bilateral", "Time", "Bicicleta Estática" },
                    { new Guid("b67e126d-9ecc-5826-95b9-78e90088ca77"), "Bilateral", "Standard", "Reverse Peck Deck" },
                    { new Guid("b707e25b-15da-5ffe-bda4-85a8bade2cdf"), "Bilateral", "Standard", "Elevaciones Laterales en Máquina" },
                    { new Guid("b7adebc1-0cc9-549d-a715-46ce91a86422"), "Bilateral", "Standard", "Extensión de Tríceps en Máquina" },
                    { new Guid("b7d4cbe5-3034-55f0-8f7d-053b51951005"), "Bilateral", "Standard", "Sentadilla en Máquina" },
                    { new Guid("b8254256-1c4d-5c6e-a71c-4f26b6986282"), "Bilateral", "Standard", "Extensiones de Cuádriceps" },
                    { new Guid("bb7ce2ef-1874-5477-9e60-d4606218c24f"), "Bilateral", "Standard", "Remo en Máquina" },
                    { new Guid("bbf4fd8c-a3d0-5bc0-8055-4def27d5ab41"), "Bilateral", "Standard", "Prensa de Piernas Horizontal" },
                    { new Guid("bcbf5ee0-774b-51fa-b81c-8ef37db04d9b"), "Bilateral", "Standard", "Peso Muerto Sumo" },
                    { new Guid("c1695b98-4900-5526-a776-f1f71873b06a"), "Bilateral", "Standard", "Abductores en Máquina" },
                    { new Guid("c5a5342d-6759-5c6a-a56a-4f3c0ecf62a8"), "Bilateral", "Standard", "Tríceps Francés" },
                    { new Guid("cd587f34-0702-5d03-b2bd-408801853d25"), "Bilateral", "Standard", "Sentadilla Trasera" },
                    { new Guid("d13ad581-d222-5191-be04-73244bf4c367"), "Bilateral", "Standard", "Press de Banca Declinado" },
                    { new Guid("d30189d9-0b3e-5168-9466-c71ecd1dd0fa"), "Bilateral", "Time", "Assault Bike" },
                    { new Guid("d3f3c117-fc77-5c8a-9b6d-d38bc60e96ac"), "Bilateral", "Standard", "Sentadilla Hack" },
                    { new Guid("d4287efd-6457-52a9-a561-06a80698333d"), "Bilateral", "Standard", "Gemelos en Prensa" },
                    { new Guid("d6ff9fab-9bf9-5e86-b4c4-a917594ac193"), "Bilateral", "Bodyweight", "Dominadas Supinas (Chin Up)" },
                    { new Guid("dae1feb4-2e05-5e34-bf90-4c19dfb83df6"), "Bilateral", "Bodyweight", "Flexiones" },
                    { new Guid("df9703da-5326-5082-8f01-14a83a3a55b4"), "Bilateral", "Standard", "Curl Predicador" },
                    { new Guid("e53ed0c4-05c3-5c94-8714-669a82e98028"), "Bilateral", "Standard", "Extensión Overhead con Mancuerna" },
                    { new Guid("e9674430-dd28-5667-ba9f-09d92907c6a9"), "Bilateral", "Time", "Elíptica" },
                    { new Guid("ea0c22ea-5576-5395-8e0d-e89240254d69"), "Bilateral", "Bodyweight", "Flexiones Declive" },
                    { new Guid("ea29f7bd-478d-5a3d-a890-c7880b7abb5a"), "Bilateral", "Standard", "Press de Banca" },
                    { new Guid("eb272348-6711-581e-8ff5-627bb4acd6fa"), "Bilateral", "Bodyweight", "Crunch Declinado" },
                    { new Guid("ebbbdad5-4d7a-5ea7-929c-b4b38dd0f510"), "Bilateral", "Time", "Plancha con Peso" },
                    { new Guid("ed18781e-5a2b-52fc-af43-f229537b25f8"), "Bilateral", "Time", "Bicicleta Reclinada" },
                    { new Guid("eea84f69-4f14-5856-b36b-6828da6d2b75"), "Bilateral", "Bodyweight", "Dominadas Neutras" },
                    { new Guid("eebbe44c-0279-5bdf-8dc8-e9d8c8522352"), "Bilateral", "Time", "Saltar Cuerda" },
                    { new Guid("eed953ff-6b64-5e6a-b0ad-d5024576af29"), "Bilateral", "Standard", "Zancadas en Smith" },
                    { new Guid("efeed315-dd0c-5f09-959b-b3f753baef29"), "Bilateral", "Standard", "Press en Máquina" },
                    { new Guid("f34a088e-dbc4-58a2-83b9-e28e53f4ac30"), "Bilateral", "Standard", "Elevación de Gemelos Sentado" },
                    { new Guid("f41cee49-0fd7-5b91-93f1-04748bf18a7a"), "Bilateral", "Bodyweight", "Dominadas Pronadas" },
                    { new Guid("fa095d4c-f1a4-52f4-82aa-c33c9b6fa791"), "Bilateral", "Bodyweight", "Fondos en Paralelas" },
                    { new Guid("fc878cb8-32c7-5ee9-a59b-46ba1a3549bb"), "Bilateral", "Standard", "Arnold Press" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("009a3bcd-daf6-5197-ab4c-86e5dc7814e1"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("01154a22-56a2-5fa4-963f-69329aabc964"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("02f3f6fe-c951-5e2c-8193-f23688f501ce"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("0807d52a-bace-5734-b016-2658215b99fb"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("08224b0a-cc35-5572-8758-3966c60d8cf1"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("0b56946a-da6c-5d3f-94e0-2ddc0d06d25c"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("0d1b0f28-f46a-5a83-ae17-466988e0c456"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("0e73b905-0027-530d-b8dc-c1f2be0f0531"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("0ea1c6ed-02cf-5dc9-bc68-e7345870b695"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("1bc014d9-1ba1-5591-8baa-53694b2da198"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("1fbca1cc-fe5d-5a51-8859-d7249faf9a5e"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("263dd739-a740-5656-85c2-e017c98cbf16"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("27503eee-cabc-5b52-92ce-d0d21fe31c23"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("283984eb-10a5-548d-a703-cf5123627106"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("28c73b58-9fe7-5b4e-9075-a1fcdcf74586"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("2a0f772d-506b-570c-a645-7629b689aca7"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("2a48b934-4c06-53c0-afc3-bccf579a0e81"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("2f447b3a-5051-56ec-82bf-cc078cdf385f"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("363df271-b263-597b-beff-e1659a38893e"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("3b472797-55d5-5313-b923-1cd009fd3120"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("3d3fa499-fa54-534f-9f97-9891de0b772a"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("3e01e0bd-63c7-5792-bfd1-380df349b01b"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("3eb3361a-b306-5e7a-8613-47fd4783297a"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("3fa770e1-b7d8-5879-b855-df34da9b2426"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("40051f28-4773-57e7-96c9-477a2b8dc513"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("459e5221-44b3-537b-bbf5-c35d3c8638e5"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("4714d208-6042-544f-939d-c2bdd7851df4"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("47f27e7d-2e12-561b-a545-aad1aad886d8"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("4b204d3a-4a3a-524a-b093-331644e5a200"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("4c726686-8113-5c21-a78e-b33a740d9079"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("4c9a7b16-23b9-5ecf-a501-998005c5a0fa"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("4e1b2f99-499a-5adc-8f10-d61653133ae9"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("4e7caacc-c5f0-5edb-9297-ebf420c259ac"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("4f902f28-8351-5ed1-afe1-95ad20efda04"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("52a4d1da-d696-5c5f-89b2-752a125e623c"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("558a1dca-f170-5483-8f05-8d0dc0556119"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("5808b076-a2da-5848-8a93-d316337d1161"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("5928c72b-1667-5592-a48c-102b17a72fba"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("5a464ab5-5ab7-51a3-a2a7-e5cc16b78580"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("5f00b897-be5a-573b-a732-b07e8c585a73"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("60c68901-081b-5e80-b28f-f329441fe182"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("62e2a5b6-9564-5c7d-84cf-4b171decca62"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("631c1db1-d4b1-59cc-b0b9-192c7feb499c"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("63b74f57-f1e4-5771-a7ee-dcbec1aa9bc0"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("66205a06-2a3a-51f9-95c2-09e64cd1db2d"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("690b6019-86f1-5c51-a573-6077ead0a1b9"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("6919cc95-f3f9-5337-a5e6-b4c23fcb8904"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("6c32ed4a-2fda-5cec-aa5e-05b25807be04"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("6cc59bb5-cc15-57b3-ad5c-f2145de3c829"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("6d452d26-1055-5b3e-90a0-fa8782b35d66"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("6d79b895-e8b1-500e-9dba-a75f24871949"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("706b3c15-bedc-5e42-98e4-2398c751cdb5"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("7526326f-bac5-5ffd-a0b1-dc17aae06df8"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("755b18a8-f5a1-55dd-8ba9-baacb1e649ec"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("7a267408-d662-51ea-9ecf-d14768e06b4d"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("7d58aad8-ac8c-5ef4-b750-85bf9fd1bdb5"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("8214e37d-3268-5a67-bdec-f82895fe5cdd"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("83b43adc-dcdd-5a2f-9b87-06a9e40ccb31"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("85d63843-52ef-5397-a62f-41258782959a"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("8c7c2375-0b50-52ea-ace7-a43fc7e15bd8"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("8cdbab6c-e87c-54b1-97dc-1f9ef1c20c77"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("8ceac6d6-aca0-5d63-be61-114fa5915841"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("90519e77-6d7e-56c3-b73a-a314a383144a"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("97332d61-2584-5ed0-9cef-066c23d26221"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("99a331ae-c76b-58e5-b977-8f0cd1254130"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("9a5ae25d-8135-56e7-9f1c-fa013198fb3b"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("9dda4b65-a421-582c-810f-810777014163"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("9dda5755-d509-50b7-8eb4-bf8e64920077"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("a13ba155-5060-5768-ba7d-a68963e20495"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("a1b02ab3-8208-54df-94bf-896ddee7d808"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("a2f5ab27-11dc-5b37-ac87-04552ca9ec66"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("a4c20eee-267d-5193-95f9-481a8176a6a3"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("a50a3fa5-7d82-5a9d-bb0f-add7b6a7d543"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("a7126405-a988-57b5-a3ab-eda55a4699bd"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("a902f83f-29a7-5500-9449-48db5adf8086"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("aa27d0c5-0d71-5157-9bab-e476234d5b09"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("adff15a8-cfb8-57ae-9b5f-d1581f961a1e"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("b175cd6e-2fb8-5991-9f11-4940ad28bf91"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("b67e126d-9ecc-5826-95b9-78e90088ca77"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("b707e25b-15da-5ffe-bda4-85a8bade2cdf"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("b7adebc1-0cc9-549d-a715-46ce91a86422"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("b7d4cbe5-3034-55f0-8f7d-053b51951005"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("b8254256-1c4d-5c6e-a71c-4f26b6986282"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("bb7ce2ef-1874-5477-9e60-d4606218c24f"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("bbf4fd8c-a3d0-5bc0-8055-4def27d5ab41"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("bcbf5ee0-774b-51fa-b81c-8ef37db04d9b"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("c1695b98-4900-5526-a776-f1f71873b06a"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("c5a5342d-6759-5c6a-a56a-4f3c0ecf62a8"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("cd587f34-0702-5d03-b2bd-408801853d25"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("d13ad581-d222-5191-be04-73244bf4c367"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("d30189d9-0b3e-5168-9466-c71ecd1dd0fa"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("d3f3c117-fc77-5c8a-9b6d-d38bc60e96ac"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("d4287efd-6457-52a9-a561-06a80698333d"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("d6ff9fab-9bf9-5e86-b4c4-a917594ac193"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("dae1feb4-2e05-5e34-bf90-4c19dfb83df6"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("df9703da-5326-5082-8f01-14a83a3a55b4"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("e53ed0c4-05c3-5c94-8714-669a82e98028"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("e9674430-dd28-5667-ba9f-09d92907c6a9"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("ea0c22ea-5576-5395-8e0d-e89240254d69"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("ea29f7bd-478d-5a3d-a890-c7880b7abb5a"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("eb272348-6711-581e-8ff5-627bb4acd6fa"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("ebbbdad5-4d7a-5ea7-929c-b4b38dd0f510"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("ed18781e-5a2b-52fc-af43-f229537b25f8"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("eea84f69-4f14-5856-b36b-6828da6d2b75"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("eebbe44c-0279-5bdf-8dc8-e9d8c8522352"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("eed953ff-6b64-5e6a-b0ad-d5024576af29"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("efeed315-dd0c-5f09-959b-b3f753baef29"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("f34a088e-dbc4-58a2-83b9-e28e53f4ac30"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("f41cee49-0fd7-5b91-93f1-04748bf18a7a"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("fa095d4c-f1a4-52f4-82aa-c33c9b6fa791"));

            migrationBuilder.DeleteData(
                table: "ExerciseCatalogEntries",
                keyColumn: "Id",
                keyValue: new Guid("fc878cb8-32c7-5ee9-a59b-46ba1a3549bb"));
        }
    }
}
