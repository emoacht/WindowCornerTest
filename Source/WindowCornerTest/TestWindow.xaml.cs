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

namespace WindowCornerTest
{
	public partial class TestWindow : Window
	{
		public TestWindow()
		{
			InitializeComponent();
		}

		private readonly CornerPreference _preference;
		private readonly bool _enableBlur;

		public TestWindow(CornerPreference preference, bool enableBlur) : this()
		{
			this._preference = preference;
			this._enableBlur = enableBlur;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			WindowHelper.SetWindowCorner(this, _preference);

			if (_enableBlur)
			{
				WindowHelper.EnableBackgroundBlur(this);
			}
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);

			this.DragMove();
		}

		private void Close_Click(object sender, RoutedEventArgs e) => this.Close();
	}
}