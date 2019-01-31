using System.Collections.Generic;
using System.Linq;
using Harmony12;

namespace TweakIt
{
    public static class HarmonyExtensions
    {
        public static void UnpatchAllOwned(this HarmonyInstance @this) => @this.UnpatchAll(@this.Id);

        public static IEnumerable<Harmony12.Patch> GetPatches(this HarmonyInstance @this, string harmonyID = null)
        {
            var patches = @this.GetPatchedMethods().SelectMany(method =>
            {
                var patch = @this.GetPatchInfo(method);
                return patch.Prefixes.Union(patch.Postfixes).Union(patch.Transpilers);
            });

            if (harmonyID != null) patches = patches.Where(patch => patch.owner == harmonyID);

            return patches;
        }

        public static IEnumerable<Harmony12.Patch> GetOwnedPatches(this HarmonyInstance @this) => @this.GetPatches(@this.Id);
    }
}