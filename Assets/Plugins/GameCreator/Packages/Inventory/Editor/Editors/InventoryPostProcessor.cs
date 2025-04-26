using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;

namespace GameCreator.Editor.Inventory
{
    public class InventoryPostProcessor : AssetPostprocessor
    {
        public static event Action EventRefresh;
        
        // PROCESSORS: ----------------------------------------------------------------------------

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            SettingsWindow.InitRunners.Add(new InitRunner(
                SettingsWindow.INIT_PRIORITY_LOW,
                CanRefreshItems,
                RefreshItems
            ));
        }
        
        private static void OnPostprocessAllAssets(
            string[] importedAssets, 
            string[] deletedAssets, 
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (importedAssets.Length == 0 && deletedAssets.Length == 0) return;
            RefreshItems();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static bool CanRefreshItems()
        {
            return true;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public static void RefreshItems()
        {
            string[] itemSettingsGuids = AssetDatabase.FindAssets($"t:{nameof(InventorySettings)}");
            if (itemSettingsGuids.Length == 0) return;

            string itemSettingsPath = AssetDatabase.GUIDToAssetPath(itemSettingsGuids[0]);
            
            InventorySettings itemSettings = AssetDatabase.LoadAssetAtPath<InventorySettings>(itemSettingsPath);
            if (itemSettings == null) return;

            string[] itemsGuids = AssetDatabase.FindAssets($"t:{nameof(Item)}");
            List<Item> items = new List<Item>(itemsGuids.Length);

            foreach (string itemGuid in itemsGuids)
            {
                string itemPath = AssetDatabase.GUIDToAssetPath(itemGuid);
                Item item = AssetDatabase.LoadAssetAtPath(itemPath, typeof(Item)) as Item;
                if (item != null && item.GetType() == typeof(Item))
                {
                    items.Add(AssetDatabase.LoadAssetAtPath<Item>(itemPath));   
                }
            }
            
            SerializedObject itemSettingsSerializedObject = new SerializedObject(itemSettings);
            SerializedProperty globalVariablesProperty = itemSettingsSerializedObject
                .FindProperty(TAssetRepositoryEditor.NAMEOF_MEMBER)
                .FindPropertyRelative("m_Items")
                .FindPropertyRelative("m_Items");
                
            globalVariablesProperty.arraySize = items.Count;
            for (int i = 0; i < items.Count; ++i)
            {
                globalVariablesProperty.GetArrayElementAtIndex(i).objectReferenceValue = items[i];
            }
            
            itemSettingsSerializedObject.ApplyModifiedPropertiesWithoutUndo();
            EventRefresh?.Invoke();
        }
    }
}
