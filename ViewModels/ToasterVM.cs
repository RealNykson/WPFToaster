using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;
using MinimalToastExample.Essentials;
using MinimalToastExample.Utilities;

namespace MinimalToastExample.ViewModels
{
  public class ToasterVM : NotifyObject
  {
    private static ToasterVM _instance;
    public static ToasterVM Instance => _instance ?? (_instance = new ToasterVM());

    public ObservableCollection<ToastItemVM> Toasts { get; } = new ObservableCollection<ToastItemVM>();

    public ToastPosition Position { get => Get<ToastPosition>(); set => Set(value); }
    public bool IsExpanded { get => Get<bool>(); set => Set(value); }

    private readonly Dispatcher _dispatcher;
    private DispatcherTimer _collapseTimer;

    private ToasterVM()
    {
      Position = ToastPosition.BottomRight;
      IsExpanded = false;
      _dispatcher = Dispatcher.CurrentDispatcher;
      Toaster.ToastRequested += OnToastRequested;
      Toaster.PromiseRequested += OnPromiseRequested;
      Toasts.CollectionChanged += OnToastsCollectionChanged;
    }

    private bool IsTop =>
      Position == ToastPosition.TopLeft ||
      Position == ToastPosition.TopCenter ||
      Position == ToastPosition.TopRight;

    public void SetExpanded(bool expanded)
    {
      if (expanded)
      {
        _collapseTimer?.Stop();
        if (!IsExpanded)
        {
          IsExpanded = true;
          foreach (var toast in Toasts)
            toast.PauseTimer();
        }
      }
      else
      {
        _collapseTimer?.Stop();
        _collapseTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
        _collapseTimer.Tick += (s, e) =>
        {
          _collapseTimer.Stop();
          IsExpanded = false;
          foreach (var toast in Toasts)
            toast.ResumeTimer();
        };
        _collapseTimer.Start();
      }
    }

    private void RecalculateIndexes()
    {
      int visibleCount = 0;
      for (int i = Toasts.Count - 1; i >= 0; i--)
      {
        if (!Toasts[i].IsVisible)
          continue;
        Toasts[i].Index = visibleCount;
        visibleCount++;
      }
    }

    private void OnToastsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      RecalculateIndexes();
    }

    private void OnToastRequested(object sender, ToastRequestedEventArgs e)
    {
      if (!_dispatcher.CheckAccess())
      {
        _dispatcher.Invoke(() => OnToastRequested(sender, e));
        return;
      }

      var item = new ToastItemVM(e.Title, e.Message, e.Status, e.DurationMs, IsTop);
      item.DismissRequested += OnDismissRequested;
      item.DismissStarting += OnDismissStarting;
      Toasts.Add(item);
    }

    private void OnPromiseRequested(object sender, PromiseToastRequestedEventArgs e)
    {
      if (!_dispatcher.CheckAccess())
      {
        _dispatcher.Invoke(() => OnPromiseRequested(sender, e));
        return;
      }

      var item = new ToastItemVM(null, e.LoadingMessage, ToastStatus.Loading, 0, IsTop, isPromise: true);
      item.DismissRequested += OnDismissRequested;
      item.DismissStarting += OnDismissStarting;
      Toasts.Add(item);
      e.CompletionSource.SetResult(item);
    }

    private void OnDismissStarting(object sender, EventArgs e) => RecalculateIndexes();

    private void OnDismissRequested(ToastItemVM item)
    {
      item.DismissRequested -= OnDismissRequested;
      item.DismissStarting -= OnDismissStarting;
      Toasts.Remove(item);
    }
  }
}
