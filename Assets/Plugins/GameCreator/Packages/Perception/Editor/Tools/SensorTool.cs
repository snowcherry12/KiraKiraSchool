using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Perception;

namespace GameCreator.Editor.Perception
{
    public class SensorTool : TPolymorphicItemTool
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Perception/Editor/StyleSheets/Sensor-Head",
            EditorPaths.PACKAGES + "Perception/Editor/StyleSheets/Sensor-Body"
        };
        
        protected override object Value => this.m_Property.GetValue<TSensor>();

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public SensorTool(IPolymorphicListTool parentTool, int index) 
            : base(parentTool, index)
        { }
    }
}