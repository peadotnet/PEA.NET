using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Configuration
{
    public class ActionListBuilder<T, AC>: SubProblemBuilder where T: class where AC : SubProblemBuilder
    {
        protected List<IBuildAction> ActionList;
        protected SubProblemBuilder AggregatorBuilder;

        public ActionListBuilder(List<IBuildAction> actionList, SubProblemBuilder aggregatorBuilder)
        {
            ActionList = actionList;
            AggregatorBuilder = aggregatorBuilder;
        }

        public AC Add<CT>() where CT: T    //TODO: Add<>
        {
            var op = new BuildAction<Type>(ActionTypes.Add, typeof(CT));
            ActionList.Add(op);
            return (AC)AggregatorBuilder;
        }

        public AC Remove<CT>() where CT: T
        {
            var op = new BuildAction<Type>(ActionTypes.Remove, typeof(CT));
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
