using System;
using System.Collections.Generic;
using System.Linq;

namespace CompQComponents.Lib.Attributes
{
    
    /// <summary>
    /// Utility class to provide an elegant way of setting up the element attributes
    /// using chain calls.
    /// </summary>
    public class AttributeBuilder
    {
        public List<IAttributeContent> Values { get; set; } = new List<IAttributeContent>();
        
        public AttributeBuilder Set<T>(Action<T> action) where T: class, IAttributeContent
        {
            try
            {
                var hold = Values.SingleOrDefault(e => e.GetType().IsAssignableFrom(typeof(T)));
                if (!(hold is T))
                {
                    hold = Activator.CreateInstance<T>();
                    Values.Add(hold);
                }

                action((T) hold);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return this;
        }
    }
}