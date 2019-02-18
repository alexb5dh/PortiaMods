using UnityModManagerNet;

namespace RemoveStackLimit
{
    public class Settings: UnityModManager.ModSettings
    {
        public int StackSizeForUnstackable { get; set; } = int.MaxValue;

        public int StackSizeForStackable { get; set; } = int.MaxValue;

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}