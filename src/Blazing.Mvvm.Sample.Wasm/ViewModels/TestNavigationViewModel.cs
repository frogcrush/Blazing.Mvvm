﻿using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Blazing.Mvvm.Sample.Wasm.ViewModels;

[ViewModelDefinition<ITestNavigationViewModel>]
public sealed partial class TestNavigationViewModel : ViewModelBase, ITestNavigationViewModel
{
    private readonly IMvvmNavigationManager _mvvmNavigationManager;
    private readonly NavigationManager _navigationManager;

    private RelayCommand? _hexTranslateNavigateCommand;
    private RelayCommand<string>? _testNavigateCommand;

    [ObservableProperty]
    private string? _queryString;

    [ObservableProperty]
    private string? _test;

    public TestNavigationViewModel(IMvvmNavigationManager mvvmNavigationManager, NavigationManager navigationManager)
    {
        _mvvmNavigationManager = mvvmNavigationManager;
        _navigationManager = navigationManager;
        _navigationManager.LocationChanged += OnLocationChanged;
    }

    public string? Echo { get; set; } = string.Empty;

    public RelayCommand HexTranslateNavigateCommand
        => _hexTranslateNavigateCommand ??= new RelayCommand(() => Navigate<HexTranslateViewModel>());

    public RelayCommand<string> TestNavigateCommand
        => _testNavigateCommand ??= new RelayCommand<string>(Navigate<ITestNavigationViewModel>);

    public void Dispose()
        => _navigationManager.LocationChanged -= OnLocationChanged;

    public override void OnInitialized()
        => ProcessQueryString();

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        => ProcessQueryString();

    private void Navigate<T>(string? @params = null)
        where T : IViewModelBase
    {
        if (string.IsNullOrWhiteSpace(@params))
        {
            _mvvmNavigationManager.NavigateTo<T>();
            return;
        }

        _mvvmNavigationManager.NavigateTo<T>(@params);
    }

    private void ProcessQueryString()
    {
        QueryString = _navigationManager.ToAbsoluteUri(_navigationManager.Uri).Query;
        _navigationManager.TryGetQueryString("test", out string temp);
        Test = temp;
    }
}
