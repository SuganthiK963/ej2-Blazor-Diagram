#if !NETSTANDARD
using System;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor
{
    /// <summary>
    /// A implementation of <see cref="IComponentActivator"/> for Syncfusion Blazor components used to initialize custom components
    /// that can be registered through dependency injection.
    /// </summary>
    public class SfComponentActivator : IComponentActivator
    {
        private IServiceProvider serviceProvider { get; }

        /// <summary>
        /// Default constructor for SfComponentActivator class.
        /// </summary>
        /// <param name="provider">Service provider for registered components.</param>
        public SfComponentActivator(IServiceProvider provider)
        {
            serviceProvider = provider;
        }

        /// <summary>
        /// Create component for the specified component type.
        /// </summary>
        /// <param name="componentType">Type of component need to be created.</param>
        /// <returns>Returns newly created component.</returns>
        public IComponent CreateInstance(Type componentType)
        {
            var instance = serviceProvider.GetService(componentType);

            if (instance == null)
            {
                instance = Activator.CreateInstance(componentType);
            }
            var component = instance as IComponent;
            if (component == null)
            {
#pragma warning disable CA1062 // Validate arguments of public methods
                throw new ArgumentException($"The type {componentType.FullName} does not implement {nameof(IComponent)}.", paramName: nameof(componentType));
#pragma warning restore CA1062 // Validate arguments of public methods
            }
            return component;
        }
    }
}
#endif