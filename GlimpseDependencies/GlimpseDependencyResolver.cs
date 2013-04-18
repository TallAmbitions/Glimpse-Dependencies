// Copyright (c) 2011 Tall Ambitions, LLC
// See included LICENSE for details.
namespace Tall.Glimpse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using global::Glimpse.Core.Extensibility;

    public class GlimpseDependencyResolver : IDependencyResolver
    {
        private readonly IDependencyResolver resolver;
        private readonly IMessageBroker messageBroker;

        public GlimpseDependencyResolver(IDependencyResolver resolver, IMessageBroker messageBroker)
        {
            this.resolver = resolver;
            this.messageBroker = messageBroker;
        }

        object IDependencyResolver.GetService(Type serviceType)
        {
            var result = this.resolver.GetService(serviceType);
            messageBroker.Publish(new GlimpseDependencyMetadata
            {
                Call = "GetService",
                RequestedType = serviceType,
                ReturnedTypes = result == null ? Enumerable.Empty<Type>() : new[] { result.GetType() },
            });
            return result;
        }

        IEnumerable<object> IDependencyResolver.GetServices(Type serviceType)
        {
            var services = this.resolver.GetServices(serviceType);
            messageBroker.Publish(new GlimpseDependencyMetadata
            {
                Call = "GetServices",
                RequestedType = serviceType,
                ReturnedTypes = (services ?? Enumerable.Empty<object>()).Select(s => s.GetType()),
            });
            return services;
        }
    }
}