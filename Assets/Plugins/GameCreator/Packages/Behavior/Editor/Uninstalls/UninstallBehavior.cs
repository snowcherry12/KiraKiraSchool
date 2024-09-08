using GameCreator.Editor.Installs;
using UnityEditor;

namespace GameCreator.Editor.Behavior
{
    public static class UninstallBehavior
    {
        [MenuItem(
            itemName: "Game Creator/Uninstall/Behavior",
            isValidateFunction: false,
            priority: UninstallManager.PRIORITY
        )]
        
        private static void Uninstall()
        {
            UninstallManager.Uninstall("Behavior");
        }
    }
}