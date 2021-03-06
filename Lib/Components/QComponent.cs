using System;
using System.Collections.Generic;
using System.Linq;
using CompQComponents.Lib.Attributes;
using Microsoft.AspNetCore.Components;

namespace CompQComponents.Lib.Components
{
    public abstract class QComponent<Comp> : QComponent where Comp : Surrogate
    {
        public Type SurrogateType = typeof(Comp);
        protected Action<Comp> OnSet;

        public override void Set<T>(T surrogate)
        {
            Wrapper = surrogate;
            OnSet?.Invoke((surrogate as Comp)!);
        }
        
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

        /// <summary>
        /// Sets the reference of the Surrogate component which may need to be referenced by the internals of this class.
        /// </summary>
        /// <param name="surrogate"></param>
        /// <typeparam name="T"></typeparam>
        public abstract void Set<T>(T surrogate) where T : Surrogate;
        
        public virtual int? TabIndex { get; set; }

        public Surrogate? Wrapper = null;

        /// <summary>
        /// Property that can be used as a generator function, or provide the IEnumerable of QComponent instances.
        /// </summary>
        protected virtual IEnumerable<QComponent> Children { get; set; }
        public bool HasRendered = false;

        public string? Content;

        public void ChangeState() => Wrapper?.ChangeState();
        public void InvokeAsync() => Wrapper?.InvokeAsync();
        
        public List<IAttributeContent>? Attributes;

        /// <summary>
        /// Creates and/or provides the IAttributeContent via Type argument T.
        /// Attributes show up in the Tag space of elements within the dom.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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
        
        /// <summary>
        /// Contains all events attached to an Event Listener.
        /// </summary>
        public HashSet<EventCallbackItem>? EventContainer = null;

        /// <summary>
        /// Generates the RenderFragment from the type passed in.
        /// comp should be a type that extends ComponentBase.
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public RenderFragment BuildSelf(Type comp)
        {
            return fbuilder =>
            {
                FragmentBuilder.Create(fbuilder, surr => 
                    {
                        surr.Open(comp);
                        if(Key != null) surr._RenderBuilder.SetKey(Key);
                        
                        //This sets the parameter property of QComponent to this (this class instance).
                        surr.AddAttribute(nameof(Surrogate.QComponent), this);
                        return surr;
                    }
                );
            };
        }
        
        /// <summary>
        /// Generates the RenderFragment from all properties of the QComponent.
        /// If there are any properties of this class, they are set in the tag space of the element.
        /// </summary>
        /// <returns></returns>
        public RenderFragment BuildElement() =>
            fbuilder =>
            {
                FragmentBuilder.Create(fbuilder, frag =>
                {
                    frag.Open(Tag);
                    var props = this.GetType().GetProperties();
                    
                    //Iterate over every property within this class, or subclasses,
                    //and add them to the attributes section of the dom element.
                    foreach (var prop in props)
                    {
                        if (prop.GetValue(this) is { } val)
                            frag.AddAttribute(prop.Name, val);
                    }

                    if (Attributes != null)
                    {
                        //Iterate over every Attribute within the Attributes member,
                        //and add them to the attributes section of the dom element by AttributeName and AttributeContent
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
                        //Iterate over every EventCallbackItem and set the referece of
                        //the EventListeners as values of the Event name.
                        foreach (var eventCallbackItem in EventContainer)
                        {
                            frag.AddAttribute(eventCallbackItem.EventName, eventCallbackItem.SetCallback!);
                        }
                    }

                    if (Content != null)
                    {
                        //Add the content member as content of this dom element.
                        frag.AddContent(Content);
                    }

                    //Iterate over every child element and append it as content of this dom element.
                    foreach (var child in Children??Array.Empty<QComponent>())
                    {
                        if (child is null) continue;
                        frag.AddContent(child!.Wrapper?.RequestContent ?? child!.BuildSelf(child.WrapperType()));
                    }

                    return frag;
                });
            };
    }

    public static class QCompnentExt
    {
        public static RenderFragment BuildSelf<T>(this QComponent<T> self) where T: Surrogate 
            => self.BuildSelf(self.SurrogateType);
    }
}