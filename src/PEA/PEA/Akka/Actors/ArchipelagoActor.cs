using System;
using System.Collections.Generic;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Akka.Messages;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.Core.Island;

namespace Pea.Akka.Actors
{
    public class ArchipelagoActor : ReceiveActor
    {
        public int IslandsCount { get; private set; } = 0;

        private PeaSettings Settings;
        List<IActorRef> Islands = new List<IActorRef>();
        private int ReceivedAcknowledgementsCount = 0;
        private IActorRef _starter;

        public ArchipelagoActor(PeaSettings settings)
        {
            Settings = settings;
            CreateIslands(Settings);
            //TODO: Stacking, Become counting
            Receive<CreatedSuccessfully>(m => CountCreatedIslands());
            
            Receive<InitEvaluator>(m => InitIslands(m));

            Receive<Travel>(m => Transit(m));

            Receive<PeaResult>(m => MergeResults(m));
        }

        private void CreateIslands(PeaSettings settings)
        {
            var key = new string[settings.SubProblemList.Count];
            var parameterSet = new ParameterSet();
            parameterSet.SetValueRange(settings.ParameterSet);
            for (int i = 0; i < settings.SubProblemList.Count; i++)
            {
                key[i] = settings.SubProblemList[i].Encoding.Key;
                parameterSet.SetValueRange(settings.SubProblemList[i].ParameterSet);
            }

            var random = (IRandom)Activator.CreateInstance(Settings.Random, settings.Seed);

            //foreach (var subProblem in settings.SubProblemList)
            //{
            //    string key = subProblem.Encoding.Key;
            //var parameterSet = new ParameterSet(subProblem.ParameterSet);
            int islandsCount = parameterSet.GetInt(ParameterNames.IslandsCount);

                for (int i = 0; i < islandsCount; i++)
                {
                    int seed = random.GetInt(0, Int32.MaxValue);

                    var islandSettings = settings.GetIslandSettings(new MultiKey(key));
                    var islandProps = IslandActor.CreateProps(islandSettings, seed);
                    var actorRef = Context.ActorOf(islandProps);
                    Islands.Add(actorRef);
                    IslandsCount++;
                }
            //}
        }

        private void CountCreatedIslands()
        {
            ReceivedAcknowledgementsCount++;
            if (ReceivedAcknowledgementsCount == IslandsCount)
            {
                Context.Parent.Tell(new CreatedSuccessfully());
            }
        }

        private void InitIslands(InitEvaluator initMessage)
        {

            foreach (var island in Islands)
            {
                island.Tell(new InitEvaluator(initMessage.InitData));
            }
        }

        private void Transit(Travel travel)
        {
            foreach (var island in Islands)
            {
                if (island != Sender)
                {
                    island.Tell(travel);
                }
            }

            if (travel.TravelerType == Migration.TravelerTypes.Best)
			{
                Context.Parent.Tell(travel);
            }
        }

        private void MergeResults(PeaResult result)
        {
            foreach (var island in Islands)
            {
                if (island != Sender)
                {
                    island.Tell(End.Instance);
                }
            }

            Context.Parent.Tell(result);
        }

        public static Props CreateProps(PeaSettings settings)
        {
            var props = Props.Create(() => new ArchipelagoActor(settings));
            return props;
        }
    }
}
