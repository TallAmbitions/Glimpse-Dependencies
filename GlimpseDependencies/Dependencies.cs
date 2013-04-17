// Copyright (c) 2011 Tall Ambitions, LLC
// See included LICENSE for details.

using Glimpse.AspNet.Extensibility;

namespace Tall.Glimpse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using global::Glimpse.Core.Extensibility;

    public class Dependencies : AspNetTab
    {
        internal const string Dependency = "Tall.Glimpse.Dependencies";
        static List<GlimpseDependencyMetadata> history;
        static readonly IEqualityComparer<GlimpseDependencyMetadata> comparer = new GlimpseDependencyComparer();

        public Dependencies()
        {
            if (Interlocked.CompareExchange(ref history, new List<GlimpseDependencyMetadata>(), null) == null)
            {
                DependencyResolver.SetResolver(new GlimpseDependencyResolver(DependencyResolver.Current));
            }
        }

        public override string Name
        {
            get { return "Dependencies"; }
        }

        public override object GetData(ITabContext context)
        {
            var header = new [] { "Call", "Requested Type", "Returned Types" };

            var newData = context.TabStore.Get(Dependency) as IList<GlimpseDependencyMetadata>;

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

        static Func<GlimpseDependencyMetadata, string[]> GetData(string @class = null)
        {
            return data => new []
                               {
                                   data.Call,
                                   data.RequestedType.FullName,
                                   String.Join(", ", data.ReturnedTypes.Select(t => t.FullName)),
                                   @class,
                               };
        }

        class GlimpseDependencyComparer : IEqualityComparer<GlimpseDependencyMetadata>
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