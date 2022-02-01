using System.Collections.Generic;

namespace CompQComponents.Services
{
    public interface IElementRegistry
    {
        public Dictionary<string, object> ElementMap { get; set; }
    }
}