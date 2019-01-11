using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightInject.ServiceLocation
{
    using System;
    using System.Collections.Generic;
    using CommonServiceLocator;

    /// <summary>
    /// An <see cref="IServiceLocator"/> adapter for the LightInject service container.
    /// </summary>
    public class LightInjectServiceLocator : ServiceLocatorImplBase
    {
        private readonly IServiceContainer serviceContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightInjectServiceLocator"/> class.
        /// </summary>
        /// <param name="serviceContainer">The <see cref="IServiceContainer"/> instance wrapped by this class.</param>
        internal LightInjectServiceLocator(IServiceContainer serviceContainer)
        {
            this.serviceContainer = serviceContainer;
        }

        /// <summary>
        /// Gets a named instance of the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type of the requested service.</param>
        /// <param name="key">The key of the requested service.</param>
        /// <returns>The requested service instance.</returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (key != null)
            {
                return serviceContainer.GetInstance(serviceType, key);
            }

            return serviceContainer.GetInstance(serviceType);
        }

        /// <summary>
        /// Gets all instances of the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type of services to resolve.</param>
        /// <returns>A list that contains all implementations of the <paramref name="serviceType"/>.</returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return serviceContainer.GetAllInstances(serviceType);
        }
    }
}
