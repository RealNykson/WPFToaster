using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MinimalToastExample.Utilities
{
  public interface IRelayCommand : ICommand
  {
    void UpdateCanExecuteState();
  }

  public class RelayCommand : IRelayCommand
  {
    readonly Func<bool> _canExecute;
    readonly Action _execute;

    public RelayCommand(Action execute) : this(execute, null) { }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
      if (execute == null) throw new ArgumentNullException("execute");
      _execute = execute;
      _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged;

    public void UpdateCanExecuteState()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      return _canExecute == null ? true : _canExecute();
    }

    public void Execute(object parameter)
    {
      _execute();
    }
  }
}
