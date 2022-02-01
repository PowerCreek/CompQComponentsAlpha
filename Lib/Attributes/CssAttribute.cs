using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompQComponents.Lib.Attributes
{
    public class CssAttribute : IAttributeContent<CssAttribute>
    {
        public string AttributeName => "class";

        private HashSet<string>? _classesMap;

        public HashSet<string>? ClassesMap => _classesMap;

        private HashSet<string> ClassMapOrNew => _classesMap ??= new HashSet<string>();

        public string? AttributeContent => ClassesMap is null ? null : string.Join(" ", ClassesMap!);

        public CssAttribute WithClass(params string[]? classes)
        {
            if (classes == null || classes.Length == 0) return this;

            foreach (var t in classes)
            {
                if (t is null) continue;
                ClassMapOrNew.Add(t);
            }

            //  Parallel.ForEach(classes??= Array.Empty<string>(), s=>ClassMapOrNew.Add(s));   
            return this;
        }

        public CssAttribute WithoutClass(params string[]? classes)
        {
            if (classes == null || classes.Length == 0) return this;

            foreach (var t in classes)
            {
                ClassMapOrNew.Remove(t);
            }

            return this;
        }
        
        public void Combine<T>(T attributeContent) => 
            WithClass((attributeContent as CssAttribute)!.ClassMapOrNew.Select(e => e).ToArray());
    }
}