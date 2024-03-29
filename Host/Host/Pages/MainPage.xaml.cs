﻿using Host.Services;
using Microsoft.Maui.Storage;

namespace Host;

public partial class MainPage : ContentPage
{
    private MainPageViewModel _vm;

    public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();

        _vm = vm;
		BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await _vm.FetchProfile();
        await _vm.LoadLocalSavedSongsAsync();
    }

    private async void AddSongsBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await pickFilesAsync();
        }
        catch
        {
            await App.Current.MainPage.DisplayAlert("Failed to open", "Premission to read required.", "OK");
        }
    }

    private async void RemoveSongsBtn_Clicked(object sender, EventArgs e)
    {
        await _vm.RemoveSongsAsync(songsList.SelectedItems);
    }

    private async void ClearSongsBtn_Clicked(object sender, EventArgs e)
    {
        await _vm.ClearSongsAsync();
    }

    private async Task pickFilesAsync()
    {
        var files = await FilePicker.PickMultipleAsync(getFilePickerOptions());

        // required dispatcher to change the poll in UI.
        Dispatcher.Dispatch(async () =>
            await _vm.AddSongsAsync(files));

        Dispatcher.Dispatch(async () => await App.Current.MainPage.DisplayAlert("Message", "Update Songs Completed.", "OK"));
    }

    /// <summary>
    /// Returns audio files options for all the platforms.
    /// </summary>
    private PickOptions getFilePickerOptions()
    {
        var audioFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "audio/mpeg" } },
                    { DevicePlatform.WinUI, new[] { "mp3" } },
                    { DevicePlatform.iOS, new[] { "public.mp3" } },
                    { DevicePlatform.MacCatalyst, new[] { "mp3" } },
                });

        var audioOptions = new PickOptions
        {
            PickerTitle = "Please select an audio files",
            FileTypes = audioFileType,
        };

        return audioOptions;
    }
}

