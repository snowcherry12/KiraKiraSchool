using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Price UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoCurrencyUI.png")]
    
    public class PriceUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private GameObject m_PrefabCoin;
        [SerializeField] private RectTransform m_CoinsContent;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RefreshUI(RuntimeItem runtimeItem)
        {
            if (this.m_PrefabCoin == null || this.m_CoinsContent == null) return;
            
            if (runtimeItem == null || runtimeItem.Currency == null) return;
            this.RefreshUI(runtimeItem.Currency, runtimeItem.Price);
        }

        public void RefreshUI(Currency currency, int value)
        {
            if (this.m_PrefabCoin == null || this.m_CoinsContent == null) return;
            if (currency == null) return;

            int coinsLength = currency.Coins.Length;
            RectTransformUtils.RebuildChildren(
                this.m_CoinsContent,
                this.m_PrefabCoin,
                coinsLength
            );
            
            int[] values = currency.Coins.Values(value);

            for (int i = 0; i < coinsLength; ++i)
            {
                Coin coin = currency.Coins[i];
                this.m_CoinsContent.GetChild(i).Get<CoinUI>().RefreshUI(coin, values[i]);
            }
        }
    }
}