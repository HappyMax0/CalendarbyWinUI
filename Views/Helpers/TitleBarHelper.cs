﻿using Microsoft.UI.Xaml;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.ViewManagement;

namespace CalendarWinUI3.Views.Helpers
{
    internal class TitleBarHelper
    {
        // workaround as Appwindow titlebar doesn't update caption button colors correctly when changed while app is running
        // https://task.ms/44172495
        public static Windows.UI.Color ApplySystemThemeToCaptionButtons(Window window)
        {
            //var frame = (Application.Current as App).GetRootFrame() as FrameworkElement;
            Windows.UI.Color color;

            var uiSettings = new UISettings();
            var backgroundColor = uiSettings.GetColorValue(UIColorType.Background);

            if (backgroundColor == Colors.Black)
            {
                color = Colors.White;
            }
            else
            {
                color = Colors.Black;
            }
            SetCaptionButtonColors(window, color);
            return color;
        }

        public static void SetCaptionButtonColors(Window window, Windows.UI.Color color)
        {
            var res = Application.Current.Resources;
            res["WindowCaptionForeground"] = color;
            window.AppWindow.TitleBar.ButtonForegroundColor = color;
        }

        public static void SetCaptionButtonBackgroundColors(Window window, Windows.UI.Color? color)
        {
            var titleBar = window.AppWindow.TitleBar;
            titleBar.ButtonBackgroundColor = color;
        }

        public static void SetForegroundColor(Window window, Windows.UI.Color? color)
        {
            var titleBar = window.AppWindow.TitleBar;
            titleBar.ForegroundColor = color;
        }

        public static void SetBackgroundColor(Window window, Windows.UI.Color? color)
        {
            var titleBar = window.AppWindow.TitleBar;
            titleBar.BackgroundColor = color;
        }

    }
}
