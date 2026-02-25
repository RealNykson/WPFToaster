using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MinimalToastExample.Converters
{
  public class ToastStackOffsetConverter : IMultiValueConverter
  {
    private const double CollapsedStep = 8.0;
    private const double ExpandedStep = 64.0;
    private const double ExitOffset = 20.0;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values.Length < 4 ||
          values[0] == DependencyProperty.UnsetValue ||
          values[1] == DependencyProperty.UnsetValue ||
          values[2] == DependencyProperty.UnsetValue ||
          values[3] == DependencyProperty.UnsetValue)
        return 0.0;

      int index = (int)values[0];
      bool isExpanded = (bool)values[1];
      bool isTop = (bool)values[2];
      bool isVisible = (bool)values[3];

      double stackDirection = isTop ? 1.0 : -1.0;
      double exitDirection = isTop ? -1.0 : 1.0;

      if (!isVisible)
        return ExitOffset * exitDirection;

      double step = isExpanded ? ExpandedStep : CollapsedStep;
      return index * step * stackDirection;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class ToastStackScaleConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values.Length < 2 ||
          values[0] == DependencyProperty.UnsetValue ||
          values[1] == DependencyProperty.UnsetValue)
        return 1.0;

      int index = (int)values[0];
      bool isExpanded = (bool)values[1];

      if (isExpanded) return 1.0;
      return Math.Max(0.85, 1.0 - index * 0.05);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class ToastStackOpacityConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values.Length < 3 ||
          values[0] == DependencyProperty.UnsetValue ||
          values[1] == DependencyProperty.UnsetValue ||
          values[2] == DependencyProperty.UnsetValue)
        return 1.0;

      int index = (int)values[0];
      bool isExpanded = (bool)values[1];
      bool isVisible = (bool)values[2];

      if (!isVisible) return 0.0;
      if (isExpanded) return 1.0;
      return Math.Max(0.4, 1.0 - index * 0.2);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class ToastStackVisibilityConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values.Length < 2 ||
          values[0] == DependencyProperty.UnsetValue ||
          values[1] == DependencyProperty.UnsetValue)
        return Visibility.Visible;

      int index = (int)values[0];
      bool isExpanded = (bool)values[1];

      if (isExpanded) return Visibility.Visible;
      return index < 3 ? Visibility.Visible : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class ToastStackZIndexConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is int index)
        return 100 - index;
      return 100;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
