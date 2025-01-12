﻿using CommunityToolkit.Maui;
using CodeBreaker.ViewModels;
using CodeBreaker.ViewModels.Services;
using CodeBreaker.MAUI.Services;
using CodeBreaker.Services;
using Microsoft.Extensions.Configuration;
using CodeBreaker.MAUI.Views.Pages;
using CodeBreaker.Shared.Extensions;

namespace CodeBreaker.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
#if DEBUG
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
#else
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Production");
#endif

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Configuration.AddJsonStream(FileSystem.OpenAppPackageFileAsync("appsettings.json").Result);
        builder.Configuration.AddJsonStream(FileSystem.OpenAppPackageFileAsync($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json").Result);
        builder.Services.Configure<GamePageViewModelOptions>(options => options.EnableDialogs = true);
        builder.Services.AddScoped<IDialogService, MauiDialogService>();
        builder.Services.AddScoped<GamePageViewModel>();
		builder.Services.AddHttpClient<IGameClient, GameClient>(client =>
        {
			client.BaseAddress = new(builder.Configuration.GetRequired("ApiBase"));
		});
		builder.Services.AddTransient<GamePage>();
		return builder.Build();
	}
}
