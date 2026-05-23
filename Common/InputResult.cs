using ConsoleApp1.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Common
{
    public class InputResult<T>
    {
        public bool IsCancelled { get; }
        public bool IsError { get; }
        public bool IsSuccess => !IsCancelled && !IsError;
        public T Value { get; }
        public string ErrorMessage { get; }

        private InputResult(bool isCancelled, bool isError, T value, string errorMessage)
        {
            IsCancelled = isCancelled;
            IsError = isError;
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static InputResult<T> Success(T value)
            => new(false, false, value, null);

        public static InputResult<T> Cancel()
            => new(true, false, default, null);

        public static InputResult<T> Error(string message)
            => new(false, true, default, message);
        public void ShowErrorIfNeeded(BaseView view)
        {
            if (IsError)
                view.ShowError(ErrorMessage);
        }
    }
}
