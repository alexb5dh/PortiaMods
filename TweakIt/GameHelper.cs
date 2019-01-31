using Harmony12;

namespace TweakIt
{
    public static class GameHelper
    {
        public static bool IsWorktable(this AutomataMachineMenuCtr machine)
        {
            return Traverse.Create(machine).Field<bool>("isWorktable").Value;
        }
    }
}