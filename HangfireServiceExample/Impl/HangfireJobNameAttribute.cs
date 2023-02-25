namespace HangfireServiceExample.Impl
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class HangfireJobNameAttribute : Attribute
    {
        public HangfireJobNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
