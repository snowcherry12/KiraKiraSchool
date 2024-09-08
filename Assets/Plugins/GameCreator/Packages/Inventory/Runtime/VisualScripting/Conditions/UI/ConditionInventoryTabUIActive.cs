using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("Is Tab UI Active")]
    [Description("Returns true if the chosen Tab UI component is currently active")]

    [Category("Inventory/UI/Is Tab UI Active")]

    [Keywords("Shop", "Exchange", "Trader")]
    
    [Image(typeof(IconBagOutline), ColorTheme.Type.TextLight, typeof(OverlayTick))]
    [Serializable]
    public class ConditionInventoryTabUIActive : Condition
    {
        [SerializeField] private PropertyGetGameObject m_TabUI = GetGameObjectInstance.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_TabUI} Active";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            BagListUITab tabUI = this.m_TabUI.Get<BagListUITab>(args);
            return tabUI != null && tabUI.IsActive;
        }
    }
}
