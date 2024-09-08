using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item has Property")]
    [Description("Returns true if the chosen Runtime Item has the specified item Property")]

    [Category("Inventory/Properties/Runtime Item has Property")]
    
    [Parameter("Runtime Item", "The Runtime Item type to check")]
    [Parameter("Property ID", "The item property ID to check")]
    
    [Keywords("Inventory", "Contains", "Exists")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Blue, typeof(OverlayDot))]
    
    [Serializable]
    public class ConditionInventoryRuntimeItemHasProperty : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"does {this.m_RuntimeItem} have {this.m_PropertyId}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            return runtimeItem?.Item.Properties.Get(this.m_PropertyId, runtimeItem.Item) != null;
        }
    }
}
