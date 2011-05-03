// Copyright (c) 2011 Tall Ambitions, LLC
// See included LICENSE for details.
namespace Tall.Glimpse
{
    using System;
    using System.Collections.Generic;

    public class GlimpseDependencyMetadata
    {
        public string Call { get; set; }
        public Type RequestedType { get; set; }
        public IEnumerable<Type> ReturnedTypes { get; set; }
    }
}