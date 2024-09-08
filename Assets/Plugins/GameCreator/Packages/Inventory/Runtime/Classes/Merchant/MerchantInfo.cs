using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class MerchantInfo
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        public PropertyGetString m_Name = new PropertyGetString("Merchant");
        public PropertyGetString m_Description = GetStringTextArea.Create("Buys and sells goods");
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public string GetName(GameObject gameObject) => this.m_Name.Get(gameObject);
        public string GetDescription(GameObject gameObject) => this.m_Description.Get(gameObject);
    }
}