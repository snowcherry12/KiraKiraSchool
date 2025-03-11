using GameCreator.Editor.Installs;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    public static class UninstallShooter
    {
        [MenuItem(
            itemName: "Game Creator/Uninstall/Shooter",
            isValidateFunction: false,
            priority: UninstallManager.PRIORITY
        )]
        
        private static void Uninstall()
        {
            UninstallManager.Uninstall("Shooter");
        }
    }
}