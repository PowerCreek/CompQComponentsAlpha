namespace CompQComponents.Lib.Attributes
{
    public interface IAttributeContent<T> : IAttributeContent
    {
        
    }

    public interface IAttributeContent
    {
        public string AttributeName { get; }
        public string? AttributeContent { get; }

        public void Combine<T>(T attributeContent);
    }
}