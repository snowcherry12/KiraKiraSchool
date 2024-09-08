using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(OverrideAttributes))]
    public class OverrideAttributesDrawer : TOverrideDrawer
    {
        private static readonly IIcon ICON = new IconAttr(ColorTheme.Type.Blue);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Label => "Attributes";
        protected override IIcon Icon => ICON;

        // IMPLEMENT METHODS: ---------------------------------------------------------------------
        
        protected override SerializedProperty GetKeys(SerializedProperty property)
        {
            return property.FindPropertyRelative(OverrideAttributes.NAME_KEYS);
        }

        protected override SerializedProperty GetValues(SerializedProperty property)
        {
            return property.FindPropertyRelative(OverrideAttributes.NAME_VALUES);
        }
    }
}