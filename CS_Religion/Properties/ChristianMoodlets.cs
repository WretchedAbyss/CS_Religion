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

namespace CS_Religion_Moodlet_Loader
{
    internal class BuffBooter
    {
        public void LoadBuffData()
        {
            AddBuffs(null);
            UIManager.NewHotInstallStoreBuffData = (UIManager.NewHotInstallStoreBuffCallback)Delegate.Combine(UIManager.NewHotInstallStoreBuffData, new UIManager.NewHotInstallStoreBuffCallback(AddBuffs));
        }

        public void AddBuffs(ResourceKey[] resourcekey)
        {
            XmlDbData xmldbdata = XmlDbData.ReadData(new ResourceKey(ResourceUtils.HashString64("Bufflist_ReligionMoodlets"), 53690476u, 0u), false);
            if (xmldbdata != null)
            {
                BuffManager.ParseBuffData(xmldbdata, true);
            }
        }

    }
    public static class Instantiator
    {
        [Tunable]
        internal static bool kinstantiator2 = false;
        static Instantiator()
        {
            LoadSaveManager.ObjectGroupsPreLoad += OnPreLoad;

        }
        public static void OnPreLoad()
        {
            new BuffBooter().LoadBuffData();
        }
    }
}
namespace CS_Religion_Moodlets
{
    public class BuffChristianShameMoodlet :Buff
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

