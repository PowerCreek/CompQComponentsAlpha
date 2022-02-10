using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CompQComponents.Lib.Components
{
    /// <summary>
    /// Provides all of the Event Listeners which the dom provides.
    /// </summary>
    public class EventItems
    {
        public static EventCallbackItem<MouseEventArgs> OnMouseDown => CreateDefaultItem<MouseEventArgs>();
        public static EventCallbackItem<MouseEventArgs> OnMouseUp => CreateDefaultItem<MouseEventArgs>();
        public static EventCallbackItem<MouseEventArgs> OnMouseMove => CreateDefaultItem<MouseEventArgs>();
        public static EventCallbackItem<MouseEventArgs> OnMouseOver => CreateDefaultItem<MouseEventArgs>();
        public static EventCallbackItem<MouseEventArgs> OnMouseLeave => CreateDefaultItem<MouseEventArgs>();
        public static EventCallbackItem<MouseEventArgs> OnMouseOut => CreateDefaultItem<MouseEventArgs>();
        
        public static EventCallbackItem<ChangeEventArgs> OnInput => CreateDefaultItem<ChangeEventArgs>();

        /// <summary>
        /// Creates the default Event Listener which also includes its respective dom event name.
        /// The name is its name in this project context, lowercase.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <typeparam name="Args"></typeparam>
        /// <returns></returns>
        private static EventCallbackItem<Args> CreateDefaultItem<Args>([CallerMemberName]string propertyName="") where Args: EventArgs =>
            new()
            {
                EventName = propertyName.ToLowerInvariant(),
            };
    }

    public static class EventItemsExt
    {
        public static EventCallbackItem AddEventListener<T>(this EventCallbackItem<T> source, Action<EventArgs> action) where T: EventArgs
        {
            return source.SetCallBackAction<EventArgs>(action);
        }
        
        public static HashSet<EventCallbackItem>? RemoveEventListener<T>(this HashSet<EventCallbackItem>? source, EventCallbackItem<T> item, Action<EventArgs>? action) where T: EventArgs
        {
            if (source is null) return null;
            
            if(source!.TryGetValue(item, out var eventCallbackItem))
                eventCallbackItem.SetCallback -= (action);

            return source;
        }
    }
    
}