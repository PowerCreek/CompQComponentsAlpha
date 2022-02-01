using System;
using System.Threading.Tasks;
using CompQComponents.Services;
using Microsoft.AspNetCore.Components;


namespace CompQComponents.Lib.Components
{
    public class Surrogate : ComponentBase
    {
        [Parameter] public QComponent? QComponent { get; set; }

        protected override void OnParametersSet()
        {
            QComponent?.Set(this);
        }

        private RenderFragment result;
        
        protected virtual RenderFragment GetContent
        {
            get
            {
                QComponent.HasRendered = true;
                result = RenderComponent(out string? message) ?? result;
                if(message != null) Console.WriteLine(message);
                return result;
            }
        }

        public RenderFragment? RenderComponent(out string? message)
        {
            string name = QComponent.Id;
            
            bool cache = false;
            
            if (cache = (Key != QComponent?.Key))
            {
                message = QComponent.LogCache ? @$"{name}: Triggered: {(QComponent.DEFAULT_KEY.Equals(QComponent.Key) ? "Initial Cache" : "ReCached")}" : null;
                Key = QComponent.Key;
                return ChildContent;
            }
            else if(cache = (Key is null))
            {
                message = QComponent.LogCache ? $"{name}: NullKey: ReCached" : null;
                return ChildContent;
            }

            if (!cache)
            {
                if(QComponent.LogCache)
                    message = $"{name}: Cached";
            }

            message = null;
            return null;
        }
        
        protected object? Key;

        public virtual RenderFragment ChildContent
        {
            get => QComponent!.BuildElement();
            set {}
        }

        public void ChangeState()
        {
            StateHasChanged();
        }

        public void InvokeAsync() => base.InvokeAsync(StateHasChanged);
    }
}