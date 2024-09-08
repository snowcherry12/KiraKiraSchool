using GameCreator.Editor.Installs;
using UnityEditor;

namespace GameCreator.Editor.Stats
{
    public static class UninstallStats
    {
        [MenuItem(
            itemName: "Game Creator/Uninstall/Stats",
            isValidateFunction: false,
            priority: UninstallManager.PRIORITY
        )]
        
        private static void Uninstall()
        {
            UninstallManager.Uninstall("Stats");
        }
    }
}