using UnityModManagerNet;

namespace TweakIt
{
    public class Settings: UnityModManager.ModSettings
    {
        public bool ShowFavor { get; set; } = true;

        public bool DefaultCraftToMax { get; set; }

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}