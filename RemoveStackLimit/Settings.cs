using UnityModManagerNet;

namespace RemoveStackLimit
{
    public class Settings: UnityModManager.ModSettings
    {
        public int StackSizeForUnstackable { get; set; } = 1;

        public int StackSizeForStackable { get; set; } = 999;

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}