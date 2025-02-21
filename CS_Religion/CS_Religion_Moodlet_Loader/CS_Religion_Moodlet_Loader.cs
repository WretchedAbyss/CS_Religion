using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using System;
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
                XmlDbData xmldbdata = XmlDbData.ReadData(new ResourceKey(ResourceUtils.HashString64("bufflist_religion_moodlets"), 0x0333406C, 0x0), false);

                if (xmldbdata != null)
                {
                    BuffManager.ParseBuffData(xmldbdata, true);
                }
            }

        }
    public static class Instantiator
    {
        [Tunable]
        internal static bool kInstantiator2 = false;
        static Instantiator()
        {
            LoadSaveManager.ObjectGroupsPreLoad += OnPreLoad;

        }
        public static void OnPreLoad()
        {
            (new BuffBooter()).LoadBuffData();
        }
    }
}