namespace Microsoft.WebApplication1.Entities
{
    public abstract class BaseEntity
    {
        //Using non-generic integer types for simplicity and to ease caching logic 
        public virtual int id { get; protected set; }
    }
}