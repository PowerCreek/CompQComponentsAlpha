using System;
using System.Collections.Generic;

namespace CompQComponents.Services
{
    public class ElementRegistry : IElementRegistry
    {
        public Dictionary<string, object> ElementMap { get; set; } = new();

        public ElementRegistry()
        {
        }
    }
}