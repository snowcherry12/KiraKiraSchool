using GameCreator.Editor.Installs;
using UnityEditor;

namespace GameCreator.Editor.Quests
{
    public static class UninstallQuests
    {
        [MenuItem(
            itemName: "Game Creator/Uninstall/Quests",
            isValidateFunction: false,
            priority: UninstallManager.PRIORITY
        )]
        
        private static void Uninstall()
        {
            UninstallManager.Uninstall("Quests");
        }
    }
}