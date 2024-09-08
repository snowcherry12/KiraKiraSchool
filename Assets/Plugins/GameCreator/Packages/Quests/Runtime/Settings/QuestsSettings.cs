using GameCreator.Runtime.Common;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameCreator.Runtime.Quests
{
    public class QuestsSettings : AssetRepository<QuestsRepository>
    {
        public override IIcon Icon => new IconQuestSolid(ColorTheme.Type.TextLight);
        public override string Name => "Quests";
        
        #if UNITY_EDITOR

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= this.OnChangePlayMode;
            EditorApplication.playModeStateChanged += this.OnChangePlayMode;
            
            this.RefreshQuestList();
        }

        private void OnChangePlayMode(PlayModeStateChange playModeStateChange)
        {
            this.RefreshQuestList();
        }

        private void RefreshQuestList()
        {
            if (AssemblyUtils.IsReloading) return;
            
            string[] questGuids = AssetDatabase.FindAssets($"t:{nameof(Quest)}");
            Quest[] quests = new Quest[questGuids.Length];

            for (int i = 0; i < questGuids.Length; i++)
            {
                string questGuid = questGuids[i];
                string questPath = AssetDatabase.GUIDToAssetPath(questGuid);
                quests[i] = AssetDatabase.LoadAssetAtPath<Quest>(questPath);
            }

            this.Get().Quests.Set(quests);
        }

        #endif
    }
}