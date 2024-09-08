using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Coin UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoCurrencyUI.png")]
    
    public class CoinUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private TextReference m_CoinName = new TextReference();
        [SerializeField] private TextReference m_CoinAmount = new TextReference();
        
        [SerializeField] private Graphic m_CoinColor;
        [SerializeField] private Image m_CoinImage;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RefreshUI(Coin coin, int amount)
        {
            this.m_CoinName.Text = coin.Name;
            this.m_CoinAmount.Text = amount.ToString();
            
            if (this.m_CoinColor != null) this.m_CoinColor.color = coin.Tint;
            if (this.m_CoinImage != null) this.m_CoinImage.overrideSprite = coin.Icon;
        }
    }
}