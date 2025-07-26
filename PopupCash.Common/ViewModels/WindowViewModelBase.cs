using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using PopupCash.Common.Models.Commands.Impl;
using PopupCash.Database.Models.Locations;

namespace PopupCash.Common.ViewModels
{
    public abstract partial class WindowViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Window ID
        /// </summary>
        protected string windowId { get; init; }

        /// <summary>
        /// Title
        /// </summary>
        [ObservableProperty]
        private string _title;

        /// <summary>
        /// Window left position
        /// </summary>
        [ObservableProperty]
        private double _windowLeft;

        /// <summary>
        /// Window top position
        /// </summary>
        [ObservableProperty]
        private double _windowTop;
        /// <summary>
        /// Window Width
        /// </summary>
        [ObservableProperty]
        private double _windowWidth;

        /// <summary>
        /// Window Height
        /// </summary>
        [ObservableProperty]
        private double _windowHeight;

        public WindowViewModelBase(ILogger<WindowViewModelBase> logger) : base(logger)
        {
            if (this.GetType().Name is string typeName)
                windowId = typeName.Replace("ViewModel", "");
            else
                windowId = string.Empty;

            _title = string.Empty;
        }

        protected WindowPosition? GetWindowPosition(Window window)
        {
            var command = new ReadMainWindowPositionCommand(windowId, WindowWidth, WindowHeight <= 0 ? window.Height : WindowHeight);
            if (command.Execute(windowId) is not WindowPosition windowPosition) return null;


            logger.LogDebug($"Start {windowId} set window position. ({windowPosition.Left}, {windowPosition.Top})");

            return windowPosition;
        }

    }
}
