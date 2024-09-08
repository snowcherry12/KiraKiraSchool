using GameCreator.Editor.Installs;
using UnityEditor;

namespace GameCreator.Editor.Stats
{
    public static class UninstallInventory
    {
        [MenuItem(
            itemName: "Game Creator/Uninstall/Inventory",
            isValidateFunction: false,
            priority: UninstallManager.PRIORITY
        )]
        
        private static void Uninstall()
        {
            UninstallManager.Uninstall("Inventory");
        }
    }
}