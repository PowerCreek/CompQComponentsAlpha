using System.Collections.Generic;
using System.Linq;

namespace CompQComponents.Lib.Attributes
{
    
    /// <summary>
    /// Data manipulation for handing local classes of an element.
    /// </summary>
    public class CssAttribute : IAttributeContent<CssAttribute>
    {
        public string AttributeName => "class";

        private HashSet<string>? _classesMap;

        public HashSet<string>? ClassesMap => _classesMap;

        private HashSet<string> ClassMapOrNew => _classesMap ??= new HashSet<string>();

        public string? AttributeContent => ClassesMap is null ? null : string.Join(" ", ClassesMap!);

        /// <summary>
        /// Adds a class to an element's class collection.
        /// Use the JSInterop with this to ensure you do not need to re-render the parent component
        /// to observe the modification.
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Removes a class to an element's class collection.
        /// Use the JSInterop with this to ensure you do not need to re-render the parent component
        /// to observe the modification.
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        public CssAttribute WithoutClass(params string[]? classes)
        {
            if (classes == null || classes.Length == 0) return this;

            foreach (var t in classes)
            {
                ClassMapOrNew.Remove(t);
            }

            return this;
        }
    }
}