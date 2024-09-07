using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    [CustomEditor(typeof(WelcomeSettings))]
    public class WelcomeSettingsEditor : TAssetRepositoryEditor
    {
        private const string MSG = "Welcome to Game Creator";

        protected override void CreateContent(VisualElement root)
        {
            InfoMessage message = new InfoMessage(MSG);
            root.Add(message);

            Button buttonFetchData = new Button(WelcomeManager.ForceFetchData)
            {
                text = "Fetch and Update"
            };
            
            root.Add(buttonFetchData);
        }
    }
}