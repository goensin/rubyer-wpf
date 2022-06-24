using Rubyer.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Rubyer.Commons
{
    /// <summary>
    /// FrameworkElement 扩展
    /// </summary>
    public static class FrameworkElementHelpers
    {
        /// <summary>
        /// 获取 root 元素
        /// </summary>
        /// <param name="dependencyObject">依赖对象</param>
        /// <returns>root 元素</returns>
        public static DependencyObject GetRoot(this DependencyObject dependencyObject)
        {
            DependencyObject result = dependencyObject;
            while ((dependencyObject = VisualTreeHelper.GetParent(dependencyObject)) != null)
            {
                result = dependencyObject;
            }
            return result;
        }

        /// <summary>
        /// 查找父元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="element">元素</param>
        /// <returns>父元素</returns>
        public static T FindParent<T>(this FrameworkElement element) where T : FrameworkElement
        {
            T t = default;
            while (element != null)
            {
                t = element as T;
                if (t != null)
                {
                    break;
                }
                element = element.Parent as FrameworkElement;
            }
            return t;
        }

        /// <summary>
        /// 获取最顶层父元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dependencyObject">依赖对象</param>
        /// <returns>父元素</returns>
        public static T GetOldestParent<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            T t;
            while ((t = VisualTreeHelper.GetParent(dependencyObject) as T) != null)
            {
                dependencyObject = t;
            }
            return dependencyObject.AssertCast<T>();
        }

        /// <summary>
        /// 尝试查找资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="root">元素</param>
        /// <param name="resourceKey">资源 key</param>
        /// <returns>资源</returns>
        internal static T TryFindResourceUp<T>(this FrameworkElement root, object resourceKey)
        {
            if (root != null && root.Resources.Contains(resourceKey))
            {
                object obj = root.Resources[resourceKey];
                if (!(obj is T))
                {
                    return default;
                }
                return (T)obj;
            }
            else
            {
                if (root.Parent is FrameworkElement)
                {
                    return ((FrameworkElement)root.Parent).TryFindResourceUp<T>(resourceKey);
                }
                return default;
            }
        }

        /// <summary>
        /// 获取子元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="parent">父元素</param>
        /// <returns>子元素</returns>
        public static T TryGetChildPartFromVisualTree<T>(this FrameworkElement parent) where T : FrameworkElement
        {
            return parent.TryGetChildFromVisualTree<T>(null);
        }

        /// <summary>
        /// 根据 Name 获取子元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="parent">父元素</param>
        /// <param name="partName">名称</param>
        /// <returns>子元素</returns>
        public static T TryGetChildPartFromVisualTree<T>(this FrameworkElement parent, string partName) where T : FrameworkElement
        {
            ValidateArgument.NotNullOrEmpty(partName, "partName");
            return parent.TryGetChildFromVisualTree<T>((FrameworkElement childControl) => childControl.Name == partName);
        }

        /// <summary>
        /// 根据 AutomationId 获取子元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="parent">父元素</param>
        /// <param name="automationId">自动 ID</param>
        /// <returns>子元素</returns>
        internal static T TryGetChildWithAutomationIdFromVisualTree<T>(this FrameworkElement parent, string automationId) where T : FrameworkElement
        {
            ValidateArgument.NotNullOrEmpty(automationId, "automationId");
            return parent.TryGetChildFromVisualTree<T>((FrameworkElement childControl) => AutomationProperties.GetAutomationId(childControl) == automationId);
        }

        /// <summary>
        /// VisualTree 获取子元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="parent">父元素</param>
        /// <param name="evaluateChild">子元素表达式</param>
        /// <returns>子元素</returns>
        internal static T TryGetChildFromVisualTree<T>(this FrameworkElement parent, Func<FrameworkElement, bool> evaluateChild) where T : FrameworkElement
        {
            ValidateArgument.NotNull(parent, "parent");
            FrameworkElement frameworkElement = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                frameworkElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (frameworkElement != null && (frameworkElement is T && (evaluateChild == null || evaluateChild.Invoke(frameworkElement)) || (frameworkElement = frameworkElement.TryGetChildFromVisualTree<T>(evaluateChild)) != null))
                {
                    break;
                }
            }

            return frameworkElement as T;
        }

        /// <summary>
        /// 获取父元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="child">子元素</param>
        /// <returns>父元素</returns>
        public static T TryGetParentFromVisualTree<T>(this FrameworkElement child) where T : FrameworkElement
        {
            if (child != null && !(child is T))
            {
                return (VisualTreeHelper.GetParent(child) as FrameworkElement).TryGetParentFromVisualTree<T>();
            }

            return (T)(object)child;
        }

        /// <summary>
        /// 遍历子元素
        /// </summary>
        /// <param name="dependencyObj">依赖对象</param>
        /// <param name="childAction">变量方法</param>
        internal static void ForEachVisualChild(this DependencyObject dependencyObj, Action<DependencyObject> childAction)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObj, i);
                child.ForEachVisualChild(childAction);
                childAction.Invoke(child);
            }
        }

        /// <summary>
        /// 是否包含子对象
        /// </summary>
        /// <param name="parent">父元素</param>
        /// <param name="dependencyObj">依赖对象</param>
        /// <returns>结果</returns>
        public static bool ContainsChild(this DependencyObject parent, DependencyObject dependencyObj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child == dependencyObj || child.ContainsChild(dependencyObj))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取依赖属性的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="reference">对象</param>
        /// <param name="property">属性</param>
        /// <returns>属性的值</returns>
        public static T GetAncestorValue<T>(this DependencyObject reference, DependencyProperty property)
        {
            T result = default;
            while (reference != null && (result = ObjectExtension.AssertCast<T>(reference.GetValue(property))) == null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(reference);
                if (parent == null)
                {
                    FrameworkElement frameworkElement = reference as FrameworkElement;
                    if (frameworkElement != null)
                    {
                        parent = frameworkElement.Parent;
                    }
                }

                reference = parent;
            }

            return result;
        }
    }
}
