using Sims3.Gameplay.ActorSystems;
using System;

namespace CS_Religion.Buffs
{
    public class BuffChristianShameMoodlet : Buff
    {
        private const ulong kChristianShameBuffGuid = 0x6939E94EF4DF3135; //Hashed ChristianShameBuff == 0x6939E94EF4DF3135

        public static ulong StaticGuid
        {
            get
            {
                return 0x6939E94EF4DF3135;
            }
        }
        public BuffChristianShameMoodlet(BuffData info) : base(info)
        {

        }
    }
}