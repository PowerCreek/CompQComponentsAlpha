using System;
using Microsoft.AspNetCore.Components.Web;

namespace CompQComponents.Lib.Components
{
    public interface IMouseCallbacks
    {
        Action<MouseEventArgs> OnMouseDown { get; set; }
        Action<MouseEventArgs> OnMouseUp { get; set; }
        Action<MouseEventArgs> OnMouseMove { get; set; }
        Action<MouseEventArgs> OnMouseOver { get; set; }
    }
    
    public interface IKeyCallbacks
    {
        Action<KeyboardEventArgs> OnKeyDown { get; set; }
        Action<KeyboardEventArgs> OnKeyUp { get; set; }
        Action<KeyboardEventArgs> OnKeyPressed { get; set; }
    }

    public class EventCallbackContainer
    {
        
    }
}