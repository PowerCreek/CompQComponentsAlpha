using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using CompQComponents.Lib.Attributes;
using CompQComponents.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CompQComponents.Lib.Components
{

    public class EventCallbackItem<Args> : EventCallbackItem where Args : EventArgs
    {
        public Action<Args> Callback => CallbackAction!;
    }

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
    
    public abstract class QComponent<Comp> : QComponent where Comp : Surrogate
    {
        public Type SurrogateType = typeof(Comp);
        protected Action<Comp> OnSet;

        public override void Set<T>(T surrogate)
        {
            Wrapper = surrogate;
            OnSet?.Invoke((surrogate as Comp)!);
        }
        //public override QComponent<Comp> SetEvent<T>(Expression<Func<ExprItems, EventTypes>> expr, Action<T> action) => (QComponent<Comp>) base.SetEvent(expr, action);
        
        public T Perform<T>(Action<T> doThing) where T: QComponent<Comp>
        {
            doThing((T)this);
            return (T)this;
        }
    }

    public abstract class QComponent
    {

        public string Tag = "div";
        
        public bool LogCache = false;
        
        private static int _id = 0;
        protected static int NextId => _id++; 

        private int? uniqueId;
        public bool IsUnique = true;

        public object? Key;

        private int _keyTrack = 0;

        public const string DEFAULT_KEY = nameof(DEFAULT_KEY);
        
        public bool NeverSetKey = false;
        
        protected QComponent()
        {
            Key = DEFAULT_KEY;
        }
        
        public void UpdateKey(object? obj = null)
        {
            Key = obj ?? _keyTrack++;
        }

        public void TriggerRender()
        {
            if (!NeverSetKey)
                UpdateKey();
                
            ChangeState();
        }

        public virtual string Id
        {
            get => $"{this.GetType().Name}{(IsUnique?"_"+(uniqueId??=NextId):"")}";
            set { }
        }

        public abstract void Set<T>(T surrogate) where T : Surrogate;
        
        public virtual int? TabIndex { get; set; }

        public Surrogate? Wrapper = null;

        protected virtual IEnumerable<QComponent> Children { get; set; }
        public bool HasRendered = false;

        public IElementRegistry? ElementRegistry;

        public string? Content;

        public void ChangeState() => Wrapper?.ChangeState();
        public void InvokeAsync() => Wrapper?.InvokeAsync();
        
        public List<IAttributeContent>? Attributes;

        public T GetAttribute<T>() where T : class, IAttributeContent, new()
        {
            if((Attributes ??= new()).All(e => e.GetType() != typeof(T)))
            {
                T hold = null;
                Attributes.Add(hold = new T());
                return hold;
            }
            
            return (T) (Attributes??=new List<IAttributeContent>()).Find(e => e.GetType() == typeof(T))!;
        }
        
        public HashSet<EventCallbackItem>? EventContainer;

        public void AddEvent(params EventCallbackItem[]? eventItem)
        {
            if (eventItem == null) return;
            foreach (var item in eventItem)
            {
                EventContainer.SetEvent(item);
            }
        }
        

        public RenderFragment BuildSelf(Type comp)
        {
            return fbuilder =>
            {
                FragmentBuilder.Create(fbuilder, surr => 
                    {
                        surr.Open(comp);
                        if(Key != null) surr._RenderBuilder.SetKey(Key);
                        surr.AddAttribute(nameof(Surrogate.QComponent), this);
                        return surr;
                    }
                );
            };
        }
        
        public RenderFragment BuildElement()
        {
            
            return fbuilder =>
            {
                FragmentBuilder.Create(fbuilder, frag =>
                {
                    frag.Open(Tag);
                    var props = this.GetType().GetProperties();

                    foreach (var prop in props)
                    {
                        if (prop.GetValue(this) is { } val)
                            frag.AddAttribute(prop.Name, val);
                    }

                    if (Attributes != null)
                    {
                        foreach (var attr in Attributes)
                        {
                            if (attr.AttributeContent != null)
                            {
                               frag.AddAttribute(attr.AttributeName, attr.AttributeContent);
                            }
                        }
                    }

                    if (EventContainer != null)
                    {
                        foreach (var eventCallbackItem in EventContainer)
                        {
                            frag.AddAttribute(eventCallbackItem.EventName, eventCallbackItem.SetCallback!);
                        }
                    }

                    if (Content != null)
                    {
                        frag.AddContent(Content);
                    }

                    foreach (var child in Children??Array.Empty<QComponent>())
                    {
                        if (child is null) continue;
                        frag.AddContent(child.BuildSelf(child.WrapperType()));
                    }

                    return frag;
                });
            };
        }
    }

    public static class QCompnentExt
    {
        public static RenderFragment BuildSelf<T>(this QComponent<T> self) where T: Surrogate 
            => self.BuildSelf(self.SurrogateType);
    }
}