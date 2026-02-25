using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MinimalToastExample.Essentials;

namespace MinimalToastExample.Converters
{
  public class ToastStatusToIconDataConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is ToastStatus status)
      {
        switch (status)
        {
          case ToastStatus.Success:
            return Geometry.Parse("M9,0 A9,9 0 1,1 9,18 A9,9 0 1,1 9,0 M5,9 L8,12 L13,6");
          case ToastStatus.Error:
            return Geometry.Parse("M9,0 A9,9 0 1,1 9,18 A9,9 0 1,1 9,0 M6,6 L12,12 M12,6 L6,12");
          case ToastStatus.Warning:
            return Geometry.Parse("M9,1 L17,16 L1,16 Z M9,7 L9,11 M9,13 L9,14");
          case ToastStatus.Info:
            return Geometry.Parse("M9,0 A9,9 0 1,1 9,18 A9,9 0 1,1 9,0 M9,5 L9,5.5 M9,8 L9,13");
          case ToastStatus.Loading:
            return Geometry.Empty;
        }
      }
      return Geometry.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class ToastStatusToAccentBrushConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is ToastStatus status)
      {
        string key;
        switch (status)
        {
          case ToastStatus.Success: key = "ToastSuccessAccent"; break;
          case ToastStatus.Error: key = "ToastErrorAccent"; break;
          case ToastStatus.Warning: key = "ToastWarningAccent"; break;
          case ToastStatus.Loading: key = "ToastLoadingAccent"; break;
          default: key = "ToastInfoAccent"; break;
        }
        return Application.Current.FindResource(key) as Brush;
      }
      return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
