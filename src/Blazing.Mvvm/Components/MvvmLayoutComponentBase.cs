﻿using System.ComponentModel;
using Blazing.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Blazing.Mvvm.Components;

/// <summary>
///  A base class for components that represent a layout and resolves a ViewModel of type <typeparamref name="TViewModel"/>.
/// </summary>
/// <typeparam name="TViewModel">The ViewModel.</typeparam>
public abstract class MvvmLayoutComponentBase<TViewModel> : LayoutComponentBase, IView<TViewModel>
    where TViewModel : IViewModelBase
{
    /// <summary>
    /// Indicates if the component has been disposed.
    /// </summary>
    protected bool IsDisposed { get; private set; }

    /// <summary>
    /// The <c>ViewModel</c> associated with this component, resolved from the dependency injection container.
    /// </summary>
    [Inject]
    protected virtual TViewModel ViewModel { get; set; } = default!;

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected override void OnAfterRender(bool firstRender)
        => ViewModel.OnAfterRender(firstRender);

    /// <inheritdoc/>
    protected override Task OnAfterRenderAsync(bool firstRender)
        => ViewModel.OnAfterRenderAsync(firstRender);

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        ViewModel.PropertyChanged += OnPropertyChanged;
        ViewModel.OnInitialized();
    }

    /// <inheritdoc/>
    protected override Task OnInitializedAsync()
        => ViewModel.OnInitializedAsync();

    /// <inheritdoc/>
    protected override void OnParametersSet()
        => ViewModel.OnParametersSet();

    /// <inheritdoc/>
    protected override Task OnParametersSetAsync()
        => ViewModel.OnParametersSetAsync();

    /// <inheritdoc/>
    protected override bool ShouldRender()
        => ViewModel.ShouldRender();

    /// <summary>
    /// Disposes the component and releases unmanaged resources.
    /// </summary>
    /// <param name="disposing">A boolean value indicating whether the method was called from the dispose method or from a finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            ViewModel.PropertyChanged -= OnPropertyChanged;
        }

        IsDisposed = true;
    }

    private void OnPropertyChanged(object? o, PropertyChangedEventArgs propertyChangedEventArgs)
        => InvokeAsync(StateHasChanged);
}
