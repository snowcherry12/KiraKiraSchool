using GameCreator.Editor.Installs;
using UnityEditor;

namespace GameCreator.Editor.Dialogue
{
    public static class UninstallDialogue
    {
        [MenuItem(
            itemName: "Game Creator/Uninstall/Dialogue",
            isValidateFunction: false,
            priority: UninstallManager.PRIORITY
        )]

        private static void Uninstall()
        {
            UninstallManager.Uninstall("Dialogue");
        }
    }
}
