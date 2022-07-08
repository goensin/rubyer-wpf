using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Rubyer.Models
{
    /// <summary>
    /// 错误验证模型
    /// </summary>
    public class ValidationModel : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationModel"/> class.
        /// </summary>
        public ValidationModel()
        {
            Errors = new Dictionary<string, List<string>>();
            PropertyChanged += (sender, e) => RefreshErrors(e.PropertyName);
        }

        /// <summary>
        /// 所有错误
        /// </summary>
        protected Dictionary<string, List<string>> Errors { get; set; }

        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool HasErrors => Errors.Count > 0;

        /// <summary>
        /// 错误改变事件
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 获取错误
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>错误集合</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return Errors.Count > 0 ? Errors.First().Value : new List<string>();
            }
            else
            {
                return Errors.ContainsKey(propertyName) ? Errors[propertyName] : new List<string>();
            }
        }

        /// <summary>
        /// 设置属性错误
        /// </summary>
        /// <param name="propertyErrors">错误集合</param>
        /// <param name="propertyName">属性名称</param>
        protected void SetErrors(List<string> propertyErrors, [CallerMemberName] string propertyName = null)
        {
            Errors.Remove(propertyName);
            Errors.Add(propertyName, propertyErrors);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 设置属性错误
        /// </summary>
        /// <param name="propertyError">错误</param>
        /// <param name="propertyName">属性名称</param>
        protected void SetErrors(string propertyError, [CallerMemberName] string propertyName = null) => SetErrors(new List<string> { propertyError }, propertyName);

        /// <summary>
        /// 清除属性的错误
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected void ClearErrors([CallerMemberName] string propertyName = null)
        {
            Errors.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 清除所有错误
        /// </summary>
        protected void ClearAllErrors()
        {
            foreach (var property in GetType().GetProperties())
            {
                Errors.Remove(property.Name);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property.Name));
            }
        }

        /// <summary>
        /// 刷新所有错误验证
        /// </summary>
        public void RaiseErrorValidation()
        {
            var properties = GetType().GetProperties().Where(x => x.Name != nameof(HasErrors));
            foreach (var property in properties)
            {
                RefreshErrors(property);
            }
        }

        /// <summary>
        /// 判断属性是否有错误
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>是否有错误</returns>
        public bool HasError([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
            {
                if (GetErrors(propertyName) is List<string> errors)
                {
                    return errors.Any();
                }
            }

            return false;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="storage">字段引用</param>
        /// <param name="value">值</param>
        /// <param name="propertyName">属性名称</param>
        public void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return;
            }

            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 设置属性值，并刷新属性错误
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="storage">字段引用</param>
        /// <param name="value">值</param>
        /// <param name="propertyName">属性名称</param>
        public void SetValidationProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            SetProperty(ref storage, value, propertyName);
            RefreshErrors(propertyName);
        }

        private void RefreshErrors(string propertyName)
        {
            ClearErrors(propertyName);
            if (propertyName != null)
            {
                var property = GetType().GetProperty(propertyName);
                if (property != null)
                {
                    RefreshErrors(property);
                }
            }
        }

        private void RefreshErrors(PropertyInfo property)
        {
            var errors = new List<string>();
            var attributes = (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), false);
            foreach (var attribute in attributes)
            {
                if (!attribute.IsValid(property.GetValue(this)))
                {
                    var displayAttribute = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                    string name = displayAttribute == null ? property.Name : displayAttribute.Name;
                    errors.Add(attribute.FormatErrorMessage(name ?? string.Empty));
                }
            }

            if (errors.Count > 0)
            {
                SetErrors(errors, property.Name);
            }
        }
    }
}