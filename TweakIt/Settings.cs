using UnityModManagerNet;

namespace TweakIt
{
    public class Settings: UnityModManager.ModSettings
    {
        public bool SortChestsByName { get; set; } = true;

        public bool DefaultCraftToMax { get; set; } = true;

        public bool DetailedNotifications { get; set; } = true;

        public bool RemoveCookingStun { get; set; } = true;

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}