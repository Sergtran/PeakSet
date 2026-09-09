using PeakSet.Domain.Entities;
using PeakSet.Domain.Enums;
using PeakSet.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PeakSet.Infrastructure.Data;

public class ExerciseCatalogConfiguration
	: IEntityTypeConfiguration<ExerciseCatalogEntry>
{
	public void Configure(
		EntityTypeBuilder<ExerciseCatalogEntry> builder)
	{
		builder.Property(x => x.Name)
			.HasConversion(name => name.Value, value => new Name(value))
			.HasMaxLength(ExerciseCatalogEntry.MaxNameLength)
			.IsRequired();

		builder.Property(x => x.ExerciseType).HasConversion<string>();
		builder.Property(x => x.DefaultLaterality).HasConversion<string>();

		builder.HasIndex(x => x.Name).IsUnique();

		// 111 ejercicios de DEFAULT_EXERCISES (index.html).
		// GUIDs deterministas (uuid5 sobre el nombre), generados una sola vez y estables para HasData.
		builder.HasData(
			new ExerciseCatalogEntry(Guid.Parse("ea29f7bd-478d-5a3d-a890-c7880b7abb5a"), new Name("Press de Banca"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("27503eee-cabc-5b52-92ce-d0d21fe31c23"), new Name("Press de Banca Inclinado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("d13ad581-d222-5191-be04-73244bf4c367"), new Name("Press de Banca Declinado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("a13ba155-5060-5768-ba7d-a68963e20495"), new Name("Press con Mancuernas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("02f3f6fe-c951-5e2c-8193-f23688f501ce"), new Name("Press Inclinado con Mancuernas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("efeed315-dd0c-5f09-959b-b3f753baef29"), new Name("Press en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("2f447b3a-5051-56ec-82bf-cc078cdf385f"), new Name("Press Hammer Strength"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("4e1b2f99-499a-5adc-8f10-d61653133ae9"), new Name("Chest Press en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("4e7caacc-c5f0-5edb-9297-ebf420c259ac"), new Name("Aperturas con Mancuernas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("0ea1c6ed-02cf-5dc9-bc68-e7345870b695"), new Name("Aperturas en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("6919cc95-f3f9-5337-a5e6-b4c23fcb8904"), new Name("Aperturas en Poleas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("7d58aad8-ac8c-5ef4-b750-85bf9fd1bdb5"), new Name("Peck Deck"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("5f00b897-be5a-573b-a732-b07e8c585a73"), new Name("Press Militar con Barra"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("66205a06-2a3a-51f9-95c2-09e64cd1db2d"), new Name("Press Militar con Mancuernas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("63b74f57-f1e4-5771-a7ee-dcbec1aa9bc0"), new Name("Press Militar en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("fc878cb8-32c7-5ee9-a59b-46ba1a3549bb"), new Name("Arnold Press"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("0b56946a-da6c-5d3f-94e0-2ddc0d06d25c"), new Name("Push Press"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("706b3c15-bedc-5e42-98e4-2398c751cdb5"), new Name("Elevaciones Laterales"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("b707e25b-15da-5ffe-bda4-85a8bade2cdf"), new Name("Elevaciones Laterales en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("a1b02ab3-8208-54df-94bf-896ddee7d808"), new Name("Elevaciones Frontales"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("a50a3fa5-7d82-5a9d-bb0f-add7b6a7d543"), new Name("Pájaros (Deltoide Posterior)"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("b67e126d-9ecc-5826-95b9-78e90088ca77"), new Name("Reverse Peck Deck"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("fa095d4c-f1a4-52f4-82aa-c33c9b6fa791"), new Name("Fondos en Paralelas"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("283984eb-10a5-548d-a703-cf5123627106"), new Name("Fondos en Máquina Asistida"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("631c1db1-d4b1-59cc-b0b9-192c7feb499c"), new Name("Fondos en Banco"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("dae1feb4-2e05-5e34-bf90-4c19dfb83df6"), new Name("Flexiones"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("0807d52a-bace-5734-b016-2658215b99fb"), new Name("Flexiones Diamante"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("ea0c22ea-5576-5395-8e0d-e89240254d69"), new Name("Flexiones Declive"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("40051f28-4773-57e7-96c9-477a2b8dc513"), new Name("Extensión de Tríceps en Polea"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("1fbca1cc-fe5d-5a51-8859-d7249faf9a5e"), new Name("Extensión de Tríceps con Cuerda"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("b7adebc1-0cc9-549d-a715-46ce91a86422"), new Name("Extensión de Tríceps en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("e53ed0c4-05c3-5c94-8714-669a82e98028"), new Name("Extensión Overhead con Mancuerna"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("c5a5342d-6759-5c6a-a56a-4f3c0ecf62a8"), new Name("Tríceps Francés"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("3b472797-55d5-5313-b923-1cd009fd3120"), new Name("Press Cerrado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("f41cee49-0fd7-5b91-93f1-04748bf18a7a"), new Name("Dominadas Pronadas"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("d6ff9fab-9bf9-5e86-b4c4-a917594ac193"), new Name("Dominadas Supinas (Chin Up)"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("eea84f69-4f14-5856-b36b-6828da6d2b75"), new Name("Dominadas Neutras"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("3d3fa499-fa54-534f-9f97-9891de0b772a"), new Name("Dominadas en Máquina Asistida"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("4b204d3a-4a3a-524a-b093-331644e5a200"), new Name("Jalón al Pecho"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("4714d208-6042-544f-939d-c2bdd7851df4"), new Name("Jalón Agarre Cerrado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("0e73b905-0027-530d-b8dc-c1f2be0f0531"), new Name("Jalón Unilateral"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("363df271-b263-597b-beff-e1659a38893e"), new Name("Remo con Barra"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("8214e37d-3268-5a67-bdec-f82895fe5cdd"), new Name("Remo Pendlay"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("9dda4b65-a421-582c-810f-810777014163"), new Name("Remo con Mancuerna"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("a4c20eee-267d-5193-95f9-481a8176a6a3"), new Name("Remo en Polea Baja"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("bb7ce2ef-1874-5477-9e60-d4606218c24f"), new Name("Remo en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("3fa770e1-b7d8-5879-b855-df34da9b2426"), new Name("Remo Hammer Strength"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("99a331ae-c76b-58e5-b977-8f0cd1254130"), new Name("Remo T-Bar"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("85d63843-52ef-5397-a62f-41258782959a"), new Name("Remo Alto en Polea"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("8c7c2375-0b50-52ea-ace7-a43fc7e15bd8"), new Name("Pullover en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("558a1dca-f170-5483-8f05-8d0dc0556119"), new Name("Pullover en Polea"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("4f902f28-8351-5ed1-afe1-95ad20efda04"), new Name("Face Pull"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("7a267408-d662-51ea-9ecf-d14768e06b4d"), new Name("Curl de Bíceps con Barra"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("2a48b934-4c06-53c0-afc3-bccf579a0e81"), new Name("Curl de Bíceps con Mancuernas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("08224b0a-cc35-5572-8758-3966c60d8cf1"), new Name("Curl Martillo"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("60c68901-081b-5e80-b28f-f329441fe182"), new Name("Curl en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("df9703da-5326-5082-8f01-14a83a3a55b4"), new Name("Curl Predicador"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("8cdbab6c-e87c-54b1-97dc-1f9ef1c20c77"), new Name("Curl Scott"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("5808b076-a2da-5848-8a93-d316337d1161"), new Name("Curl en Polea"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("cd587f34-0702-5d03-b2bd-408801853d25"), new Name("Sentadilla Trasera"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("a7126405-a988-57b5-a3ab-eda55a4699bd"), new Name("Sentadilla Frontal"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("1bc014d9-1ba1-5591-8baa-53694b2da198"), new Name("Sentadilla Goblet"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("d3f3c117-fc77-5c8a-9b6d-d38bc60e96ac"), new Name("Sentadilla Hack"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("8ceac6d6-aca0-5d63-be61-114fa5915841"), new Name("Sentadilla Hack Inversa"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("b7d4cbe5-3034-55f0-8f7d-053b51951005"), new Name("Sentadilla en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("3eb3361a-b306-5e7a-8613-47fd4783297a"), new Name("Sentadilla Smith"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("0d1b0f28-f46a-5a83-ae17-466988e0c456"), new Name("Prensa de Piernas 45°"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("bbf4fd8c-a3d0-5bc0-8055-4def27d5ab41"), new Name("Prensa de Piernas Horizontal"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("4c726686-8113-5c21-a78e-b33a740d9079"), new Name("Prensa de Piernas Unilateral"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("47f27e7d-2e12-561b-a545-aad1aad886d8"), new Name("Peso Muerto Convencional"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("bcbf5ee0-774b-51fa-b81c-8ef37db04d9b"), new Name("Peso Muerto Sumo"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("aa27d0c5-0d71-5157-9bab-e476234d5b09"), new Name("Peso Muerto Rumano"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("690b6019-86f1-5c51-a573-6077ead0a1b9"), new Name("Peso Muerto en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("28c73b58-9fe7-5b4e-9075-a1fcdcf74586"), new Name("Hip Thrust con Barra"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("2a0f772d-506b-570c-a645-7629b689aca7"), new Name("Hip Thrust en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("adff15a8-cfb8-57ae-9b5f-d1581f961a1e"), new Name("Zancadas Caminando"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("755b18a8-f5a1-55dd-8ba9-baacb1e649ec"), new Name("Zancadas Estáticas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("5a464ab5-5ab7-51a3-a2a7-e5cc16b78580"), new Name("Zancadas Reversas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("eed953ff-6b64-5e6a-b0ad-d5024576af29"), new Name("Zancadas en Smith"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("b8254256-1c4d-5c6e-a71c-4f26b6986282"), new Name("Extensiones de Cuádriceps"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("9dda5755-d509-50b7-8eb4-bf8e64920077"), new Name("Curl Femoral Acostado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("90519e77-6d7e-56c3-b73a-a314a383144a"), new Name("Curl Femoral Sentado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("3e01e0bd-63c7-5792-bfd1-380df349b01b"), new Name("Curl Femoral de Pie"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("459e5221-44b3-537b-bbf5-c35d3c8638e5"), new Name("Aductores en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("c1695b98-4900-5526-a776-f1f71873b06a"), new Name("Abductores en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("97332d61-2584-5ed0-9cef-066c23d26221"), new Name("Elevación de Gemelos de Pie"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("f34a088e-dbc4-58a2-83b9-e28e53f4ac30"), new Name("Elevación de Gemelos Sentado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("d4287efd-6457-52a9-a561-06a80698333d"), new Name("Gemelos en Prensa"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("9a5ae25d-8135-56e7-9f1c-fa013198fb3b"), new Name("Crunch Abdominal"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("83b43adc-dcdd-5a2f-9b87-06a9e40ccb31"), new Name("Crunch en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("4c9a7b16-23b9-5ecf-a501-998005c5a0fa"), new Name("Crunch en Polea"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("eb272348-6711-581e-8ff5-627bb4acd6fa"), new Name("Crunch Declinado"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("263dd739-a740-5656-85c2-e017c98cbf16"), new Name("Plancha Abdominal"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("62e2a5b6-9564-5c7d-84cf-4b171decca62"), new Name("Plancha Lateral"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("ebbbdad5-4d7a-5ea7-929c-b4b38dd0f510"), new Name("Plancha con Peso"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("52a4d1da-d696-5c5f-89b2-752a125e623c"), new Name("Elevaciones de Piernas"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("6c32ed4a-2fda-5cec-aa5e-05b25807be04"), new Name("Elevaciones de Piernas Colgado"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("01154a22-56a2-5fa4-963f-69329aabc964"), new Name("Ab Wheel"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("eebbe44c-0279-5bdf-8dc8-e9d8c8522352"), new Name("Saltar Cuerda"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("6d79b895-e8b1-500e-9dba-a75f24871949"), new Name("Burpees"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("7526326f-bac5-5ffd-a0b1-dc17aae06df8"), new Name("Jump Squats"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("009a3bcd-daf6-5197-ab4c-86e5dc7814e1"), new Name("Caminadora"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("b175cd6e-2fb8-5991-9f11-4940ad28bf91"), new Name("Bicicleta Estática"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("ed18781e-5a2b-52fc-af43-f229537b25f8"), new Name("Bicicleta Reclinada"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("e9674430-dd28-5667-ba9f-09d92907c6a9"), new Name("Elíptica"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("a902f83f-29a7-5500-9449-48db5adf8086"), new Name("Escaladora"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("6cc59bb5-cc15-57b3-ad5c-f2145de3c829"), new Name("Remo Ergómetro"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("d30189d9-0b3e-5168-9466-c71ecd1dd0fa"), new Name("Assault Bike"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("6d452d26-1055-5b3e-90a0-fa8782b35d66"), new Name("Wall Sit"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("5928c72b-1667-5592-a48c-102b17a72fba"), new Name("Dead Hang"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(Guid.Parse("a2f5ab27-11dc-5b37-ac87-04552ca9ec66"), new Name("Farmer Walk"), ExerciseType.Time, Laterality.Bilateral)
		);
	}
}
