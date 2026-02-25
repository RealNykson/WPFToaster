using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MinimalToastExample.Behaviors
{
  public static class SmoothTransform
  {
    private static readonly Duration DefaultDuration = new Duration(TimeSpan.FromMilliseconds(250));
    private static readonly CubicEase Easing = new CubicEase { EasingMode = EasingMode.EaseOut };

    #region TargetY

    public static readonly DependencyProperty TargetYProperty =
      DependencyProperty.RegisterAttached(
        "TargetY", typeof(double), typeof(SmoothTransform),
        new PropertyMetadata(0.0, OnTargetYChanged));

    public static double GetTargetY(DependencyObject d) => (double)d.GetValue(TargetYProperty);
    public static void SetTargetY(DependencyObject d, double value) => d.SetValue(TargetYProperty, value);

    private static void OnTargetYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is UIElement element)
      {
        var transform = GetOrCreateTranslate(element);
        var anim = new DoubleAnimation((double)e.NewValue, DefaultDuration) { EasingFunction = Easing };
        transform.BeginAnimation(TranslateTransform.YProperty, anim);
      }
    }

    #endregion

    #region TargetScale

    public static readonly DependencyProperty TargetScaleProperty =
      DependencyProperty.RegisterAttached(
        "TargetScale", typeof(double), typeof(SmoothTransform),
        new PropertyMetadata(1.0, OnTargetScaleChanged));

    public static double GetTargetScale(DependencyObject d) => (double)d.GetValue(TargetScaleProperty);
    public static void SetTargetScale(DependencyObject d, double value) => d.SetValue(TargetScaleProperty, value);

    private static void OnTargetScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is UIElement element)
      {
        var transform = GetOrCreateScale(element);
        var val = (double)e.NewValue;
        var animX = new DoubleAnimation(val, DefaultDuration) { EasingFunction = Easing };
        var animY = new DoubleAnimation(val, DefaultDuration) { EasingFunction = Easing };
        transform.BeginAnimation(ScaleTransform.ScaleXProperty, animX);
        transform.BeginAnimation(ScaleTransform.ScaleYProperty, animY);
      }
    }

    #endregion

    #region TargetOpacity

    public static readonly DependencyProperty TargetOpacityProperty =
      DependencyProperty.RegisterAttached(
        "TargetOpacity", typeof(double), typeof(SmoothTransform),
        new PropertyMetadata(0.0, OnTargetOpacityChanged));

    public static double GetTargetOpacity(DependencyObject d) => (double)d.GetValue(TargetOpacityProperty);
    public static void SetTargetOpacity(DependencyObject d, double value) => d.SetValue(TargetOpacityProperty, value);

    private static void OnTargetOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is UIElement element)
      {
        var anim = new DoubleAnimation((double)e.NewValue, DefaultDuration) { EasingFunction = Easing };
        element.BeginAnimation(UIElement.OpacityProperty, anim);
      }
    }

    #endregion

    #region Transform helpers

    private static TranslateTransform GetOrCreateTranslate(UIElement element)
    {
      var fe = element as FrameworkElement;
      if (fe == null) return new TranslateTransform();

      if (fe.RenderTransform is TransformGroup group)
      {
        foreach (var t in group.Children)
          if (t is TranslateTransform tt) return tt;
        var newTt = new TranslateTransform();
        group.Children.Add(newTt);
        return newTt;
      }

      if (fe.RenderTransform is TranslateTransform existing)
        return existing;

      var tg = new TransformGroup();
      tg.Children.Add(new ScaleTransform(1, 1));
      tg.Children.Add(new TranslateTransform());
      fe.RenderTransform = tg;
      return (TranslateTransform)tg.Children[1];
    }

    private static ScaleTransform GetOrCreateScale(UIElement element)
    {
      var fe = element as FrameworkElement;
      if (fe == null) return new ScaleTransform();

      if (fe.RenderTransform is TransformGroup group)
      {
        foreach (var t in group.Children)
          if (t is ScaleTransform st) return st;
        var newSt = new ScaleTransform(1, 1);
        group.Children.Insert(0, newSt);
        return newSt;
      }

      if (fe.RenderTransform is ScaleTransform existing)
        return existing;

      var tg = new TransformGroup();
      tg.Children.Add(new ScaleTransform(1, 1));
      tg.Children.Add(new TranslateTransform());
      fe.RenderTransform = tg;
      return (ScaleTransform)tg.Children[0];
    }

    #endregion
  }
}
