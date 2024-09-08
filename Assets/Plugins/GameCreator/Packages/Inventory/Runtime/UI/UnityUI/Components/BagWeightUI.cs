using System;
using System.Collections;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Bag Weight UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoWeightUI.png")]
    
    public class BagWeightUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_FromBag = GetGameObjectPlayer.Create();

        [SerializeField] private TextReference m_WeightCurrent = new TextReference();
        [SerializeField] private TextReference m_WeightMax = new TextReference();

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
            
            if (this.Bag != null) this.Bag.EventChange -= this.OnChange;

            this.Bag = this.m_FromBag.Get<Bag>(this.gameObject);
            if (this.Bag != null) this.Bag.EventChange += this.OnChange;
            
            this.RefreshUI();
        }

        private void OnDisable()
        {
            if (this.Bag != null) this.Bag.EventChange -= this.OnChange;
        }

        private void OnChange()
        {
            this.RefreshUI();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RefreshUI()
        {
            if (this.Bag == null) return;
            
            this.m_WeightCurrent.Text = this.Bag.Content.CurrentWeight.ToString();

            int maxWeight = this.Bag.Shape.MaxWeight;
            this.m_WeightMax.Text = maxWeight < int.MaxValue 
                ? maxWeight.ToString() 
                : "-";
        }
    }
}