using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Image(typeof(IconBagSolid), ColorTheme.Type.Green)]
    
    [Title("Bag")]
    [Category("Inventory/Bag")]
    
    [Description("Remembers the contents of a Bag component")]

    [Serializable]
    public class MemoryBag : Memory
    {
        [SerializeField] private bool m_Shape = true;
        [SerializeField] private bool m_Items = true;
        [SerializeField] private bool m_Wealth = true;
        [SerializeField] private bool m_Equipment = true;
        [SerializeField] private bool m_Cooldowns = true;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Bag";

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Token GetToken(GameObject target)
        {
            return new TokenBag(
                target.Get<Bag>(),
                this.m_Shape,
                this.m_Items,
                this.m_Wealth,
                this.m_Equipment,
                this.m_Cooldowns
            );
        }

        public override void OnRemember(GameObject target, Token token)
        {
            if (token is not TokenBag tokenBag) return;
            
            Bag bag = target.Get<Bag>();
            _ = DeferOnRemember(bag, tokenBag);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private async Task DeferOnRemember(Bag bag, TokenBag tokenBag)
        {
            await Task.Yield();
            if (bag == null || ApplicationManager.IsExiting) return;
            
            if (this.m_Shape) bag.Shape.OnLoad(tokenBag.Shape);
            if (this.m_Items) bag.Content.OnLoad(tokenBag.Items);
            if (this.m_Wealth) bag.Wealth.OnLoad(tokenBag.Wealth);
            if (this.m_Equipment) bag.Equipment.OnLoad(tokenBag.Equipment);
            if (this.m_Cooldowns) bag.Cooldowns.OnLoad(tokenBag.Cooldowns);
        }
    }
}