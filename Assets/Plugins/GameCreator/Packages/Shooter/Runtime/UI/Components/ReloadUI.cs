using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Shooter
{
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoReloadUI.png")]
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [AddComponentMenu("Game Creator/UI/Shooter/Reload UI")]
    [Serializable]
    public class ReloadUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetWeapon m_Weapon = GetWeaponShooterInstance.Create();

        [SerializeField] private GameObject m_ActiveIfReloading;
        [SerializeField] private GameObject m_ActiveInQuickReloadRange;
        [SerializeField] private GameObject m_ActiveFailQuickReload;
        
        [SerializeField] private Image m_ReloadProgressFill;
        [SerializeField] private RectTransform m_ReloadProgressScaleX;
        [SerializeField] private RectTransform m_ReloadProgressScaleY;
        
        [SerializeField] private RectTransform m_QuickReloadRange;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        [NonSerialized] private Character m_CharacterCache;
        [NonSerialized] private ShooterWeapon m_WeaponCache;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            this.m_Args = new Args(this.gameObject);
        }

        private void Update()
        {
             Character character = this.m_Character.Get<Character>(this.m_Args);
             if (character == null) return;

             ShooterWeapon weapon = this.m_Weapon.Get(this.m_Args) as ShooterWeapon;
             if (weapon == null) return;

             ShooterStance stance = character.Combat.RequestStance<ShooterStance>();
             bool isReloading = stance.Reloading.IsReloading && stance.Reloading.WeaponReloading == weapon;
             float reloadRatio = stance.Reloading.Ratio;
             
             if (this.m_ActiveIfReloading != null)
             {
                 this.m_ActiveIfReloading.SetActive(isReloading);
             }

             if (this.m_ActiveInQuickReloadRange != null)
             {
                 bool inQuickReloadRange = reloadRatio >= stance.Reloading.QuickReloadRange.x &&
                                           reloadRatio <= stance.Reloading.QuickReloadRange.y;
                 
                 this.m_ActiveInQuickReloadRange.SetActive(isReloading && inQuickReloadRange);
             }

             if (this.m_ActiveFailQuickReload != null)
             {
                 this.m_ActiveFailQuickReload.SetActive(isReloading && stance.Reloading.TriedQuickReload);
             }

             if (this.m_ReloadProgressFill != null)
             {
                 this.m_ReloadProgressFill.fillAmount = reloadRatio;
             }

             if (this.m_ReloadProgressScaleX != null)
             {
                 this.m_ReloadProgressScaleX.localScale = new Vector3(
                     reloadRatio,
                     this.m_ReloadProgressScaleX.localScale.y,
                     this.m_ReloadProgressScaleX.localScale.z
                 );
             }
             
             if (this.m_ReloadProgressScaleY != null)
             {
                 this.m_ReloadProgressScaleY.localScale = new Vector3(
                     this.m_ReloadProgressScaleY.localScale.x,
                     reloadRatio,
                     this.m_ReloadProgressScaleY.localScale.z
                 );
             }

             if (this.m_QuickReloadRange != null)
             {
                 RectTransform quickReloadParent = this.m_QuickReloadRange.GetComponentInParent<RectTransform>();
                 if (quickReloadParent != null)
                 {
                     Vector2 quickReloadRange = stance.Reloading.QuickReloadRange;
                     
                     this.m_QuickReloadRange.anchorMin = new Vector2(quickReloadRange.x, 0f);
                     this.m_QuickReloadRange.anchorMax = new Vector2(quickReloadRange.y, 1f);
                     this.m_QuickReloadRange.pivot = new Vector2(0.5f, 0.5f);
                     
                     this.m_QuickReloadRange.anchoredPosition = Vector2.zero;
                     this.m_QuickReloadRange.sizeDelta = Vector2.zero;
                 }
             }
        }
    }
}
