using System;
using System.Collections.Generic;
using System.Linq;

namespace CompQComponents.Lib.Components
{
    public static class QComponentExt
    {

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
        
        public static Type WrapperType(this QComponent comp)
        {
            var cType = comp.GetType();

            do
            {
                var hold = cType.BaseType;
                List<Type> types = cType.GetInterfaces().Where(e=>e.IsGenericType).ToList();
                if (hold is not null) types?.Add(hold);

                var match =
                    types!
                        .Where(e => e.IsGenericType)?
                        .Where(e => e.GetGenericArguments()
                            .Any(a=>a.IsAssignableTo(typeof(Surrogate))))?.First()
                        .GetGenericArguments()
                        .First(e=>e.IsAssignableTo(typeof(Surrogate)));

                if (match != null)
                {
                    return match!;
                }

                cType = cType.BaseType;
            } while (cType != null);

            return null!;
        }
    }
}