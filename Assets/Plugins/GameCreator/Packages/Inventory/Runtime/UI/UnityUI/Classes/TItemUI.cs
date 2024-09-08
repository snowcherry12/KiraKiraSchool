using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Serializable]
    public abstract class TItemUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private TextReference m_Name = new TextReference();
        [SerializeField] private TextReference m_Description = new TextReference();
        
        [SerializeField] private Image m_Icon;
        [SerializeField] private Graphic m_Color;

        [SerializeField] private GameObject m_ActiveIsUsable;
        [SerializeField] private GameObject m_ActiveIsCraftable;
        [SerializeField] private GameObject m_ActiveIsDismantable;
        [SerializeField] private GameObject m_ActiveIsDroppable;
        [SerializeField] private GameObject m_ActiveIsEquippable;
        
        [SerializeField] private TextReference m_Width = new TextReference();
        [SerializeField] private TextReference m_Height = new TextReference();
        [SerializeField] private TextReference m_Weight = new TextReference();
        [SerializeField] private PriceUI m_Price;

        [SerializeField] private GameObject m_ActiveHasProperties;
        [SerializeField] private GameObject m_PrefabProperty;
        [SerializeField] private RectTransform m_PropertiesContent;
        
        [SerializeField] private GameObject m_ActiveHasSockets;
        [SerializeField] private TextReference m_SocketsCount = new TextReference();
        [SerializeField] private GameObject m_PrefabSocket;
        [SerializeField] private RectTransform m_SocketsContent;

        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected void RefreshItemUI(Bag bag, Item item, bool forceSingleChunk)
        {
            bool asChunks = !forceSingleChunk && bag != null && bag.Content.CellsAsChunks;
            Args args = bag != null ? bag.Args : null;
            Info itemInfo = item != null ? item.Info : null;

            this.m_Name.Text = itemInfo?.Name(args) ?? string.Empty;
            this.m_Description.Text = itemInfo?.Description(args) ?? string.Empty;
            
            if (this.m_Icon != null) this.m_Icon.overrideSprite = this.GetSprite(item, asChunks, args);
            if (this.m_Color != null) this.m_Color.color = itemInfo?.Color(args) ?? Color.black;

            if (this.m_ActiveIsUsable)
            {
                bool canUse = item != null && Usage.RunCanUse(item, args);
                this.m_ActiveIsUsable.SetActive(canUse);
            }
            
            if (this.m_ActiveIsCraftable)
            {
                bool canCraft = item != null && item.Crafting.AllowToCraft;
                this.m_ActiveIsCraftable.SetActive(canCraft);
            }
            
            if (this.m_ActiveIsDismantable)
            {
                bool canDismantle = item != null && item.Crafting.AllowToDismantle;
                this.m_ActiveIsDismantable.SetActive(canDismantle);
            }

            if (this.m_ActiveIsDroppable)
            {
                bool canDrop = item != null && item.HasPrefab && item.CanDrop;
                this.m_ActiveIsDroppable.SetActive(canDrop);
            }

            if (this.m_ActiveIsEquippable)
            {
                bool canEquip = item != null && item.Equip.IsEquippable;
                this.m_ActiveIsEquippable.SetActive(canEquip);
            }
        }

        protected void RefreshRuntimeItemUI(Bag bag, RuntimeItem runtimeItem)
        {
            Shape itemShape = runtimeItem?.Item != null ? runtimeItem.Item.Shape : null;
                        
            this.m_Width.Text = itemShape?.Width.ToString() ?? string.Empty;
            this.m_Height.Text = itemShape?.Height.ToString() ?? string.Empty;
            this.m_Weight.Text = itemShape?.Weight.ToString() ?? string.Empty;

            if (this.m_Price != null) this.m_Price.RefreshUI(runtimeItem);
            
            int socketsCount = runtimeItem?.Sockets?.Count ?? 0;
            this.m_SocketsCount.Text = socketsCount > 0
                ? socketsCount.ToString() 
                : string.Empty;

            if (this.m_ActiveHasSockets != null)
            {
                this.m_ActiveHasSockets.SetActive(socketsCount > 0);
            }
            
            if (this.m_PrefabSocket != null && this.m_SocketsContent != null)
            {
                RectTransformUtils.RebuildChildren(
                    this.m_SocketsContent,
                    this.m_PrefabSocket,
                    socketsCount
                );
                
                if (runtimeItem != null)
                {
                    List<IdString> sockets = runtimeItem.Sockets != null
                        ? new List<IdString>(runtimeItem.Sockets.Keys)
                        : new List<IdString>();
                    
                    for (int i = 0; i < socketsCount; ++i)
                    {
                        SocketUI socketUI = this.m_SocketsContent.GetChild(i).Get<SocketUI>();
                        if (socketUI != null)
                        {
                            RuntimeSocket runtimeSocket = runtimeItem.Sockets[sockets[i]];
                            socketUI.RefreshUI(bag, runtimeItem, runtimeSocket);
                        }
                    }   
                }
            }

            List<RuntimeProperty> properties = new List<RuntimeProperty>();
            if (runtimeItem != null)
            {
                foreach (KeyValuePair<IdString, RuntimeProperty> entry in runtimeItem.Properties)
                {
                    if (!entry.Value.IsVisible(runtimeItem)) continue;
                    properties.Add(entry.Value);
                }
            }

            int propertiesCount = properties.Count;

            if (this.m_ActiveHasProperties != null)
            {
                this.m_ActiveHasProperties.SetActive(propertiesCount > 0);
            }
            
            if (this.m_PrefabProperty != null && this.m_PropertiesContent != null)
            {
                RectTransformUtils.RebuildChildren(
                    this.m_PropertiesContent,
                    this.m_PrefabProperty,
                    propertiesCount
                );
            
                if (runtimeItem != null)
                {
                    properties.Sort(this.SortProperties);
                    
                    for (int i = 0; i < properties.Count; ++i)
                    {
                        PropertyUI propertyUI = this.m_PropertiesContent.GetChild(i).Get<PropertyUI>();
                        if (propertyUI != null) propertyUI.RefreshUI(bag, runtimeItem, properties[i]);
                    }   
                }
            }
        }
        
        protected virtual Sprite GetSprite(Item item, bool asChunks, Args args)
        {
            return item == null ? null : item.Info?.Sprite(args);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private int SortProperties(RuntimeProperty x, RuntimeProperty y)
        {
            return string.Compare(x.ID.String, y.ID.String, StringComparison.InvariantCulture);
        }
    }
}