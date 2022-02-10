using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompQComponents.Lib.Attributes
{
    
    /// <summary>
    /// Data manipulation for handing local styles of an element.
    /// </summary>
    public class StyleAttribute : IAttributeContent<StyleAttribute>
    {
        public string AttributeName => "style";
        public Dictionary<string,string>? StyleMap => _styleMap;
        
        private Dictionary<string,string>? _styleMap;
        private Dictionary<string,string> StyleMapOrNew => _styleMap ??= new Dictionary<string,string>();
        
        public string? AttributeContent => StyleMap is null? null : string.Join("; ", StyleMap!.Select(e => $"{e.Key}: {e.Value}"));

        /// <summary>
        /// Adds an expanded enumerable of styles as tuples (string,string)  to an element's style collection.
        /// Use the JSInterop with this to ensure you do not need to re-render the parent component
        /// to observe the modification.
        /// <param name="styles"></param>
        /// <returns></returns>
        public StyleAttribute WithStyle(params (string key, string value)[]? styles)
        {
            Parallel.ForEach(styles ??= Array.Empty<(string, string)>(),
                (s) =>
                {
                    var (key, value) = s;
                    StyleMapOrNew[key] = value;
                });
            return this;
        }

        /// <summary>
        /// Removes an expanded enumerable of styles by style key strings from an element's style collection.
        /// Use the JSInterop with this to ensure you do not need to re-render the parent component
        /// to observe the modification.
        /// <param name="styles"></param>
        /// <returns></returns>
        public StyleAttribute WithoutStyle(params string[] styles)
        {
            foreach (var s in styles)
            {
                StyleMapOrNew.Remove(s);
            }

            return this;
        }
    }
}