using System.Collections;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Stats/Status Effect List UI")]
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoStatusEffects.png")]
    public class StatusEffectListUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField] private StatusEffectTypeMask m_Types =
            StatusEffectTypeMask.Positive |
            StatusEffectTypeMask.Negative |
            StatusEffectTypeMask.Neutral;

        [SerializeField] private bool m_ShowHidden = false;

        [SerializeField] private RectTransform m_Container;
        [SerializeField] private GameObject m_PrefabStatusEffect;

        // MEMBERS: -------------------------------------------------------------------------------

        private GameObject m_LastTarget;
        private Args m_Args;

        private Dictionary<IdString, StatusEffectUI> m_StatusEffects;

        // PROPERTIES: ----------------------------------------------------------------------------

        public GameObject Target
        {
            set
            {
                this.m_Target = GetGameObjectInstance.Create(value);
                this.RefreshStatusEffects();
            }
        }
        
        private bool IsInitialized { get; set; }

        // INITIALIZER: ---------------------------------------------------------------------------

        private void Awake()
        {
            this.m_Args = new Args(this.gameObject);
            this.m_StatusEffects = new Dictionary<IdString, StatusEffectUI>();
        }

        private IEnumerator Start()
        {
            yield return null;
            
            this.RefreshStatusEffects();
            this.IsInitialized = true;
        }
        
        private void OnEnable()
        {
            if (!this.IsInitialized) return;
            this.RefreshStatusEffects();
        }

        private void OnDisable()
        {
            if (ApplicationManager.IsExiting) return;
            if (this.m_LastTarget == null) return;

            Traits lastTraits = this.m_LastTarget.Get<Traits>();
            if (lastTraits != null)
            {
                lastTraits.RuntimeStatusEffects.EventChange -= this.OnChangeStatusEffects;
            }
            
            this.m_LastTarget = null;
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        private void OnChangeStatusEffects(IdString statusEffectID)
        {
            this.RefreshStatusEffects();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshStatusEffects()
        {
            this.UpdateTargetEvents();
            if (this.m_LastTarget == null) return;

            Traits traits = this.m_LastTarget.Get<Traits>();
            if (traits == null) return;

            List<IdString> activeList = traits.RuntimeStatusEffects.GetActiveList();
            List<IdString> removeCandidates = new List<IdString>(this.m_StatusEffects.Keys);

            int allowedTypes = (int) this.m_Types;

            foreach (IdString activeStatusEffect in activeList)
            {
                StatusEffect statusEffect = traits
                    .RuntimeStatusEffects
                    .GetActiveStatusEffect(activeStatusEffect);

                if (statusEffect == null) continue;

                int statusEffectType = (int) statusEffect.Type;
                if ((allowedTypes & statusEffectType) == 0) continue;
                if (statusEffect.IsHidden && !this.m_ShowHidden) continue;
                
                if (this.m_StatusEffects.ContainsKey(activeStatusEffect))
                {
                    removeCandidates.Remove(activeStatusEffect);
                    this.m_StatusEffects[activeStatusEffect].Repaint();
                }
                else
                {
                    GameObject instance = Instantiate(this.m_PrefabStatusEffect, this.m_Container);
                    StatusEffectUI statusEffectUI = instance.Get<StatusEffectUI>();

                    this.m_StatusEffects.Add(activeStatusEffect, statusEffectUI);
                    statusEffectUI.Target(traits, activeStatusEffect);
                }
            }

            for (int i = removeCandidates.Count - 1; i >= 0; --i)
            {
                IdString removeKey = removeCandidates[i];
                StatusEffectUI statusEffectUI = this.m_StatusEffects[removeKey];

                Destroy(statusEffectUI.gameObject);
                this.m_StatusEffects.Remove(removeKey);
            }
        }

        private void UpdateTargetEvents()
        {
            GameObject currentTarget = this.m_Target.Get(this.m_Args);
            if (this.m_LastTarget == currentTarget) return;
            
            if (this.m_LastTarget != null)
            {
                Traits lastTraits = this.m_LastTarget.Get<Traits>();
                if (lastTraits != null)
                {
                    lastTraits.RuntimeStatusEffects.EventChange -= this.OnChangeStatusEffects;
                }
            }

            this.m_LastTarget = currentTarget;
            if (currentTarget == null) return;
            
            Traits currentTraits = currentTarget.Get<Traits>();
            if (currentTraits != null)
            {
                currentTraits.RuntimeStatusEffects.EventChange += this.OnChangeStatusEffects;
            }
        }
    }
}
