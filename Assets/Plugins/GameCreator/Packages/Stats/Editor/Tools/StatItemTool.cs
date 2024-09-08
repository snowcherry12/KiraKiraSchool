using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    public class StatItemTool : TPolymorphicItemTool
    {
        private const string PROP_IS_HIDDEN = "m_IsHidden";
        
        private static readonly IIcon ICON_HIDDEN_ON = new IconVisibleOff(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_HIDDEN_OFF = new IconVisibleOn(ColorTheme.Type.TextNormal);
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private Image m_HeadHiddenImage;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override object Value => this.m_Property.GetValue<StatItem>();
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public StatItemTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override void SetupHeadExtras()
        {
            Button headHidden = new Button(() =>
            {
                this.m_Property.serializedObject.Update();
                SerializedProperty isHidden = this.m_Property.FindPropertyRelative(PROP_IS_HIDDEN);
                isHidden.boolValue = !isHidden.boolValue;

                SerializationUtils.ApplyUnregisteredSerialization(this.m_Property.serializedObject);
                this.UpdateHead();
            });

            this.m_HeadHiddenImage = new Image
            {
                image = this.m_Property.FindPropertyRelative(PROP_IS_HIDDEN).boolValue 
                    ? ICON_HIDDEN_ON.Texture 
                    : ICON_HIDDEN_OFF.Texture
            };

            headHidden.tooltip = "Hidden Stats won't be displayed in the Traits component";
            headHidden.Add(this.m_HeadHiddenImage);
            headHidden.AddToClassList(CLASS_HEAD_BUTTON);
            this.m_Head.Add(headHidden);
        }
        
        protected override void UpdateHead()
        {
            base.UpdateHead();
            this.m_Property.serializedObject.Update();

            SerializedProperty isHidden = this.m_Property.FindPropertyRelative(PROP_IS_HIDDEN);
            this.m_HeadHiddenImage.image = isHidden.boolValue
                ? ICON_HIDDEN_ON.Texture
                : ICON_HIDDEN_OFF.Texture;
        }
    }
}
