namespace CompQComponents.Lib.Attributes
{
    
    /// <summary>
    /// Interface extension of IAttributeContent to allow for a more elegant implementation of attribute types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAttributeContent<T> : IAttributeContent
    {
        
    }

    public interface IAttributeContent
    {
        /// <summary>
        /// The attribute name within the dom element's attribute items scope.
        /// </summary>
        public string AttributeName { get; }
        
        /// <summary>
        /// How the Attribute value appears within the dom element's attribute items scope.
        /// </summary>
        public string? AttributeContent { get; }

    }
}