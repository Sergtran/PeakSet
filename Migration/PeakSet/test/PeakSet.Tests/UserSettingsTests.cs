using PeakSet.Domain.Entities;
using PeakSet.Domain.Enums;

namespace PeakSet.Tests;

public class UserSettingsTests
{
	private const string UserId = "user-1";

	[Fact]
	public void Defaults_ShouldMatchIndexHtml()
	{
		var settings = new UserSettings(UserId);

		Assert.Equal(Theme.Light, settings.Theme);
		Assert.Equal(10, settings.TimerPrepSeconds);
		Assert.Equal(40, settings.TimerWorkSeconds);
		Assert.Equal(20, settings.TimerRestSeconds);
		Assert.Equal(5, settings.TimerSets);
	}

	[Fact]
	public void SetTheme_ShouldUpdate()
	{
		var settings = new UserSettings(UserId);

		settings.SetTheme(Theme.Dark);

		Assert.Equal(Theme.Dark, settings.Theme);
	}

	[Fact]
	public void UpdateTimer_ShouldApplyValidValues()
	{
		var settings = new UserSettings(UserId);

		settings.UpdateTimer(15, 45, 30, 4);

		Assert.Equal(15, settings.TimerPrepSeconds);
		Assert.Equal(45, settings.TimerWorkSeconds);
		Assert.Equal(30, settings.TimerRestSeconds);
		Assert.Equal(4, settings.TimerSets);
	}

	[Theory]
	[InlineData(-1, 40, 20, 5)]
	[InlineData(10, 0, 20, 5)]
	[InlineData(10, 40, 0, 5)]
	[InlineData(10, 40, 20, 0)]
	public void UpdateTimer_WithInvalidValues_ShouldThrow(int prep, int work, int rest, int sets)
	{
		var settings = new UserSettings(UserId);

		Assert.Throws<ArgumentOutOfRangeException>(() => settings.UpdateTimer(prep, work, rest, sets));
	}

	[Fact]
	public void Create_WithEmptyUserId_ShouldThrow()
	{
		Assert.Throws<ArgumentException>(() => new UserSettings(""));
	}
}
