using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Animations))]
    public class AnimationsDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Weapon Animations";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty idle = property.FindPropertyRelative("m_Idle");
            SerializedProperty empty = property.FindPropertyRelative("m_Empty");
            SerializedProperty shoot = property.FindPropertyRelative("m_Shoot");
            SerializedProperty reloadQuick = property.FindPropertyRelative("m_ReloadQuick");
            SerializedProperty reloadDry = property.FindPropertyRelative("m_ReloadDry");
            SerializedProperty jamEnter = property.FindPropertyRelative("m_JamEnter");
            SerializedProperty jamExit = property.FindPropertyRelative("m_JamExit");
            SerializedProperty jammed = property.FindPropertyRelative("m_Jammed");
            SerializedProperty chargeProgressMin = property.FindPropertyRelative("m_ChargeProgressMin");
            SerializedProperty chargeProgressMax = property.FindPropertyRelative("m_ChargeProgressMax");
            
            container.Add(new SpaceSmaller());
            container.Add(new PropertyField(idle));
            container.Add(new PropertyField(empty));
            container.Add(new PropertyField(shoot));
            
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(reloadQuick));
            container.Add(new PropertyField(reloadDry));
            
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(jamEnter));
            container.Add(new PropertyField(jamExit));
            container.Add(new PropertyField(jammed));
            
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(chargeProgressMin));
            container.Add(new PropertyField(chargeProgressMax));
        }
    }
}