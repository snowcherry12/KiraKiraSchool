using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Has Runtime Item")]
    [Description("Returns true if the Bag component contains the Item instance")]

    [Category("Inventory/Has Runtime Item")]
    
    [Parameter("Runtime Item", "The item instance to check")]
    [Parameter("Bag", "The targeted Bag")]

    [Keywords("Inventory", "Contains", "Includes", "Wears")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Blue)]
    [Serializable]
    public class ConditionInventoryHasRuntimeItem : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"has {this.m_RuntimeItem} in {this.m_Bag}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            
            Bag bag = this.m_Bag.Get<Bag>(args);
            return bag != null && bag.Content.Contains(runtimeItem);
        }
    }
}
