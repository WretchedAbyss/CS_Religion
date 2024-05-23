using System;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.Gameplay.Objects;

namespace CS_Religion
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class DemoClass
    {
        [Tunable]
        protected static bool kInstantiator = false;
        private static EventListener sSimInstantiatedListener = null;
        private static EventListener sSimAgedUpListener = null;

        static DemoClass()
        {
            World.sOnWorldLoadFinishedEventHandler += new EventHandler(OnWorldLoadFinishedHandler);
        }

        public static void OnWorldLoadFinishedHandler(object sender, System.EventArgs e)
        {
            try
            {
                foreach (Sim sim in Sims3.Gameplay.Queries.GetObjects<Sim>())
                {
                    if (sim != null)
                    {
                        AddInteractions(sim);
                    }

                }
            }
            catch (Exception exception)
            {
                Exception(exception);
            }
            try
            {
                foreach (Book book in Sims3.Gameplay.Queries.GetObjects<Book>())
                {
                    if (book != null)
                    {
                        AddInteractions(book);
                    }

                }
            }
            catch (Exception exception)
            {
                Exception(exception);
            }
            sSimInstantiatedListener = EventTracker.AddListener(EventTypeId.kSimInstantiated, new ProcessEventDelegate(OnSimInstantiated));
            sSimAgedUpListener = EventTracker.AddListener(EventTypeId.kSimAgeTransition, new ProcessEventDelegate(OnSimInstantiated));
        }

        protected static ListenerAction OnSimInstantiated(Event e)
        {
            try
            {
                Sim sim = e.TargetObject as Sim;
                if (sim != null)
                {
                    AddInteractions(sim);
                }
            }
            catch (Exception exception)
            {
                Exception(exception);
            }
            return ListenerAction.Keep;
        }

        public static void AddInteractions(Sim sim)
        {
            try
            {
                if (sim.SimDescription.TeenOrAbove)
                {
                    foreach (InteractionObjectPair pair in sim.Interactions)
                    {
                        if (pair.InteractionDefinition.GetType() == ShowNotification.Singleton.GetType())
                        {
                            return;
                        }
                    }
                    sim.AddInteraction(ShowNotification.Singleton);
                }

            }
            catch (Exception exception)
            {
                WriteLog(exception);
                return;

            }
        }
        public static void AddInteractions(Book book)
        {
            try
            {
                book.AddInteraction(Author.Singleton, true);
            }
            catch (Exception exception)
            {
                WriteLog(exception);
                return;
            }
        }


        public static bool Exception(Exception exception)
        {
            try
            {
                return ((IScriptErrorWindow)AppDomain.CurrentDomain.GetData("ScriptErrorWindow")).DisplayScriptError(null, exception);
            }
            catch
            {
                WriteLog(exception);
                return true;
            }
        }
        public static bool WriteLog(Exception exception)
        {
            try
            {
                new ScriptError(null, exception, 0).WriteMiniScriptError();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private sealed class ShowNotification : ImmediateInteraction<Sim, Sim>
        {
            public static readonly InteractionDefinition Singleton = new Definition();
            public override bool Run()
            {
                StyledNotification.Show(new StyledNotification.Format(base.Actor.Name + " has clicked on " + base.Target.Name,
                            StyledNotification.NotificationStyle.kGameMessagePositive));
                return true;
            }
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, ShowNotification>
            {
                public override string GetInteractionName(Sim a, Sim target, InteractionObjectPair interaction)
                {
                    return "Show sim names";
                }
                public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
                public override string[] GetPath(bool bPath)

                {

                    return new string[1] { " Demo Options..." };

                }
            }
        }
        private sealed class Author : ImmediateInteraction<Sim, Book>
        {
            public static readonly InteractionDefinition Singleton = new Definition();
            public override bool Run()
            {
                StyledNotification.Show(new StyledNotification.Format("The Title is " + base.Target.Title,
                            StyledNotification.NotificationStyle.kGameMessagePositive));
                return true;
            }
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Book, Author>
            {
                public override string GetInteractionName(Sim a, Book target, InteractionObjectPair interaction)
                {
                    return "Tell me the author";
                }
                public override bool Test(Sim a, Book target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

    }

}