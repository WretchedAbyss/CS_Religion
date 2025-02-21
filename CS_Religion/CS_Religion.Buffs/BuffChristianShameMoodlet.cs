using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.UI;

namespace CS_Religion.Buffs
{
    public class BuffChristianShameMoodlet : Buff
    {
        private const ulong kChristianShameBuffGuid = 0x6939E94EF4DF3135;

        public static ulong StaticGuid
        {
            get { return kChristianShameBuffGuid; }
        }

        public BuffChristianShameMoodlet(BuffData info) : base(info)
        {
        }

        public override void OnAddition(BuffManager bm, BuffInstance bi, bool travelReaddition)
        {
            Sim actor = bm.Actor;
            base.OnAddition(bm, bi, travelReaddition);
        }

        public override void OnRemoval(BuffManager bm, BuffInstance bi)
        {
            Sim actor = bm.Actor;
            base.OnRemoval(bm, bi);
        }

        public override void OnTimeout(BuffManager bm, BuffInstance bi, OnTimeoutReasons reason)
        {
            Sim actor = bm.Actor;
            base.OnTimeout(bm, bi, reason);
        }

        public override bool ShouldAdd(BuffManager bm, MoodAxis axisEffected, int moodValue)
        {
            Sim actor = bm.Actor;
            return ChistianInteractions.IsChristian(actor);
        }
    }
}