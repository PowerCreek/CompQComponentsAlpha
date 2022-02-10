using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace CompQComponents.Lib.Components
{

    public enum FragType
    {
        COMPONENT,
        ELEMENT
    }

    public class FragmentBuilder
    {
        private int Sequence { get; set; } = 0;
        
        /// <summary>
        /// FragType is used to determine how to build the Blazor component context in this class.
        /// </summary>
        private FragType? _fragType { get; set; }

        public RenderTreeBuilder? _RenderBuilder { get; set; }
        
        /// <summary>
        /// The getter of this property auto-increments the Sequence member
        /// in order to stay in sync with the RenderTreeBuilder of Blazor.
        /// </summary>
        private RenderTreeBuilder? RenderBuilder
        {
            get
            {
                Sequence++;
                return _RenderBuilder;
            }
            set => _RenderBuilder = value;
        }

        protected FragmentBuilder(RenderTreeBuilder builder) => 
            RenderBuilder = builder ?? throw new NullReferenceException("builder must not be null");
        
        public static void Create(RenderTreeBuilder b, Func<FragmentBuilder, FragmentBuilder> buildAction) => 
            new FragmentBuilder(b).DoBuild(buildAction);

        private void DoBuild(Func<FragmentBuilder, FragmentBuilder> fragmentBuilder)
        {
            fragmentBuilder(this);
            Close();
        }

        public void Close()
        {
            switch (_fragType)
            {
                case FragType.ELEMENT:
                    _RenderBuilder!.CloseElement();
                    break;
                case FragType.COMPONENT:
                    _RenderBuilder!.CloseComponent();
                    break;
            }
        }

        /// <summary>
        /// This method begins the Component scope of the given component type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FragmentBuilder Open(Type comp)
        {
            SetFragmentType(FragType.COMPONENT);
            RenderBuilder!.OpenComponent(Sequence, comp);
            return this;
        }
        
        /// <summary>
        /// This method begins the Element scope of the given elementType.
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public FragmentBuilder Open(string elementType)
        {
            SetFragmentType(FragType.ELEMENT);
            
            _fragType = FragType.ELEMENT;
            RenderBuilder!.OpenElement(Sequence, elementType);
            return this;
        }

        public FragmentBuilder AddAttribute(string key, object obj)
        {
            RenderBuilder!.AddAttribute(Sequence, key, obj);
            return this;
        }
        
        public FragmentBuilder AddContent(RenderFragment content)
        {
            RenderBuilder!.AddContent(Sequence, content);
            return this;
        }
        
        public FragmentBuilder AddContent(string content)
        {
            RenderBuilder!.AddContent(Sequence, content);
            return this;
        }

        public void SetFragmentType(FragType fragType) => _fragType = _fragType switch
        {
            null => fragType,
            _ => throw new Exception("Fragment type is already defined.")
        };

    }
}