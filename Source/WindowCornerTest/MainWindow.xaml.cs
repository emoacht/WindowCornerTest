using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace WindowCornerTest
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Create_Click(object sender, RoutedEventArgs e)
		{
			var preference = (DoNotRoundButton.IsChecked is true) ? CornerPreference.DoNotRound
						   : (RoundButton.IsChecked is true) ? CornerPreference.Round
						   : (RoundSmallButton.IsChecked is true) ? CornerPreference.RoundSmall
						   : CornerPreference.Default;

			var enableBlur = (this.BackgroundBlurCheckBox.IsChecked is true);

			var window = new TestWindow(preference, enableBlur);

			if (this.WindowStyleCheckBox.IsChecked is true)
			{
				window.WindowStyle = WindowStyle.None;
			}

			if (this.AllowsTransparencyCheckBox.IsChecked is true)
			{
				window.AllowsTransparency = true;
			}

			if (this.WindowChromeCheckBox.IsChecked is true)
			{
				WindowChrome.SetWindowChrome(window, new WindowChrome { GlassFrameThickness = new Thickness(-1) });
			}

			window.Owner = this;
			window.Show();
		}
	}
}