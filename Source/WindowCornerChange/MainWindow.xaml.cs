using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using WindowCornerTest;

namespace WindowCornerChange
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public int Count
		{
			get { return (int)GetValue(CountProperty); }
			set { SetValue(CountProperty, value); }
		}
		public static readonly DependencyProperty CountProperty =
			DependencyProperty.Register("Count", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

		public string ClassNames
		{
			get { return (string)GetValue(ClassNamesProperty); }
			set { SetValue(ClassNamesProperty, value); }
		}
		public static readonly DependencyProperty ClassNamesProperty =
			DependencyProperty.Register("ClassNames", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

		private async void Start_Click(object sender, RoutedEventArgs e)
		{
			this.StartButton.IsEnabled = false;
			Count = 3;
			ClassNames = null;

			while (Count > 0)
			{
				await Task.Delay(TimeSpan.FromSeconds(1));
				Count--;
			}

			Debug.WriteLine($"----------");

			foreach (var windowHandle in WindowHelper.EnumerateWindowsUnderCursor())
			{
				var result = WindowHelper.SetWindowCorners(windowHandle, CornerPreference.DoNotRound);
				var className = $"{WindowHelper.GetWindowClassName(windowHandle)} ({windowHandle}) [{result}]";
				Debug.WriteLine(className);
				ClassNames += className + Environment.NewLine;
			}

			SystemSounds.Asterisk.Play();
			this.StartButton.IsEnabled = true;
		}
	}
}