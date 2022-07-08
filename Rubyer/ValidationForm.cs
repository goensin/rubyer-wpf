using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Rubyer
{
    /// <summary>
    /// 验证表单，只用于判断子控件是否有验证错误信息
    /// </summary>
    public class ValidationForm : Border
    {
        private readonly ObservableCollection<ValidationError> errors = new ObservableCollection<ValidationError>();

        static ValidationForm()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ValidationForm), new FrameworkPropertyMetadata(typeof(ValidationForm)));
        }

        internal static readonly DependencyPropertyKey IsValidPropertyKey = DependencyProperty.RegisterReadOnly(
             "IsValid", typeof(bool), typeof(ValidationForm), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// 是否有效
        /// </summary>
        public static readonly DependencyProperty IsValidProperty = IsValidPropertyKey.DependencyProperty;

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            private set { SetValue(IsValidPropertyKey, value); }
        }

        internal static readonly DependencyPropertyKey ErrorsPropertyKey = DependencyProperty.RegisterReadOnly(
            "Errors", typeof(ReadOnlyObservableCollection<ValidationError>), typeof(ValidationForm), new FrameworkPropertyMetadata(new ReadOnlyObservableCollection<ValidationError>(new ObservableCollection<ValidationError>()), FrameworkPropertyMetadataOptions.NotDataBindable));

        /// <summary>
        /// 所有错误
        /// </summary>
        public static readonly DependencyProperty ErrorsProperty = ErrorsPropertyKey.DependencyProperty;

        /// <summary>
        /// 所有错误
        /// </summary>
        public ReadOnlyObservableCollection<ValidationError> Errors
        {
            get { return (ReadOnlyObservableCollection<ValidationError>)GetValue(ErrorsProperty); }
            private set { SetValue(ErrorsPropertyKey, value); }
        }

        internal static readonly DependencyPropertyKey FirstErrorPropertyKey = DependencyProperty.RegisterReadOnly(
            "FirstError", typeof(ValidationError), typeof(ValidationForm), new FrameworkPropertyMetadata(default(ValidationError)));

        /// <summary>
        /// 所有错误
        /// </summary>
        public static readonly DependencyProperty FirstErrorProperty = FirstErrorPropertyKey.DependencyProperty;

        /// <summary>
        /// 所有错误
        /// </summary>
        public ValidationError FirstError
        {
            get { return (ValidationError)GetValue(FirstErrorProperty); }
            private set { SetValue(FirstErrorPropertyKey, value); }
        }

        /// <inheritdoc/>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            AddValidationDependencyObject(this);
        }

        private void AddValidationDependencyObject(DependencyObject element)
        {
            if (element == null)
            {
                return;
            }

            if (element is TextBox || element is PasswordBox)
            {
                Validation.AddErrorHandler(element, OnValidationError);
            }

            foreach (var child in LogicalTreeHelper.GetChildren(element))
            {
                AddValidationDependencyObject(child as DependencyObject);
            }
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            switch (e.Action)
            {
                case ValidationErrorEventAction.Added:
                    errors.Add(e.Error);
                    break;

                case ValidationErrorEventAction.Removed:
                    errors.Remove(e.Error);
                    break;
            }

            Errors = new ReadOnlyObservableCollection<ValidationError>(errors);
            FirstError = Errors.FirstOrDefault();
            IsValid = Errors.Count == 0;
        }
    }
}