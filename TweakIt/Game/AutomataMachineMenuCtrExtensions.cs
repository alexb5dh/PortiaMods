using Harmony12;

namespace TweakIt
{
    public static class AutomataMachineMenuCtrExtensions
    {
        public static bool IsWorktable(this AutomataMachineMenuCtr machine) => Traverse.Create(machine).Field<bool>("isWorktable").Value;
    }
}