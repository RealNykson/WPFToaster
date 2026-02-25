using System;
using System.Threading.Tasks;
using MinimalToastExample.ViewModels;

namespace MinimalToastExample.Essentials
{
  public class ToastRequestedEventArgs : EventArgs
  {
    public string Title { get; }
    public string Message { get; }
    public ToastStatus Status { get; }
    public int DurationMs { get; }

    public ToastRequestedEventArgs(string title, string message, ToastStatus status, int durationMs)
    {
      Title = title;
      Message = message;
      Status = status;
      DurationMs = durationMs;
    }
  }

  public class PromiseToastRequestedEventArgs : EventArgs
  {
    public string LoadingMessage { get; }
    public TaskCompletionSource<ToastItemVM> CompletionSource { get; }

    public PromiseToastRequestedEventArgs(string loadingMessage, TaskCompletionSource<ToastItemVM> completionSource)
    {
      LoadingMessage = loadingMessage;
      CompletionSource = completionSource;
    }
  }

  public static class Toaster
  {
    public static event EventHandler<ToastRequestedEventArgs> ToastRequested;
    public static event EventHandler<PromiseToastRequestedEventArgs> PromiseRequested;

    public static void Toast(string message, ToastStatus status, int durationMs = 5000)
    {
      ToastRequested?.Invoke(null, new ToastRequestedEventArgs(null, message, status, durationMs));
    }

    public static void Toast(string title, string message, ToastStatus status, int durationMs = 5000)
    {
      ToastRequested?.Invoke(null, new ToastRequestedEventArgs(title, message, status, durationMs));
    }

    public static async Task Promise(Func<Task> action, string loading, string success, string error, int durationMs = 5000)
    {
      var tcs = new TaskCompletionSource<ToastItemVM>();
      PromiseRequested?.Invoke(null, new PromiseToastRequestedEventArgs(loading, tcs));
      var vm = await tcs.Task;

      try
      {
        await action();
        vm.ResolveAsSuccess(success, durationMs);
      }
      catch
      {
        vm.ResolveAsError(error, durationMs);
      }
    }

    public static async Task<T> Promise<T>(Func<Task<T>> action, string loading, Func<T, string> success, string error, int durationMs = 5000)
    {
      var tcs = new TaskCompletionSource<ToastItemVM>();
      PromiseRequested?.Invoke(null, new PromiseToastRequestedEventArgs(loading, tcs));
      var vm = await tcs.Task;

      try
      {
        var result = await action();
        vm.ResolveAsSuccess(success(result), durationMs);
        return result;
      }
      catch
      {
        vm.ResolveAsError(error, durationMs);
        throw;
      }
    }
  }
}
