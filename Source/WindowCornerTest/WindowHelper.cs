using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace WindowCornerTest
{
	public static class WindowHelper
	{
		#region Win32

		[DllImport("Dwmapi.dll", SetLastError = true)]
		private static extern int DwmSetWindowAttribute(
			IntPtr hwnd,
			uint dwAttribute,
			[In] ref uint pvAttribute, // IntPtr
			uint cbAttribute);

		// Derived from dwmapi.h included in Windows Insider Preview SDK 10.0.22000.0
		private enum DWMWINDOWATTRIBUTE : uint
		{
			DWMWA_NCRENDERING_ENABLED = 1,        // [get] Is non-client rendering enabled/disabled
			DWMWA_NCRENDERING_POLICY,             // [set] DWMNCRENDERINGPOLICY - Non-client rendering policy
			DWMWA_TRANSITIONS_FORCEDISABLED,      // [set] Potentially enable/forcibly disable transitions
			DWMWA_ALLOW_NCPAINT,                  // [set] Allow contents rendered in the non-client area to be visible on the DWM-drawn frame.
			DWMWA_CAPTION_BUTTON_BOUNDS,          // [get] Bounds of the caption button area in window-relative space.
			DWMWA_NONCLIENT_RTL_LAYOUT,           // [set] Is non-client content RTL mirrored
			DWMWA_FORCE_ICONIC_REPRESENTATION,    // [set] Force this window to display iconic thumbnails.
			DWMWA_FLIP3D_POLICY,                  // [set] Designates how Flip3D will treat the window.
			DWMWA_EXTENDED_FRAME_BOUNDS,          // [get] Gets the extended frame bounds rectangle in screen space
			DWMWA_HAS_ICONIC_BITMAP,              // [set] Indicates an available bitmap when there is no better thumbnail representation.
			DWMWA_DISALLOW_PEEK,                  // [set] Don't invoke Peek on the window.
			DWMWA_EXCLUDED_FROM_PEEK,             // [set] LivePreview exclusion information
			DWMWA_CLOAK,                          // [set] Cloak or uncloak the window
			DWMWA_CLOAKED,                        // [get] Gets the cloaked state of the window
			DWMWA_FREEZE_REPRESENTATION,          // [set] BOOL, Force this window to freeze the thumbnail without live update

			// Newly added
			DWMWA_PASSIVE_UPDATE_MODE,            // [set] BOOL, Updates the window only when desktop composition runs for other reasons
			DWMWA_USE_HOSTBACKDROPBRUSH,          // [set] BOOL, Allows the use of host backdrop brushes for the window.
			DWMWA_USE_IMMERSIVE_DARK_MODE = 20,   // [set] BOOL, Allows a window to either use the accent color, or dark, according to the user Color Mode preferences.
			DWMWA_WINDOW_CORNER_PREFERENCE = 33,  // [set] WINDOW_CORNER_PREFERENCE, Controls the policy that rounds top-level window corners
			DWMWA_BORDER_COLOR,                   // [set] COLORREF, The color of the thin border around a top-level window
			DWMWA_CAPTION_COLOR,                  // [set] COLORREF, The color of the caption
			DWMWA_TEXT_COLOR,                     // [set] COLORREF, The color of the caption text
			DWMWA_VISIBLE_FRAME_BORDER_THICKNESS, // [get] UINT, width of the visible border around a thick frame window

			DWMWA_LAST
		}

		// Newly added
		private enum DWM_WINDOW_CORNER_PREFERENCE : uint
		{
			DWMWCP_DEFAULT = 0,
			DWMWCP_DONOTROUND = 1,
			DWMWCP_ROUND = 2,
			DWMWCP_ROUNDSMALL = 3
		}

		public const int S_OK = 0x0;
		public const int S_FALSE = 0x1;

		[DllImport("User32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetWindowCompositionAttribute(
			IntPtr hwnd,
			ref WindowCompositionAttributeData data);

		[StructLayout(LayoutKind.Sequential)]
		private struct WindowCompositionAttributeData
		{
			public WindowCompositionAttribute Attribute;
			public IntPtr Data;
			public int SizeOfData;
		}

		private enum WindowCompositionAttribute
		{
			// ...
			WCA_ACCENT_POLICY = 19
			// ...
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct AccentPolicy
		{
			public AccentState AccentState;
			public int AccentFlags;
			public uint GradientColor;
			public int AnimationId;
		}

		private enum AccentState
		{
			ACCENT_DISABLED = 0,
			ACCENT_ENABLE_GRADIENT = 1,
			ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
			ACCENT_ENABLE_BLURBEHIND = 3,
			ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
			ACCENT_INVALID_STATE = 5
		}

		#endregion

		public static bool SetWindowCorners(Window window, CornerPreference preference)
		{
			var windowHandle = new WindowInteropHelper(window).Handle;

			return SetWindowCorners(windowHandle, preference);
		}

		public static bool SetWindowCorners(IntPtr windowHandle, CornerPreference preference)
		{
			var value = (uint)preference;

			return (DwmSetWindowAttribute(
				windowHandle,
				(uint)DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
				ref value,
				(uint)Marshal.SizeOf(value)) == S_OK);
		}

		public static bool EnableBackgroundBlur(Window window)
		{
			var windowHandle = new WindowInteropHelper(window).Handle;

			return EnableBackgroundBlur(windowHandle);
		}

		public static bool EnableBackgroundBlur(IntPtr windowHandle)
		{
			var accent = new AccentPolicy { AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND };
			var accentSize = Marshal.SizeOf(accent);

			var accentPointer = IntPtr.Zero;
			try
			{
				accentPointer = Marshal.AllocHGlobal(accentSize);
				Marshal.StructureToPtr(accent, accentPointer, false);

				var data = new WindowCompositionAttributeData
				{
					Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
					Data = accentPointer,
					SizeOfData = accentSize
				};

				return SetWindowCompositionAttribute(
					windowHandle,
					ref data);
			}
			finally
			{
				Marshal.FreeHGlobal(accentPointer);
			}
		}
	}

	public enum CornerPreference : uint
	{
		/// <summary>
		/// Let the system decide whether or not to round window corners.
		/// Equivalent to DWMWCP_DEFAULT
		/// </summary>
		Default = 0,

		/// <summary>
		/// Never round window corners.
		/// Equivalent to DWMWCP_DONOTROUND
		/// </summary>
		DoNotRound = 1,

		/// <summary>
		/// Round the corners if appropriate.
		/// Equivalent to DWMWCP_ROUND
		/// </summary>
		Round = 2,

		/// <summary>
		/// Round the corners if appropriate, with a small radius.
		/// Equivalent to DWMWCP_ROUNDSMALL
		/// </summary>
		RoundSmall = 3
	}
}