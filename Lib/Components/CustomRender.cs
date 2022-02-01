using System;
using System.Collections.Generic;

namespace CompQComponents.Lib.Components
{    public class CustomRender<T> : QComponent<T> where T: Surrogate
    {
        protected override IEnumerable<QComponent>? Children {
            get => Render;
            set { }
        }

        public IEnumerable<QComponent>? Render;
    }
}