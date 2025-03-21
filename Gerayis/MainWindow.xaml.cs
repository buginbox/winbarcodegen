﻿/*
MIT License

Copyright (c) Léo Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/
using Gerayis.Classes;
using Gerayis.Enums;
using Gerayis.Pages;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Gerayis;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private Button CheckedButton { get; set; }
	private AppPages? PageToShow { get; init; }
	private IKeyboardMouseEvents KeyboardMouseEvents;
	private bool Focused { get; set; }

	readonly ColorAnimation colorAnimation = new()
	{
		From = (Color)ColorConverter.ConvertFromString(App.Current.Resources["LightAccentColor"].ToString()),
		To = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Background1"].ToString()),
		Duration = new(TimeSpan.FromSeconds(0.2d))
	};
	public MainWindow(AppPages? pageToShow = null)
	{
		InitializeComponent();
		PageToShow = pageToShow; // If we want to show a specific page on startup

		InitUI(); // Load the UI
	}

	private void InitUI()
	{
		Activated += (o, e) => { Focused = true; }; // The window is focused
		Deactivated += (o, e) => { Focused = false; }; // The window isn't focused
		Closing += (o, e) => Application.Current.Shutdown();

		KeyboardMouseEvents = Hook.GlobalEvents(); // Hook the keyboard and mouse events
		Hook.GlobalEvents().OnCombination(new Dictionary<Combination, Action>
		{
			{
				Combination.FromString("Control+C"), () =>
				{
					if (Focused)
					{
						if (PageContent.Content is BarCodePage)
						{
							Global.BarCodePage.CopyBtn_Click(null, null);
						}
						else if (PageContent.Content is QRCodePage)
						{
							Global.QRCodePage.CopyBtn_Click(null, null);
						}
					}
				}
			}
		});

		HelloTxt.Text = Global.GetHiSentence; // Set the "Hello" message

		CheckButton(((PageToShow is null) ? Global.Settings.StartupPage : PageToShow) switch
		{
			AppPages.BarCode => BarCodeTabBtn,
			AppPages.QRCode => QRCodeTabBtn,
			_ => BarCodeTabBtn
		}); // Check the start page button
		PageContent.Content = ((PageToShow is null) ? Global.Settings.StartupPage : PageToShow) switch
		{
			AppPages.BarCode => Global.BarCodePage,
			AppPages.QRCode => Global.QRCodePage,
			_ => Global.BarCodePage
		}; // Set page

		PageContent.Navigated += (o, e) => AnimatePage();

		//if (MessageBox.Show(Properties.Resources.UseQrixMsg, Properties.Resources.LaunchQrix, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
		//{
		//	Process.Start("explorer.exe", "https://qrix.leocorporation.dev");
		//}
	}

	private void CheckButton(Button button)
	{
		button.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["LightAccentColor"].ToString()) }; // Set the background

		CheckedButton = button; // Set the "checked" button
	}

	private void ResetAllCheckStatus()
	{
		BarCodeTabBtn.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set the foreground
		BarCodeTabBtn.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Background1"].ToString()) }; // Set the background

		QRCodeTabBtn.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set the foreground
		QRCodeTabBtn.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Background1"].ToString()) }; // Set the background

		SettingsTabBtn.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set the foreground
		SettingsTabBtn.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Background1"].ToString()) }; // Set the background
	}

	private void TabLeave(object sender, MouseEventArgs e)
	{
		Button button = (Button)sender; // Create button

		if (button != CheckedButton)
		{
			button.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation); // Play animation
		}
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		Application.Current.Shutdown(); // Quit
	}

	private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized; // Minimize window
	}

	private void BarCodeTabBtn_Click(object sender, RoutedEventArgs e)
	{
		ResetAllCheckStatus(); // Reset the background and foreground of all buttons
		CheckButton(BarCodeTabBtn); // Check the "BarCode" button

		PageContent.Navigate(Global.BarCodePage); // Navigate
	}

	private void QRCodeTabBtn_Click(object sender, RoutedEventArgs e)
	{
		ResetAllCheckStatus(); // Reset the background and foreground of all buttons
		CheckButton(QRCodeTabBtn); // Check the "QRCode" button

		PageContent.Navigate(Global.QRCodePage); // Navigate
	}

	private void SettingsTabBtn_Click(object sender, RoutedEventArgs e)
	{
		ResetAllCheckStatus(); // Reset the background and foreground of all buttons
		CheckButton(SettingsTabBtn); // Check the "Settings" button

		PageContent.Navigate(Global.SettingsPage); // Navigate
	}

	private void PinBtn_Click(object sender, RoutedEventArgs e)
	{
		Topmost = !Topmost; // Pin/Unpin
		PinBtn.Content = Topmost ? "\uF604" : "\uF602"; // Set text
		PinToolTip.Content = Topmost ? Properties.Resources.Unpin : Properties.Resources.Pin; // Set text
	}

	private void AnimatePage()
	{
		Storyboard storyboard = new();

		ThicknessAnimationUsingKeyFrames t = new();
		t.KeyFrames.Add(new SplineThicknessKeyFrame(new(0, 30, 0, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
		t.KeyFrames.Add(new SplineThicknessKeyFrame(new(0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.1))));
		t.AccelerationRatio = 0.5;

		storyboard.Children.Add(t);

		Storyboard.SetTargetName(t, PageContent.Name);
		Storyboard.SetTargetProperty(t, new(Frame.MarginProperty));
		storyboard.Begin(this);
	}

	private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		Process.Start("explorer.exe", "https://qrix.leocorporation.dev");
    }
}
