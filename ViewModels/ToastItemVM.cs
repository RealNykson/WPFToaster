using System;
using System.Windows.Input;
using System.Windows.Threading;
using MinimalToastExample.Essentials;
using MinimalToastExample.Utilities;

namespace MinimalToastExample.ViewModels
{
  public class ToastItemVM : NotifyObject
  {
    private const int ExitAnimationMs = 250;

    private readonly DispatcherTimer _timer;
    private readonly Dispatcher _dispatcher;
    private int _durationMs;
    private bool _isDismissing;

    public string Title { get => Get<string>(); set => Set(value); }
    public string Message { get => Get<string>(); set => Set(value); }
    public ToastStatus Status { get => Get<ToastStatus>(); set => Set(value); }
    public bool IsVisible { get => Get<bool>(); set => Set(value); }
    public bool IsTopPosition { get => Get<bool>(); set => Set(value); }
    public int Index { get => Get<int>(); set => Set(value); }
    public bool IsPromise { get => Get<bool>(); set => Set(value); }

    public ICommand DismissCommand { get; }

    public event Action<ToastItemVM> DismissRequested;
    public event EventHandler DismissStarting;

    public ToastItemVM(string title, string message, ToastStatus status, int durationMs, bool isTopPosition, bool isPromise = false)
    {
      Title = title;
      Message = message;
      Status = status;
      IsTopPosition = isTopPosition;
      IsVisible = true;
      Index = 0;
      IsPromise = isPromise;
      _durationMs = durationMs;
      _dispatcher = Dispatcher.CurrentDispatcher;

      DismissCommand = new RelayCommand(() => RequestDismiss(), () => !IsPromise);

      _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(durationMs) };
      _timer.Tick += (s, e) => RequestDismiss();

      if (!isPromise)
        _timer.Start();
    }

    public void PauseTimer()
    {
      if (!_isDismissing && !IsPromise)
        _timer.Stop();
    }

    public void ResumeTimer()
    {
      if (!_isDismissing && !IsPromise)
      {
        _timer.Interval = TimeSpan.FromMilliseconds(_durationMs);
        _timer.Start();
      }
    }

    public void ResolveAsSuccess(string message, int durationMs)
    {
      Resolve(message, ToastStatus.Success, durationMs);
    }

    public void ResolveAsError(string message, int durationMs)
    {
      Resolve(message, ToastStatus.Error, durationMs);
    }

    private void Resolve(string message, ToastStatus status, int durationMs)
    {
      if (!_dispatcher.CheckAccess())
      {
        _dispatcher.Invoke(() => Resolve(message, status, durationMs));
        return;
      }

      Message = message;
      Status = status;
      IsPromise = false;
      ((IRelayCommand)DismissCommand).UpdateCanExecuteState();
      _durationMs = durationMs;
      _timer.Interval = TimeSpan.FromMilliseconds(durationMs);
      _timer.Start();
    }

    private void RequestDismiss()
    {
      if (_isDismissing || IsPromise) return;
      _isDismissing = true;
      _timer.Stop();
      IsVisible = false;
      DismissStarting?.Invoke(this, EventArgs.Empty);

      var exitTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(ExitAnimationMs) };
      exitTimer.Tick += (s, e) =>
      {
        exitTimer.Stop();
        DismissRequested?.Invoke(this);
      };
      exitTimer.Start();
    }
  }
}
