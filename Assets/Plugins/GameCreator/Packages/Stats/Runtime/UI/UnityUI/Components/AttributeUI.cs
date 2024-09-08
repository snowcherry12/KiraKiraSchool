using System;
using System.Collections;
using System.Globalization;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Stats.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Stats/Attribute UI")]
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoAttribute.png")]
    public class AttributeUI : MonoBehaviour
    {
        private enum UnitMode
        {
            KeepEmptyUnits,
            HideEmptyUnits
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField] private Attribute m_Attribute;

        [SerializeField] private UICommon m_Common = new UICommon();

        [SerializeField] private TextReference m_Value = new TextReference();
        [SerializeField] private TextReference m_Percentage = new TextReference();
        [SerializeField] private TextReference m_MaxValue = new TextReference();
        [SerializeField] private TextReference m_MinValue = new TextReference();

        [SerializeField] private Image m_ImageFill;
        [SerializeField] private RectTransform m_ScaleX;
        [SerializeField] private RectTransform m_ScaleY;

        [SerializeField] private RectTransform m_UnitContainer;
        [SerializeField] private GameObject m_UnitPrefab;
        [SerializeField] private UnitMode m_UnitMode = UnitMode.KeepEmptyUnits;
        [SerializeField] private float m_UnitValue = 10f;

        [SerializeField] private bool m_WhenIncrement;
        [SerializeField] private bool m_WhenDecrement;
        [SerializeField] private float m_StallDuration;
        [SerializeField] private float m_TransitionDuration;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private GameObject m_LastTarget;
        [NonSerialized] private Args m_Args;

        [NonSerialized] private AnimFloat m_ProgressAnimation;
        [NonSerialized] private float m_LastChangeTime = -999f;

        // PROPERTIES: ----------------------------------------------------------------------------

        public GameObject Target
        {
            set
            {
                this.m_Target = GetGameObjectInstance.Create(value);
                this.RefreshValues();
                this.RefreshProgress();
            }
        }

        public Attribute Attribute
        {
            set
            {
                this.m_Attribute = value;
                
                this.RefreshValues();
                this.RefreshProgress();
            }
        }
        
        private bool IsInitialized { get; set; }

        // INITIALIZER: ---------------------------------------------------------------------------

        private void Awake()
        {
            this.m_Args = new Args(this.gameObject);
        }

        private IEnumerator Start()
        {
            yield return null;
            
            this.RefreshValues();
            this.RefreshProgress();

            this.IsInitialized = true;
        }

        private void OnEnable()
        {
            if (!this.IsInitialized) return;
            
            this.RefreshValues();
            this.RefreshProgress();
        }

        private void OnDisable()
        {
            if (ApplicationManager.IsExiting) return;
            if (this.m_LastTarget == null) return;
            if (this.m_Attribute == null) return;

            Traits lastTraits = this.m_LastTarget.Get<Traits>();
            if (lastTraits != null) lastTraits.EventChange -= this.OnChangeAttribute;

            this.m_LastTarget = null;
        }

        public static AttributeUI CreateFrom(Image image)
        {
            AttributeUI attributeUI = image.gameObject.AddComponent<AttributeUI>();
            attributeUI.m_ImageFill = image;

            return attributeUI;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            if (this.m_LastTarget == null) return;
            if (this.m_Attribute == null) return;

            Traits traits = this.m_LastTarget.Get<Traits>();
            if (traits == null) return;

            RuntimeAttributeData attribute = traits.RuntimeAttributes.Get(this.m_Attribute.ID);
            float attributeRatio = (float) attribute.Ratio;

            float animationTarget = attributeRatio;
            float animationSmooth = 0f;

            bool hamperTime = Time.unscaledTime - this.m_LastChangeTime < this.m_StallDuration;

            if (this.m_ProgressAnimation.Current < attributeRatio)
            {
                animationTarget = hamperTime && this.m_WhenIncrement
                    ? this.m_ProgressAnimation.Current
                    : attributeRatio;

                animationSmooth = this.m_WhenIncrement
                    ? this.m_TransitionDuration
                    : 0f;
            }

            if (this.m_ProgressAnimation.Current > attributeRatio)
            {
                animationTarget = hamperTime && this.m_WhenDecrement
                    ? this.m_ProgressAnimation.Current
                    : attributeRatio;

                animationSmooth = this.m_WhenDecrement
                    ? this.m_TransitionDuration
                    : 0f;
            }

            this.m_ProgressAnimation.UpdateWithDelta(
                animationTarget,
                animationSmooth,
                Time.unscaledDeltaTime
            );

            this.UpdateProgress();
        }

        private void UpdateProgress()
        {
            float progress = this.m_ProgressAnimation.Current;

            if (this.m_ImageFill != null) this.m_ImageFill.fillAmount = progress;

            if (this.m_ScaleX != null)
            {
                Vector3 scale = this.m_ScaleX.localScale;
                this.m_ScaleX.localScale = new Vector3(progress, scale.y, scale.z);
            }

            if (this.m_ScaleY != null)
            {
                Vector3 scale = this.m_ScaleY.localScale;
                this.m_ScaleY.localScale = new Vector3(scale.x, progress, scale.z);
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        /// <summary>
        /// Forces to repaint all values and progress values, skipping any transitions.
        /// </summary>
        public void Repaint()
        {
            this.RefreshValues();
            this.RefreshProgress();
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        private void OnChangeAttribute()
        {
            this.m_LastChangeTime = Time.unscaledTime;
            this.RefreshValues();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshProgress()
        {
            if (this.m_LastTarget == null) return;
            if (this.m_Attribute == null) return;

            Traits traits = this.m_LastTarget.Get<Traits>();
            if (traits == null) return;

            RuntimeAttributeData attribute = traits.RuntimeAttributes.Get(this.m_Attribute.ID);
            this.m_ProgressAnimation = new AnimFloat(
                (float) attribute.Ratio,
                (float) attribute.Ratio, 
                0f
            );

            this.UpdateProgress();
        }

        private void RefreshValues()
        {
            this.UpdateTargetEvents();

            if (this.m_Attribute == null) return;
            if (this.m_LastTarget == null) return;

            Traits traits = this.m_LastTarget.Get<Traits>();
            if (traits == null) return;

            if (this.m_Common.Icon != null) this.m_Common.Icon.overrideSprite = this.m_Attribute.GetIcon(this.m_Args);
            if (this.m_Common.Color != null) this.m_Common.Color.color = this.m_Attribute.Color;

            this.m_Common.Name.Text = this.m_Attribute.GetName(this.m_Args);
            this.m_Common.Acronym.Text = this.m_Attribute.GetAcronym(this.m_Args);
            this.m_Common.Description.Text = this.m_Attribute.GetDescription(this.m_Args);

            RuntimeAttributeData attribute = traits.RuntimeAttributes.Get(this.m_Attribute.ID);
            if (attribute == null) return;

            this.m_Value.Text = FromDouble(attribute.Value, "0");
            this.m_Percentage.Text = (attribute.Ratio * 100).ToString("0");
            this.m_MaxValue.Text = FromDouble(attribute.MaxValue, "0");
            this.m_MinValue.Text = FromDouble(attribute.MinValue, "0");

            if (this.m_UnitContainer != null && this.m_UnitPrefab != null)
            {
                int numUnits = this.m_UnitMode switch
                {
                    UnitMode.KeepEmptyUnits => Mathf.CeilToInt((float) attribute.MaxValue / this.m_UnitValue),
                    UnitMode.HideEmptyUnits => Mathf.CeilToInt((float) attribute.Value / this.m_UnitValue),
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                RectTransformUtils.RebuildChildren(this.m_UnitContainer, this.m_UnitPrefab, numUnits);

                for (int i = 0; i < numUnits; ++i)
                {
                    AttributeUnitUI unit = this.m_UnitContainer.GetChild(i).Get<AttributeUnitUI>();
                    
                    float maxValue = Mathf.Clamp01((float) attribute.MaxValue / this.m_UnitValue - i);
                    float currentValue = Mathf.Clamp01((float) attribute.Value / this.m_UnitValue - i);
                    if (unit != null) unit.Refresh(this.m_Attribute, maxValue, currentValue, this.m_Args);
                }
            }
        }

        private void UpdateTargetEvents()
        {
            if (this.m_Attribute == null) return;

            GameObject currentTarget = this.m_Target.Get(this.m_Args);
            if (this.m_LastTarget == currentTarget) return;
            
            if (this.m_LastTarget != null)
            {
                Traits lastTraits = this.m_LastTarget.Get<Traits>();
                if (lastTraits != null) lastTraits.EventChange -= this.OnChangeAttribute;
            }

            this.m_LastTarget = currentTarget;
            if (currentTarget == null) return;
            
            Traits currentTraits = currentTarget.Get<Traits>();
            if (currentTraits != null) currentTraits.EventChange += this.OnChangeAttribute;;
        }

        private static string FromDouble(double value, string format = "")
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
