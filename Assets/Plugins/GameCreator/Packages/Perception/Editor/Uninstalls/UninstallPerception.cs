using GameCreator.Editor.Installs;
using UnityEditor;

namespace GameCreator.Editor.Quests
{
    public static class UninstallPerception
    {
        [MenuItem(
            itemName: "Game Creator/Uninstall/Perception",
            isValidateFunction: false,
            priority: UninstallManager.PRIORITY
        )]
        
        private static void Uninstall()
        {
            UninstallManager.Uninstall("Perception");
        }
    }
}