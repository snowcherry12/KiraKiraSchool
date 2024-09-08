using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Serializable]
    public class RuntimeItemUI : TItemUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private GameObject m_ActiveContent;
        [SerializeField] private GameObject m_ActiveEquipped;
        [SerializeField] private GameObject m_ActiveNotEquipped;
        
        [SerializeField] private Image m_CooldownProgress;
        [SerializeField] private GameObject m_ActiveInCooldown;
        [SerializeField] private GameObject m_ActiveNotCooldown;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public Bag Bag { get; private set; }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void RefreshCooldown(Bag bag, Item item)
        {
            Cooldown cooldown = bag != null ? bag.Cooldowns.GetCooldown(item) : null;
            
            bool isReady = cooldown?.IsReady ?? true;
            float completion = cooldown?.Completion ?? 0f;

            if (this.m_CooldownProgress != null) this.m_CooldownProgress.fillAmount = 1f - completion;
            if (this.m_ActiveInCooldown != null) this.m_ActiveInCooldown.SetActive(!isReady);
            if (this.m_ActiveNotCooldown != null) this.m_ActiveNotCooldown.SetActive(isReady);
        }
        
        public void RefreshUI(Bag bag, RuntimeItem runtimeItem, bool isActive, bool forceSingleChunk)
        {
            this.Bag = bag;

            if (this.m_ActiveContent != null)
            {
                this.m_ActiveContent.SetActive(runtimeItem != null && isActive);
            }

            if (bag != null && this.m_ActiveEquipped != null)
            {
                this.m_ActiveEquipped.SetActive(bag.Equipment.IsEquipped(runtimeItem));
            }
            
            if (bag != null && this.m_ActiveNotEquipped != null)
            {
                this.m_ActiveNotEquipped.SetActive(!bag.Equipment.IsEquipped(runtimeItem));
            }

            this.RefreshItemUI(bag, runtimeItem?.Item, forceSingleChunk);
            this.RefreshRuntimeItemUI(bag, runtimeItem);
        }

        // UI EVENTS: -----------------------------------------------------------------------------

        public void OnHover(RuntimeItem runtimeItem)
        {
            RuntimeItem.UI_LastItemHovered = runtimeItem;
        }

        public void OnSelect(RuntimeItem runtimeItem)
        {
            RuntimeItem.UI_LastItemSelected = runtimeItem;
        }
    }
}