using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MinimalToastExample.Essentials;

namespace MinimalToastExample.Converters
{
  public class ToastPositionToHorizontalAlignmentConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is ToastPosition pos)
      {
        switch (pos)
        {
          case ToastPosition.TopLeft:
          case ToastPosition.BottomLeft:
            return HorizontalAlignment.Left;
          case ToastPosition.TopCenter:
          case ToastPosition.BottomCenter:
            return HorizontalAlignment.Center;
          case ToastPosition.TopRight:
          case ToastPosition.BottomRight:
            return HorizontalAlignment.Right;
        }
      }
      return HorizontalAlignment.Right;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class ToastPositionToVerticalAlignmentConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is ToastPosition pos)
      {
        switch (pos)
        {
          case ToastPosition.TopLeft:
          case ToastPosition.TopCenter:
          case ToastPosition.TopRight:
            return VerticalAlignment.Top;
          case ToastPosition.BottomLeft:
          case ToastPosition.BottomCenter:
          case ToastPosition.BottomRight:
            return VerticalAlignment.Bottom;
        }
      }
      return VerticalAlignment.Bottom;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
