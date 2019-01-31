using UnityModManagerNet;

namespace RemoveStackLimit
{
    public class Settings: UnityModManager.ModSettings
    {
        public bool RemoveForSingles { get; set; }

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}