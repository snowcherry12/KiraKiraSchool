using System.Globalization;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Stats.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Stats/Status Effect UI")]
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoStatusEffect.png")]
    public class StatusEffectUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private UICommon m_Common = new UICommon();

        [SerializeField] private TextReference m_Count;
        [SerializeField] private TextReference m_RemainingTime;

        [SerializeField] private Image m_ImageFill;
        [SerializeField] private RectTransform m_ScaleX;
        [SerializeField] private RectTransform m_ScaleY;

        // MEMBERS: -------------------------------------------------------------------------------

        private Traits m_Traits;
        private IdString m_StatusEffectID;

        private Args m_Args;

        // INITIALIZERS: --------------------------------------------------------------------------

        public static StatusEffectUI CreateFrom(Image image)
        {
            StatusEffectUI statusEffectUI = image.gameObject.AddComponent<StatusEffectUI>();
            statusEffectUI.m_ImageFill = image;

            return statusEffectUI;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            if (this.m_Traits == null) return;

            RuntimeStatusEffectValue statusEffect = this.m_Traits
                .RuntimeStatusEffects
                .GetActiveAt(this.m_StatusEffectID, 0);

            float progress = statusEffect.HasDuration ? 1f - statusEffect.Progress : 1f;
            if (this.m_ImageFill != null) this.m_ImageFill.fillAmount = progress;

            if (this.m_ScaleX != null)
            {
                Vector3 scale = this.m_ScaleX.localScale;
                this.m_ScaleX.localScale = new Vector3(progress, scale.y, scale.z);
            }

            if (this.m_ScaleY != null)
            {
                Vector3 scale = this.m_ScaleY.localScale;
                this.m_ScaleY.localScale = new Vector3(progress, scale.y, scale.z);
            }

            float remainingTime = statusEffect.HasDuration ? statusEffect.TimeRemaining : 0f;
            this.m_RemainingTime.Text = FromFloat(remainingTime, remainingTime.ToString("0:F2"));
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        /// <summary>
        /// Sets the targeted traits component and status effect id.
        /// </summary>
        /// <param name="traits"></param>
        /// <param name="statusEffectID"></param>
        public void Target(Traits traits, IdString statusEffectID)
        {
            this.m_Traits = traits;
            this.m_StatusEffectID = statusEffectID;

            this.m_Args = new Args(this.gameObject, traits.gameObject);
            this.Repaint();
        }

        /// <summary>
        /// Forces to repaint all values.
        /// </summary>
        public void Repaint()
        {
            if (this.m_Traits == null) return;

            StatusEffect statusEffect = this.m_Traits
                .RuntimeStatusEffects
                .GetActiveStatusEffect(this.m_StatusEffectID);

            if (statusEffect == null) return;

            if (this.m_Common.Icon != null) this.m_Common.Icon.overrideSprite = statusEffect.GetIcon(this.m_Args);
            if (this.m_Common.Color != null) this.m_Common.Color.color = statusEffect.Color;

            this.m_Common.Name.Text = statusEffect.GetName(this.m_Args);
            this.m_Common.Acronym.Text = statusEffect.GetAcronym(this.m_Args);
            this.m_Common.Description.Text = statusEffect.GetDescription(this.m_Args);

            int count = this.m_Traits
                .RuntimeStatusEffects
                .GetActiveStackCount(this.m_StatusEffectID);

            RuntimeStatusEffectValue first = this.m_Traits
                .RuntimeStatusEffects
                .GetActiveAt(this.m_StatusEffectID, 0);

            this.m_Count.Text = count.ToString();
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        private static string FromFloat(float value, string format)
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
