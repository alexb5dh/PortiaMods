using UnityModManagerNet;

namespace TweakIt
{
    public class Settings: UnityModManager.ModSettings
    {
        public bool DefaultCraftToMax { get; set; }

        public bool RemoveCookingStun { get; set; }

        public bool SortChestsByName { get; set; } = true;

        public bool DetailedNotifications { get; set; } = true;

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}