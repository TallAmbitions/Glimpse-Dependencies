// Copyright (c) 2011 Tall Ambitions, LLC
// See included LICENSE for details.
namespace Tall.Glimpse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using global::Glimpse.Net.Extensibility;

    [GlimpsePlugin(ShouldSetupInInit = true)]
    public class Dependencies : IGlimpsePlugin
    {
        internal const string Dependency = "Tall.Glimpse.Dependencies";
        private static Dependencies instance;

        public object GetData(HttpApplication application)
        {
            var header = new [] { "Call", "Requested Type", "Returned Types" };
            var calls = ((IList<GlimpseDependencyMetadata>)application.Context.Items[Dependency])
                .Select(data => new []
                    {
                        data.Call,
                        data.RequestedType.FullName,
                        String.Join(", ", data.ReturnedTypes.Select(t => t.FullName)),
                    });

            return new [] { header }.Concat(calls).ToArray();
        }

        public void SetupInit(HttpApplication application)
        {
            if (Interlocked.CompareExchange(ref instance, this, null) == null)
            {
                DependencyResolver.SetResolver(new GlimpseDependencyResolver(DependencyResolver.Current));
            }
        }

        public string Name { get { return "Dependencies"; } }
    }
}