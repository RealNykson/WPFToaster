using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MinimalToastExample.Essentials;
using MinimalToastExample.ViewModels;

namespace MinimalToastExample.Views
{
  public partial class ToastHost : ContentControl
  {
    public static readonly DependencyProperty PositionProperty =
      DependencyProperty.Register(
        nameof(Position),
        typeof(ToastPosition),
        typeof(ToastHost),
        new PropertyMetadata(ToastPosition.BottomRight, OnPositionChanged));

    public ToastPosition Position
    {
      get => (ToastPosition)GetValue(PositionProperty);
      set => SetValue(PositionProperty, value);
    }

    private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToasterVM.Instance.Position = (ToastPosition)e.NewValue;
    }

    public ToastHost()
    {
      InitializeComponent();
      ToasterVM.Instance.Position = Position;
    }

    private void ToastArea_MouseEnter(object sender, MouseEventArgs e)
    {
      ToasterVM.Instance.SetExpanded(true);
    }

    private void ToastArea_MouseLeave(object sender, MouseEventArgs e)
    {
      ToasterVM.Instance.SetExpanded(false);
    }
  }
}
