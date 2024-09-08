using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomEditor(typeof(Prop))]
    public class PropEditor : UnityEditor.Editor
    {
        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Root;
        private VisualElement m_Sockets;

        private SerializedProperty propertyItem;
        private SerializedProperty propertySockets;

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            this.propertyItem = this.serializedObject.FindProperty("m_Item");
            this.propertySockets = this.serializedObject.FindProperty("m_Sockets");

            PropertyField fieldItem = new PropertyField(this.propertyItem);
            this.m_Sockets = new VisualElement();

            this.m_Root.Add(fieldItem);
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new LabelTitle("Sockets"));
            this.m_Root.Add(this.m_Sockets);

            fieldItem.RegisterValueChangeCallback(_ => this.RefreshSockets());
            this.RefreshSockets();
            
            return this.m_Root;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshSockets()
        {
            this.serializedObject.Update();
            this.m_Sockets.Clear();

            SerializedProperty keys = this.propertySockets.FindPropertyRelative(Prop.PropSockets.NAME_KEYS);
            SerializedProperty values = this.propertySockets.FindPropertyRelative(Prop.PropSockets.NAME_VALUES);

            int count = keys.arraySize;
            for (int i = 0; i < count; ++i)
            {
                SerializedProperty key = keys.GetArrayElementAtIndex(i);
                SerializedProperty value = values.GetArrayElementAtIndex(i);

                string socketName = key.FindPropertyRelative(IdStringDrawer.NAME_STRING)
                    .stringValue;

                var fieldTransform = new PropertyField(value, TextUtils.Humanize(socketName));
                fieldTransform.Bind(this.serializedObject);
                this.m_Sockets.Add(fieldTransform);
            }
        }
    }
}