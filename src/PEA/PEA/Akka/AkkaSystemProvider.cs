using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Pea.ActorModel.Messages;
using Pea.Akka.Actors;
using Pea.Akka.Messages;
using Pea.Configuration.Implementation;
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
                    stdout-loglevel = ERROR
                    loglevel = ERROR
                    log-config-on-start = on

                    actor {
                        debug {  
                            receive = off
                            autoreceive = off
                            lifecycle = off
                            event-stream = off
                            unhandled = off
                        }
                        serialize-creators = off
                        serialize-messages = off
                        provider = Akka.Actor.LocalActorRefProvider
                        guardian-supervisor-strategy = Akka.Actor.DefaultSupervisorStrategy
                    }
                }";

            var config = ConfigurationFactory.ParseString(configString);
            System = ActorSystem.Create("PEA", config);
            SystemActor = System.ActorOf(PeaSystemActor.CreateProps());
        }

        public PeaResult Start(PeaSettings settings, IEvaluationInitData initData)
        {

            var inbox = Inbox.Create(System);
            PeaResult result = null;
            try
            {
                inbox.Send(SystemActor, new CreateSystem(settings));
                var response = inbox.Receive(TimeSpan.FromSeconds(30));
                if (response is CreatedSuccessfully)
                {
                    inbox.Send(SystemActor, new InitEvaluator(initData));
                    result = (PeaResult)inbox.Receive(TimeSpan.FromMinutes(30));
                }
            }
            catch (Exception e)
            {
                result = new PeaResult(new List<string>() {e.ToString()}, new List<IEntity>());
            }


            //var response = await SystemActor.Ask(new CreateSystem(settings));

            //if (response is CreatedSuccessfully)
            //{
            //result = await SystemActor.Ask(new InitEvaluator(initData));
            //}

            return result;
        }
    }
}
