using System;
using System.Threading.Tasks;
using System.Windows;
using MinimalToastExample.Essentials;

namespace MinimalToastExample.Views
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void OnSuccess(object sender, RoutedEventArgs e)
    {
      Toaster.Toast("Operation completed successfully", ToastStatus.Success);
    }

    private void OnError(object sender, RoutedEventArgs e)
    {
      Toaster.Toast("Error", "Something went wrong", ToastStatus.Error);
    }

    private void OnWarning(object sender, RoutedEventArgs e)
    {
      Toaster.Toast("Check your connection", ToastStatus.Warning);
    }

    private void OnInfo(object sender, RoutedEventArgs e)
    {
      Toaster.Toast("Tip", "You can hover to expand stacked toasts", ToastStatus.Info);
    }

    private async void OnPromise(object sender, RoutedEventArgs e)
    {
      await Toaster.Promise(
        async () =>
        {
          await Task.Delay(2000);
          if (new Random().Next(2) == 0)
            throw new Exception("Random failure");
        },
        loading: "Processing...",
        success: "Done!",
        error: "Failed to process"
      );
    }
  }
}
