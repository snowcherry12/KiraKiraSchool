using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomPropertyDrawer(typeof(TItemUI))]
    public class TItemUIDrawer : TBoxDrawer
    {
        protected sealed override void CreatePropertyContent(VisualElement root, SerializedProperty property)
        {
            SerializedProperty name = property.FindPropertyRelative("m_Name");
            SerializedProperty description = property.FindPropertyRelative("m_Description");
            SerializedProperty image = property.FindPropertyRelative("m_Icon");
            SerializedProperty color = property.FindPropertyRelative("m_Color");
            
            SerializedProperty activeCanUse = property.FindPropertyRelative("m_ActiveIsUsable");
            SerializedProperty activeCanCraft = property.FindPropertyRelative("m_ActiveIsCraftable");
            SerializedProperty activeCanDismantle = property.FindPropertyRelative("m_ActiveIsDismantable");
            SerializedProperty activeCanDrop = property.FindPropertyRelative("m_ActiveIsDroppable");
            SerializedProperty activeCanEquip = property.FindPropertyRelative("m_ActiveIsEquippable");

            this.AddBefore(root, property);
            
            root.Add(new PropertyField(name));
            root.Add(new PropertyField(description));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(image));
            root.Add(new PropertyField(color));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(activeCanUse));
            root.Add(new PropertyField(activeCanCraft));
            root.Add(new PropertyField(activeCanDismantle));
            root.Add(new PropertyField(activeCanDrop));
            root.Add(new PropertyField(activeCanEquip));
            
            SerializedProperty width = property.FindPropertyRelative("m_Width");
            SerializedProperty height = property.FindPropertyRelative("m_Height");
            SerializedProperty weight = property.FindPropertyRelative("m_Weight");
            SerializedProperty price = property.FindPropertyRelative("m_Price");
            SerializedProperty activeHasProperties = property.FindPropertyRelative("m_ActiveHasProperties");
            SerializedProperty prefabProperty = property.FindPropertyRelative("m_PrefabProperty");
            SerializedProperty propertiesContent = property.FindPropertyRelative("m_PropertiesContent");
            SerializedProperty activeHasSockets = property.FindPropertyRelative("m_ActiveHasSockets");
            SerializedProperty socketsCount = property.FindPropertyRelative("m_SocketsCount");
            SerializedProperty prefabSocket = property.FindPropertyRelative("m_PrefabSocket");
            SerializedProperty socketsContent = property.FindPropertyRelative("m_SocketsContent");

            root.Add(new SpaceSmall());
            root.Add(new PropertyField(weight));
            root.Add(new PropertyField(width));
            root.Add(new PropertyField(height));
            root.Add(new PropertyField(price));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(activeHasProperties));
            root.Add(new PropertyField(prefabProperty));
            root.Add(new PropertyField(propertiesContent));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(activeHasSockets));
            root.Add(new PropertyField(socketsCount));
            root.Add(new PropertyField(prefabSocket));
            root.Add(new PropertyField(socketsContent));

            this.AddAfter(root, property);
        }

        protected virtual void AddBefore(VisualElement root, SerializedProperty property)
        { }
        
        protected virtual void AddAfter(VisualElement root, SerializedProperty property)
        { }
    }
}
