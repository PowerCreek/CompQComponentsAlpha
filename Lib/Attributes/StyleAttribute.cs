using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompQComponents.Lib.Attributes
{
    public class StyleAttribute : IAttributeContent<StyleAttribute>
    {
        public string AttributeName => "style";
        public Dictionary<string,string>? StyleMap => _styleMap;
        private Dictionary<string,string>? _styleMap;
        private Dictionary<string,string> StyleMapOrNew => _styleMap ??= new Dictionary<string,string>();
        
        public string? AttributeContent => StyleMap is null? null : string.Join("; ", StyleMap!.Select(e => $"{e.Key}: {e.Value}"));

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

        public StyleAttribute WithoutStyle(params string[] styles)
        {
            foreach (var s in styles)
            {
                StyleMapOrNew.Remove(s);
            }

            return this;
        }

        public void Combine<T>(T attributeContent) =>
            WithStyle((attributeContent as StyleAttribute)!.StyleMapOrNew.Select(e=>(e.Key, e.Value)).ToArray());
    }
}