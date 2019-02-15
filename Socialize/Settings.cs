using UnityModManagerNet;

namespace Socialize
{
    public class Settings: UnityModManager.ModSettings
    {
        public bool ShowFavor { get; set; } = true;

        public bool OrderGiftsByPreference { get; set; } = true;

        public bool ShowGiftOptions { get; set; } = true;

        public bool ShowUnknownGiftOptions { get; set; } = false;

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}