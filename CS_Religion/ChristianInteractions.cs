using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Pools;
using Sims3.Gameplay.Utilities;
using Sims3.Gameplay.CAS;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.ChildAndTeenUpdates;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.CookingObjects;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace.Enums;

namespace CS_Religion
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class ChistianInteractions
    {
        public static bool IsChristian(Sim actor)
        {
            return actor.HasTrait((TraitNames)0xDB8CE8B9A70DD9FB);
        }
        [Tunable]
        protected static bool kInstantiator = false;
        private static EventListener sSimInstantiatedListener = null;
        private static EventListener sSimAgedUpListener = null;
        private static EventListener onTraitGainedListener = null;

        static ChistianInteractions()
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

            sSimInstantiatedListener = EventTracker.AddListener(EventTypeId.kSimInstantiated, new ProcessEventDelegate(OnSimInstantiated));
            sSimAgedUpListener = EventTracker.AddListener(EventTypeId.kSimAgeTransition, new ProcessEventDelegate(OnSimInstantiated));
            onTraitGainedListener = EventTracker.AddListener(EventTypeId.kTraitGained, OnTraitGained);

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
        private static ListenerAction OnTraitGained(Event e)
        {
            try
            {
                Sim sim = e.Actor as Sim;
                TraitNames traitName = (e as TraitGainedEvent).TraitName;
                if (sim != null && traitName == (TraitNames)0xDB8CE8B9A70DD9FB)
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
                if (sim.SimDescription.TeenOrAbove && IsChristian(sim))
                {
                    foreach (InteractionObjectPair pair in sim.Interactions)
                    {
                        if (pair.InteractionDefinition.GetType() == Pray.Singleton.GetType())
                        {
                            return;
                        }
                    }
                    sim.AddInteraction(Pray.Singleton);
                }

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

        private sealed class Pray : Interaction<Sim, Sim>
        {
            public static readonly InteractionDefinition Singleton = new Definition();
            public override bool Run()
            {
                StandardEntry();
                EnterStateMachine("PlayerPray", "Enter", "x");
                EnterState("x", "Enter");
                AnimateSim("Exit");
                StandardExit();
                return true;
            }
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, Pray>, IOverridesVisualType, IHasCustomThumbnailIcon
            {
                public override string GetInteractionName(Sim a, Sim target, InteractionObjectPair interaction)
                {
                    return "Pray";
                }
                public override string[] GetPath(bool bPath)

                {

                    return new string[1] { "Christian..." };

                }
                public InteractionVisualTypes GetVisualType
                {
                    get
                    {
                        return InteractionVisualTypes.CustomThumbnail;
                    }
                }
                public ThumbnailKey GetCustomThumbnailIcon(Sim actor, IGameObject target)
                {
                    return new ThumbnailKey(ResourceKey.CreatePNGKey("CS_Christian_pie", ResourceUtils.ProductVersionToGroupId(ProductVersion.BaseGame)), ThumbnailSize.Large);

                }
                public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (a.IsSleeping || a.Posture is SwimmingInPool)
                    {
                        return false;

                    }
                    if (a == target)
                    {
                        return true;
                    }
                    return false;
                }

            }
        }
    }
}