using GameCreator.Editor.Installs;
using UnityEditor;

namespace GameCreator.Editor.Melee
{
    public static class UninstallMelee
    {
        [MenuItem(
            itemName: "Game Creator/Uninstall/Melee",
            isValidateFunction: false,
            priority: UninstallManager.PRIORITY
        )]
        
        private static void Uninstall()
        {
            UninstallManager.Uninstall("Melee");
        }
    }
}