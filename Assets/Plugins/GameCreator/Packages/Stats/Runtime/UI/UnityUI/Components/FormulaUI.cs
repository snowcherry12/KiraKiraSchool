using System.Collections;
using System.Globalization;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Stats.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Stats/Formula UI")]
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoFormula.png")]
    public class FormulaUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Source = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField] private Formula m_Formula;
        
        [SerializeField] private TextReference m_Value = new TextReference();
        [SerializeField] private Image m_RatioFill;

        // MEMBERS: -------------------------------------------------------------------------------

        private GameObject m_LastSource;
        private GameObject m_LastTarget;
        
        private Args m_Args;

        // PROPERTIES: ----------------------------------------------------------------------------

        public GameObject Source
        {
            set
            {
                this.m_Source = GetGameObjectInstance.Create(value);
                this.RefreshValues();
            }
        }
        
        public GameObject Target
        {
            set
            {
                this.m_Target = GetGameObjectInstance.Create(value);
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
            if (this.m_Formula == null) return;

            Traits lastSourceTraits = this.m_LastSource.Get<Traits>();
            if (lastSourceTraits != null) lastSourceTraits.RuntimeStats.EventChange -= this.OnChangeStat;
            
            Traits lastTargetTraits = this.m_LastTarget.Get<Traits>();
            if (lastTargetTraits != null) lastTargetTraits.RuntimeStats.EventChange -= this.OnChangeStat;
            
            this.m_LastSource = null;
            this.m_LastTarget = null;
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static FormulaUI CreateFrom(Text text)
        {
            FormulaUI formulaUI = text.gameObject.AddComponent<FormulaUI>();
            formulaUI.m_Value = new TextReference(text);

            return formulaUI;
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
            if (this.m_Formula == null) return;

            double value = this.m_Formula.Calculate(this.m_LastSource, this.m_LastTarget);

            this.m_Value.Text = FromDouble(value, "0");
            if (this.m_RatioFill != null) this.m_RatioFill.fillAmount = (float) value;
        }

        private void UpdateTargetEvents()
        {
            if (this.m_Formula == null) return;

            GameObject currentSource = this.m_Source.Get(this.m_Args);
            GameObject currentTarget = this.m_Target.Get(this.m_Args);
            if (this.m_LastSource == currentSource && this.m_LastTarget == currentTarget) return;
            
            if (this.m_LastTarget != null)
            {
                Traits lastSourceTraits = this.m_LastSource.Get<Traits>();
                if (lastSourceTraits != null) lastSourceTraits.RuntimeStats.EventChange -= this.OnChangeStat;
            }

            if (this.m_LastTarget != null)
            {
                Traits lastTargetTraits = this.m_LastTarget.Get<Traits>();
                if (lastTargetTraits != null) lastTargetTraits.RuntimeStats.EventChange -= this.OnChangeStat;
            }

            this.m_LastSource = currentSource;
            this.m_LastTarget = currentTarget;
            
            if (currentSource != null)
            {
                Traits currentSourceTraits = currentSource.Get<Traits>();
                if (currentSourceTraits != null)
                {
                    currentSourceTraits.RuntimeStats.EventChange += this.OnChangeStat;
                }   
            }

            if (currentTarget != null)
            {
                Traits currentTargetTraits = currentTarget.Get<Traits>();
                if (currentTargetTraits != null)
                {
                    currentTargetTraits.RuntimeStats.EventChange += this.OnChangeStat;
                }   
            }
        }

        private static string FromDouble(double value, string format = "")
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
