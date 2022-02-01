using System;
using System.Collections.Generic;
using System.Reflection;

namespace CompQComponents.Services
{
    public abstract class ServicePass
    {

        Dictionary<Type, PropertyInfo> propertySet = new();
        
        public void Apply(object obj)
        {
            var t = obj.GetType();
            
            foreach (var prop in GetType().GetProperties())
            {            
                propertySet.TryAdd(prop.PropertyType, prop);
            }     
            
            foreach (var prop in t.GetProperties(
                BindingFlags.Public |
                BindingFlags.NonPublic | 
                BindingFlags.Instance))
            {
                if (propertySet.TryGetValue(prop.PropertyType, out var pType))
                {
                    prop.SetValue(obj, pType.GetValue(this));
                }
            }     
        }

        private void Set(object obj)
        {
            var t = obj.GetType();
            foreach (var prop in GetType().GetProperties())
            {
                prop.SetValue(this, t.GetProperty(prop.Name)!.GetValue(obj));
            }   
        }

        public static T FromSource<T>(object obj) where T : ServicePass, new()
        {
            var hold = new T();
            hold.Set(obj);
            return hold;
        }
    }
}