using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomEditor(typeof(Sight))]
    public class SightEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty canShoot = this.serializedObject.FindProperty("m_CanShoot");
            SerializedProperty priority = this.serializedObject.FindProperty("m_Priority");
            SerializedProperty smoothTime = this.serializedObject.FindProperty("m_SmoothTime");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(canShoot));
            root.Add(new SpaceSmallest());
            root.Add(new PropertyField(priority));
            root.Add(new SpaceSmallest());
            root.Add(new PropertyField(smoothTime));
            
            SerializedProperty state = this.serializedObject.FindProperty("m_State");
            SerializedProperty layer = this.serializedObject.FindProperty("m_Layer");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(state));
            root.Add(new SpaceSmallest());
            root.Add(new PropertyField(layer));
            
            SerializedProperty onEnter = this.serializedObject.FindProperty("m_OnEnter");
            SerializedProperty onExit = this.serializedObject.FindProperty("m_OnExit");
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Enter:"));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(onEnter));
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Exit:"));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(onExit));

            SerializedProperty aim = this.serializedObject.FindProperty("m_Aim");
            SerializedProperty biomechanics = this.serializedObject.FindProperty("m_Biomechanics");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(aim));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(biomechanics));
            
            SerializedProperty trajectory = this.serializedObject.FindProperty("m_Trajectory");
            SerializedProperty crosshair = this.serializedObject.FindProperty("m_Crosshair");
            SerializedProperty laser = this.serializedObject.FindProperty("m_Laser");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(trajectory));
            root.Add(new SpaceSmallest());
            root.Add(new PropertyField(crosshair));
            root.Add(new SpaceSmallest());
            root.Add(new PropertyField(laser));
            
            SerializedProperty reloadingUsesFK = this.serializedObject.FindProperty("m_ReloadingUsesFK");
            SerializedProperty reloadingUsesIK = this.serializedObject.FindProperty("m_ReloadingUsesIK");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(reloadingUsesFK));
            root.Add(new SpaceSmallest());
            root.Add(new PropertyField(reloadingUsesIK));
            
            SerializedProperty shootingUsesFK = this.serializedObject.FindProperty("m_ShootingUsesFK");
            SerializedProperty shootingUsesIK = this.serializedObject.FindProperty("m_ShootingUsesIK");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(shootingUsesFK));
            root.Add(new SpaceSmallest());
            root.Add(new PropertyField(shootingUsesIK));
            
            SerializedProperty fixingUsesFK = this.serializedObject.FindProperty("m_FixingUsesFK");
            SerializedProperty fixingUsesIK = this.serializedObject.FindProperty("m_FixingUsesIK");

            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(fixingUsesFK));
            root.Add(new SpaceSmallest());
            root.Add(new PropertyField(fixingUsesIK));
            
            return root;
        }
    }
}