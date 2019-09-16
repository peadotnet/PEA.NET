namespace Pea.Configuration.Implementation
{
    public enum ActionTypes
    {
        Add,
        Remove,
        Clear
    }

    public interface IBuildAction
    {
        ActionTypes Type { get; }
        object Value { get; }
    }

    public class BuildAction<T> : IBuildAction where T: class
    {
        public ActionTypes Type { get; }

        public object Value { get; set; }

        public BuildAction(ActionTypes type, T value)
        {
            Type = type;
            Value = value;
        }
    }
}
