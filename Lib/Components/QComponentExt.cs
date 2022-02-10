using System;
using System.Collections.Generic;
using System.Linq;

namespace CompQComponents.Lib.Components
{
    /// <summary>
    /// Utility extension methods.
    /// </summary>
    public static class QComponentExt
    {

        /// <summary>
        /// Sets EventCallbackItems, expanded as params, to allow the developer to instantiate a new
        /// instance of HashSet_EventCallbackItem while providing the events as needed.
        /// This lowers the complexity of the project, and allows developers to code in a more concise manner
        /// without any negative tradeoffs.
        /// If obj is null, it will generate the HashSet, then add all of the items provides, then return.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static HashSet<EventCallbackItem> SetEvent(this HashSet<EventCallbackItem>? obj, params EventCallbackItem[] items)
        {
            obj ??= new();
            foreach (var item in items)
            {
                if(!obj.TryGetValue(item, out var eventItem))
                {
                    obj.Add(eventItem = item);
                }
                else
                {
                    eventItem.SetCallback += item.SetCallback;
                }
            }
            return obj;
        }
        
        /// <summary>
        /// Provides the Surrogate type a QComponent extends with its Generic Arguments..
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static Type WrapperType(this QComponent comp)
        {
            var componentType = comp.GetType();

            do
            {
                var tempType = componentType.BaseType;
                List<Type> types = componentType.GetInterfaces().Where(e=>e.IsGenericType).ToList();
                if (tempType is not null) types?.Add(tempType);

                var match =
                    types!
                        .Where(e => e.IsGenericType)?
                        .Where(e => e.GetGenericArguments()
                            .Any(a=>a.IsAssignableTo(typeof(Surrogate))))?.First()
                        .GetGenericArguments()
                        .First(e=>e.IsAssignableTo(typeof(Surrogate)));

                if (match != null)
                    return match!;

                componentType = componentType.BaseType;
            } while (componentType != null);

            return null!;
        }
    }
}