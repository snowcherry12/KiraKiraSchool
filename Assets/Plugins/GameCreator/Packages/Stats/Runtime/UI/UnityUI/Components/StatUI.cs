using System.Collections;
using System.Globalization;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Stats.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Stats/Stat UI")]
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoStat.png")]
    public class StatUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField] private Stat m_Stat;

        [SerializeField] private UICommon m_Common = new UICommon();

        [SerializeField] private TextReference m_Value = new TextReference();
        [SerializeField] private TextReference m_Base = new TextReference();
        [SerializeField] private TextReference m_Modifiers = new TextReference();

        [SerializeField] private Image m_RatioFill;

        [SerializeField] private GameObject m_ActiveIfModifiers;
        [SerializeField] private GameObject m_ActiveIfNotModifiers;

        // MEMBERS: -------------------------------------------------------------------------------

        private GameObject m_LastTarget;
        private Args m_Args;

        // PROPERTIES: ----------------------------------------------------------------------------

        public GameObject Target
        {
            set
            {
                this.m_Target = GetGameObjectInstance.Create(value);
                this.RefreshValues();
            }
        }
        
        public Stat Stat
        {
            set
            {
                this.m_Stat = value;
                this.RefreshValues();
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
            this.IsInitialized = true;
        }

        private void OnEnable()
        {
            if (!this.IsInitialized) return;
            this.RefreshValues();
        }

        private void OnDisable()
        {
            if (ApplicationManager.IsExiting) return;
            if (this.m_LastTarget == null) return;
            if (this.m_Stat == null) return;

            Traits lastTraits = this.m_LastTarget.Get<Traits>();
            if (lastTraits != null) lastTraits.RuntimeStats.EventChange -= this.OnChangeStat;
            
            this.m_LastTarget = null;
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static StatUI CreateFrom(Text text)
        {
            StatUI statUI = text.gameObject.AddComponent<StatUI>();
            statUI.m_Value = new TextReference(text);

            return statUI;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        /// <summary>
        /// Forces to repaint all values and progress values, skipping any transitions.
        /// </summary>
        public void Repaint()
        {
            this.RefreshValues();
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        private void OnChangeStat(IdString statID)
        {
            this.RefreshValues();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshValues()
        {
            this.UpdateTargetEvents();
            if (this.m_Stat == null) return;

            if (this.m_LastTarget == null) return;

            Traits traits = this.m_LastTarget.Get<Traits>();
            if (traits == null) return;

            if (this.m_Common.Icon != null) this.m_Common.Icon.overrideSprite = this.m_Stat.GetIcon(this.m_Args);
            if (this.m_Common.Color != null) this.m_Common.Color.color = this.m_Stat.Color;

            this.m_Common.Name.Text = this.m_Stat.GetName(this.m_Args);
            this.m_Common.Acronym.Text = this.m_Stat.GetAcronym(this.m_Args);
            this.m_Common.Description.Text = this.m_Stat.GetDescription(this.m_Args);

            RuntimeStatData stat = traits.RuntimeStats.Get(this.m_Stat.ID);
            if (stat == null) return;

            this.m_Value.Text = FromDouble(stat.Value, "0");
            this.m_Base.Text = FromDouble(stat.Base, "0");
            this.m_Modifiers.Text = FromDouble(stat.ModifiersValue, "+#;-#;0");
            
            if (this.m_RatioFill != null) this.m_RatioFill.fillAmount = (float) stat.Value;

            bool hasModifiers = Mathf.Approximately((float) stat.ModifiersValue, 0) == false;
            
            if (this.m_ActiveIfModifiers != null) this.m_ActiveIfModifiers.SetActive(hasModifiers);
            if (this.m_ActiveIfNotModifiers != null) this.m_ActiveIfNotModifiers.SetActive(!hasModifiers);
        }

        private void UpdateTargetEvents()
        {
            if (this.m_Stat == null) return;

            GameObject currentTarget = this.m_Target.Get(this.m_Args);
            if (this.m_LastTarget == currentTarget) return;
            
            if (this.m_LastTarget != null)
            {
                Traits lastTraits = this.m_LastTarget.Get<Traits>();
                if (lastTraits != null) lastTraits.RuntimeStats.EventChange -= this.OnChangeStat;
            }

            this.m_LastTarget = currentTarget;
            if (currentTarget == null) return;
            
            Traits currentTraits = currentTarget.Get<Traits>();
            if (currentTraits != null) currentTraits.RuntimeStats.EventChange += this.OnChangeStat;
        }

        private static string FromDouble(double value, string format = "")
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
