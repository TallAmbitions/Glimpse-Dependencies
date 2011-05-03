// Copyright (c) 2011 Tall Ambitions, LLC
// See included LICENSE for details.
namespace Tall.Glimpse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class GlimpseDependencyResolver : IDependencyResolver
    {
        private readonly IDependencyResolver resolver;

        public GlimpseDependencyResolver(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        object IDependencyResolver.GetService(Type serviceType)
        {
            var result = this.resolver.GetService(serviceType);
            Store.Add(new GlimpseDependencyMetadata
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
            Store.Add(new GlimpseDependencyMetadata
            {
                Call = "GetServices",
                RequestedType = serviceType,
                ReturnedTypes = (services ?? Enumerable.Empty<object>()).Select(s => s.GetType()),
            });
            return services;
        }

        private static IList<GlimpseDependencyMetadata> Store
        {
            get
            {
                var items = HttpContext.Current.Items;
                var store = items[Dependencies.Dependency] as IList<GlimpseDependencyMetadata>;
                if (store == null)
                {
                    items[Dependencies.Dependency] = store = new List<GlimpseDependencyMetadata>();
                }

                return store;
            }
        }
    }
}