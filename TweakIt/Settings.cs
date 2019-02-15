using UnityModManagerNet;

namespace TweakIt
{
    public class Settings: UnityModManager.ModSettings
    {
        public bool ShowFavor { get; set; }

        public bool OrderGiftsByPreference { get; set; }

        public bool ShowGiftOptions { get; set; }

        public bool ShowUnknownGiftOptions { get; set; }

        public bool DefaultCraftToMax { get; set; }

        public bool RemoveCookingStun { get; set; }

        public bool DetailedNotifications { get; set; } = true;

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}