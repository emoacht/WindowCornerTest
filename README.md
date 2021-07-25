# Window Corner Test

Rounded corners of window on Windows 11 applicable to WPF.

## Requirements

- .NET 5.0
- Windows 11 (10.0.22000.100)

## Overview

To work with rounded corners introduced in Windows 11, check the official explanation on this feature.

- [Apply rounded corners in desktop apps for Windows 11](https://docs.microsoft.com/en-us/windows/apps/desktop/modernize/apply-rounded-corners)

(This document is dated on 10/02/2020 but actually it was committed on 07/17/2021.)

The underlying values of key constants, `DWMWA_WINDOW_CORNER_PREFERENCE` and `DWM_WINDOW_CORNER_PREFERENCE` enumeration, can be found in dwmapi.h included in Windows Insider Preview SDK.

Unfortunately, the rounded corners have __noticeable jaggies__ (except those of standard title bar) and anti-aliasing is not applied at this moment. This ugly edge is really discouraging to use this feature in production.
