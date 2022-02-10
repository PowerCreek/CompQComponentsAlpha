using System;

namespace CompQComponents.Lib.Components
{
    public class EventCallbackItem
    {
        public string EventName { get; init; } = null!;

        protected Action<EventArgs>? CallbackAction { get; set; }

        public Action<EventArgs>? SetCallback;

        public EventCallbackItem SetCallBackAction<T>(Action<EventArgs> t) where T: EventArgs
        {
            SetCallback += t;
            return this;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj == this) return true;
            return obj.GetType() == this.GetType() && EventName.Equals((obj as EventCallbackItem)?.EventName);
        }

        public override int GetHashCode()
        {
            return EventName!.GetHashCode();
        }
    }

    public class EventCallbackItem<Args> : EventCallbackItem where Args : EventArgs
    {
        public Action<Args> Callback => CallbackAction!;
    }
}