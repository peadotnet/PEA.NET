using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Configuration
{
    public class ActionListBuilder<T, AC> where T: class where AC : SubProblemBuilder
    {
        protected List<BuildAction<T>> ActionList;
        protected SubProblemBuilder AggregatorBuilder;

        public ActionListBuilder(List<BuildAction<T>> actionList, SubProblemBuilder aggregatorBuilder)
        {
            ActionList = actionList;
            AggregatorBuilder = aggregatorBuilder;
        }

        public AC Add(T configClass)
        {
            var op = new BuildAction<T>(ActionTypes.Add, configClass);
            ActionList.Add(op);
            return (AC)AggregatorBuilder;
        }

        public AC Remove(T configClass)
        {
            var op = new BuildAction<T>(ActionTypes.Remove, configClass);
            ActionList.Add(op);
            return (AC)AggregatorBuilder;
        }

        public AC Clear()
        {
            var op = new BuildAction<T>(ActionTypes.Clear, null);
            ActionList.Add(op);
            return (AC)AggregatorBuilder;
        }
    }
}
