using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Pea.ActorModel.Messages;
using Pea.Akka.Actors;
using Pea.Akka.Messages;
using Pea.Core;

namespace Pea.Akka
{
    public class AkkaSystemProvider
    {
        public ActorSystem System { get; }
        public IActorRef SystemActor { get; }

        public AkkaSystemProvider()
        {
            var configString = @"akka {  
                    stdout-loglevel = DEBUG
                    loglevel = DEBUG
                    log-config-on-start = on        
                    actor {
                        debug {  
                            receive = on 
                            autoreceive = on
                            lifecycle = on
                            event-stream = on
                            unhandled = on
                        }
                        provider = Akka.Actor.LocalActorRefProvider
                        guardian-supervisor-strategy = Akka.Actor.DefaultSupervisorStrategy
                    }
                }";

            var config = ConfigurationFactory.ParseString(configString);
            System = ActorSystem.Create("PEA", config);
            SystemActor = System.ActorOf(PeaSystemActor.CreateProps());
        }

        public async Task Start(PeaSettings settings, IEvaluationInitData initData)
        {
            var response = await SystemActor.Ask(new CreateSystem(settings));
            if (response is CreatedSuccessfully)
            {
                var result = await SystemActor.Ask(new InitEvaluator(initData));
            }
        }
    }
}
