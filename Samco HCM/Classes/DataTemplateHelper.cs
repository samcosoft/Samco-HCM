using System;
using System.Windows.Controls;
using System.Windows;

namespace Samco_HCM.Classes
{
    /// <summary>
    /// Class that helps the creation of control and data templates by using delegates.
    /// </summary>
    public static class TemplateGenerator
    {
        private sealed class TemplateGeneratorControl :
          ContentControl
        {
            internal static readonly DependencyProperty FactoryProperty = DependencyProperty.Register("Factory", typeof(Func<object>), typeof(TemplateGeneratorControl), new PropertyMetadata(null, _FactoryChanged));

            private static void _FactoryChanged(DependencyObject instance, DependencyPropertyChangedEventArgs args)
            {
                var control = (TemplateGeneratorControl)instance;
                var factory = (Func<object>)args.NewValue;
                if(factory == null) return;
                control.Content = factory();
            }
        }

        /// <summary>
        /// Creates a data-template that uses the given delegate to create new instances.
        /// </summary>
        public static DataTemplate CreateDataTemplate(Func<object> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var frameworkElementFactory = new FrameworkElementFactory(typeof(TemplateGeneratorControl));
            frameworkElementFactory.SetValue(TemplateGeneratorControl.FactoryProperty, factory);

            var dataTemplate = new DataTemplate(typeof(DependencyObject))
            {
                VisualTree = frameworkElementFactory
            };
            return dataTemplate;
        }

        /// <summary>
        /// Creates a control-template that uses the given delegate to create new instances.
        /// </summary>
        public static ControlTemplate CreateControlTemplate(Type controlType, Func<object> factory)
        {
            if (controlType == null)
                throw new ArgumentNullException(nameof(controlType));

            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var frameworkElementFactory = new FrameworkElementFactory(typeof(TemplateGeneratorControl));
            frameworkElementFactory.SetValue(TemplateGeneratorControl.FactoryProperty, factory);

            var controlTemplate = new ControlTemplate(controlType)
            {
                VisualTree = frameworkElementFactory
            };
            return controlTemplate;
        }
    }
}
