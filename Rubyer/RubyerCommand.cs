using System;
using System.Windows.Input;

namespace Rubyer
{
    /// <summary>
    /// Command
    /// </summary>
    public class RubyerCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RubyerCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RubyerCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RubyerCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public RubyerCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (x => true);
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Refreshes the.
        /// </summary>
        public void Refresh()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}