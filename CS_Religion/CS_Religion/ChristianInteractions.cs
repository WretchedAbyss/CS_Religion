using System;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Pools;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Objects;
using CS_Religion.Buffs;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.CAS;
using.

namespace CS_Religion
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class ChistianInteractions
    {

        public static bool IsChristian(Sim actor)
        {
            bool isChristian = actor.HasTrait((TraitNames)0xDB8CE8B9A70DD9FB);
            StyledNotification.Show(new StyledNotification.Format(
                actor.Name + " is Christian: " + isChristian,
                StyledNotification.NotificationStyle.kDebugAlert));
            return isChristian;
        }

        private static bool AreMarried(Sim sim1, Sim sim2)
        {
            if (sim1 == null || sim2 == null)
            {
                StyledNotification.Show(new StyledNotification.Format(
                    "AreMarried: One or both Sims are null",
                    StyledNotification.NotificationStyle.kDebugAlert));
                return false;
            }

            SimDescription desc1 = sim1.SimDescription;
            SimDescription desc2 = sim2.SimDescription;

            if (desc1 == null || desc2 == null)
            {
                StyledNotification.Show(new StyledNotification.Format(
                    "AreMarried: One or both SimDescriptions are null",
                    StyledNotification.NotificationStyle.kDebugAlert));
                return false;
            }

            bool areMarried = desc1.Partner == desc2;
            StyledNotification.Show(new StyledNotification.Format(
                sim1.Name + " and " + sim2.Name + " are married: " + areMarried,
                StyledNotification.NotificationStyle.kDebugAlert));
            return areMarried;
        }
        [Tunable]
        protected static bool kInstantiator = false;
        private static EventListener sSimInstantiatedListener = null;
        private static EventListener sSimAgedUpListener = null;
        private static EventListener onTraitGainedListener = null;
        private static EventListener onReadBibleListener = null;
        // private static EventListener onWoohooListenerHadFirstWoohoo = null;
        private static EventListener onWoohooListenerWooHooed = null;

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

                // Add listeners for all WooHoo location-specific events
                sSimInstantiatedListener = EventTracker.AddListener(EventTypeId.kSimInstantiated, new ProcessEventDelegate(OnSimInstantiated));
                sSimAgedUpListener = EventTracker.AddListener(EventTypeId.kSimAgeTransition, new ProcessEventDelegate(OnSimInstantiated));
                onTraitGainedListener = EventTracker.AddListener(EventTypeId.kTraitGained, OnTraitGained);
                onReadBibleListener = EventTracker.AddListener(EventTypeId.kReadBook, OnReadBible);

                // Add listeners for each WooHoo location
                // onWoohooListenerHadFirstWoohoo = EventTracker.AddListener(EventTypeId.kHadFirstWoohoo, OnWooHoo);
                onWoohooListenerWooHooed = EventTracker.AddListener(EventTypeId.kWooHooed, OnWooHoo);

                StyledNotification.Show(new StyledNotification.Format("WooHoo listeners registered successfully!", StyledNotification.NotificationStyle.kGameMessagePositive));
            }
            catch (Exception exception)
            {
                Exception(exception);
            }
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
        private static ListenerAction OnReadBible(Event e)
        {
            Sim sim = e.Actor as Sim;
            Book book = e.TargetObject as Book;
            string bookname = book.Title;
            if (sim != null && bookname == "The Holy Bible" && sim.IsInActiveHousehold)
            {
                StyledNotification.Show(new StyledNotification.Format("Verify Function Listener " + bookname,
                            StyledNotification.NotificationStyle.kGameMessagePositive));

                sim.SimDescription.TraitManager.AddHiddenElement((TraitNames)0xDB8CE8B9A70DD9FB);
            }

            return ListenerAction.Keep;
        }
        private static ListenerAction OnWooHoo(Event e)
        {
            try
            {
                StyledNotification.Show(new StyledNotification.Format(
    "Test Notification",
    StyledNotification.NotificationStyle.kDebugAlert));

                StyledNotification.Show(new StyledNotification.Format(
            "Event ID = " + e.ToString() + ", Type = " + e.GetType().ToString(),
            StyledNotification.NotificationStyle.kDebugAlert));
                // Handle base Sims 3 WooHooEvent
                WooHooEvent wooHooEvent = e as WooHooEvent;
                StyledNotification.Show(new StyledNotification.Format(
                    "WooHooEvent is " + (wooHooEvent != null ? "valid" : "null"),
                    StyledNotification.NotificationStyle.kDebugAlert));

                if (wooHooEvent != null)
                {
                    Sim actor = wooHooEvent.Actor as Sim; // Adjust property name based on dnSpy
                    Sim target = wooHooEvent.TargetObject as Sim; // Adjust property name based on dnSpy
                    StyledNotification.Show(new StyledNotification.Format(
                        "Actor is " + (actor != null ? actor.Name : "null") + ", Target is " + (target != null ? target.Name : "null"),
                        StyledNotification.NotificationStyle.kDebugAlert));
                    StyledNotification.Show(new StyledNotification.Format("WooHoo event detected for " + (actor?.Name ?? "Unknown") + " and " + (target?.Name ?? "Unknown"),
                        StyledNotification.NotificationStyle.kGameMessagePositive));
                    if (actor != null && target != null)
                    {
                        // Check actor
                        if (IsChristian(actor) && !AreMarried(actor, target))
                        {
                            actor.BuffManager.AddElement(BuffChristianShameMoodlet.StaticGuid, Origin.None);
                            StyledNotification.Show(new StyledNotification.Format(
                                actor.Name + " feels shame for WooHoo outside of marriage.",
                                StyledNotification.NotificationStyle.kGameMessageNegative));
                        }
                        // Check target
                        if (IsChristian(target) && !AreMarried(actor, target))
                        {
                            target.BuffManager.AddElement(BuffChristianShameMoodlet.StaticGuid, Origin.None);
                            StyledNotification.Show(new StyledNotification.Format(
                                target.Name + " feels shame for WooHoo outside of marriage.",
                                StyledNotification.NotificationStyle.kGameMessageNegative));
                        }
                    }
                }
                else
                {
                    StyledNotification.Show(new StyledNotification.Format(
                        "Unexpected event type for kWooHooed: Neither SocialEvent nor WooHooEvent",
                        StyledNotification.NotificationStyle.kDebugAlert));
                }


            }
            catch (Exception exception)
            {
                StyledNotification.Show(new StyledNotification.Format(
                    "Exception in OnWooHoo: " + exception.Message,
                    StyledNotification.NotificationStyle.kDebugAlert));
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
        public static void AddInteractions(Book book)
        {
            try
            {
                book.AddInteraction(ConvertFaith.Singleton, true);
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
        private sealed class ConvertFaith : ImmediateInteraction<Sim, Book>
        {
            public static readonly InteractionDefinition Singleton = new Definition();
            public override bool Run()
            {
                StyledNotification.Show(new StyledNotification.Format("Verify Function " + base.Target.Title,
                            StyledNotification.NotificationStyle.kGameMessagePositive));
                base.Actor.SimDescription.TraitManager.AddHiddenElement((TraitNames)0xDB8CE8B9A70DD9FB);
                return true;
            }
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Book, ConvertFaith>
            {
                public override string GetInteractionName(Sim a, Book target, InteractionObjectPair interaction)
                {
                    return "Convert Faith";
                }
                public override bool Test(Sim a, Book target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }
    }
}
