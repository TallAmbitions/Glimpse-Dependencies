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
    using global::Glimpse.Core.Extensibility;

    [GlimpsePlugin(ShouldSetupInInit = true)]
    public class Dependencies : IGlimpsePlugin
    {
        internal const string Dependency = "Tall.Glimpse.Dependencies";
        private static List<GlimpseDependencyMetadata> history;
        private static readonly IEqualityComparer<GlimpseDependencyMetadata> comparer = new GlimpseDependencyComparer();

        public string Name { get { return "Dependencies"; } }

        public void SetupInit(HttpApplication application)
        {
            if (Interlocked.CompareExchange(ref history, new List<GlimpseDependencyMetadata>(), null) == null)
            {
                DependencyResolver.SetResolver(new GlimpseDependencyResolver(DependencyResolver.Current));
            }
        }

        public object GetData(HttpApplication application)
        {
            var header = new [] { "Call", "Requested Type", "Returned Types" };

            var newData = ((IList<GlimpseDependencyMetadata>)application.Context.Items[Dependency]);

            List<GlimpseDependencyMetadata> oldData;
            lock (history)
            {
                oldData = history.Except(newData, comparer).ToList();
                history.Clear();
                history.Capacity = newData.Count + oldData.Count;
                history.AddRange(oldData.Concat(newData));
            }

            return new [] { header }.Concat(newData.Select(GetData())).Concat(oldData.Select(GetData("quiet"))).ToArray();
        }

        private static Func<GlimpseDependencyMetadata, string[]> GetData(string @class = null)
        {
            return data => new []
                               {
                                   data.Call,
                                   data.RequestedType.FullName,
                                   String.Join(", ", data.ReturnedTypes.Select(t => t.FullName)),
                                   @class,
                               };
        }

        private class GlimpseDependencyComparer : IEqualityComparer<GlimpseDependencyMetadata>
        {
            public bool Equals(GlimpseDependencyMetadata x, GlimpseDependencyMetadata y)
            {
                return x.Call.Equals(y.Call, StringComparison.Ordinal) && x.RequestedType.Equals(y.RequestedType);
            }

            public int GetHashCode(GlimpseDependencyMetadata obj)
            {
                return obj.Call.GetHashCode() ^ (obj.RequestedType.GetHashCode() * 397);
            }
        }
    }
}