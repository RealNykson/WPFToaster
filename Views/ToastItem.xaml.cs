using System.Windows.Controls;
using System.Windows.Input;
using MinimalToastExample.ViewModels;

namespace MinimalToastExample.Views
{
  public partial class ToastItem : UserControl
  {
    public ToastItem()
    {
      InitializeComponent();
      MouseEnter += OnMouseEnter;
      MouseLeave += OnMouseLeave;
    }

    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
      ToasterVM.Instance.SetExpanded(true);
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
      ToasterVM.Instance.SetExpanded(false);
    }
  }
}
