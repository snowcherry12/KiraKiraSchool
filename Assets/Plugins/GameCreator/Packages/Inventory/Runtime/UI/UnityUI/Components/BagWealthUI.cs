using System;
using System.Collections;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Bag Wealth UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoCurrencyUI.png")]

    public class BagWealthUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Currency m_Currency;
        [SerializeField] private PropertyGetGameObject m_FromBag = GetGameObjectPlayer.Create();

        [SerializeField] private GameObject m_PrefabCoin;
        [SerializeField] private RectTransform m_CoinsContent;

        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] private Bag Bag { get; set; }

        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            StartCoroutine(this.DeferredOnEnable());
        }

        private IEnumerator DeferredOnEnable()
        {
            yield return null;

            if (this.Bag != null) this.Bag.Wealth.EventChange -= this.OnChangeWealth;

            this.Bag = this.m_FromBag.Get<Bag>(this.gameObject);
            if (this.Bag != null) this.Bag.Wealth.EventChange += this.OnChangeWealth;

            this.RefreshUI();
        }

        private void OnDisable()
        {
            if (this.Bag != null) this.Bag.Wealth.EventChange -= this.OnChangeWealth;
        }

        private void OnChangeWealth(IdString currencyID, int prevAmount, int newAmount)
        {
            this.RefreshUI();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RefreshUI()
        {
            if (this.m_Currency == null) return;
            if (this.Bag == null) return;

            int amount = this.Bag.Wealth.Get(this.m_Currency);
            int[] values = this.m_Currency.Coins.Values(amount);
            
            RectTransformUtils.RebuildChildren(
                this.m_CoinsContent,
                this.m_PrefabCoin,
                values.Length
            );

            for (int i = 0; i < values.Length; ++i)
            {
                Coin coin = this.m_Currency.Coins[i];
                this.m_CoinsContent.GetChild(i).Get<CoinUI>().RefreshUI(coin, values[i]);
            }
        }
    }
}
