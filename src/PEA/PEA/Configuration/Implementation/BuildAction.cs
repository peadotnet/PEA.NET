namespace Pea.Configuration.Implementation
{
    public enum ActionTypes
    {
        Add,
        Remove,
        Clear
    }

    public class BuildAction<T>
    {
        public ActionTypes Type { get; }
        public T ObjectClass { get; }

        public BuildAction(ActionTypes type, T objectClass)
        {
            Type = type;
            ObjectClass = objectClass;
        }
    }
}
