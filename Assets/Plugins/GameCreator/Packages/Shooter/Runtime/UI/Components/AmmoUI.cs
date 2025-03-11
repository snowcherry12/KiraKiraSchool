using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Shooter
{
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoAmmoUI.png")]
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [AddComponentMenu("Game Creator/UI/Shooter/Ammo UI")]
    [Serializable]
    public class AmmoUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetWeapon m_Weapon = GetWeaponShooterInstance.Create();

        [SerializeField] private TextReference m_InMagazine = new TextReference();
        [SerializeField] private TextReference m_InMunition = new TextReference();

        [SerializeField] private Image m_MagazineFill;
        
        [SerializeField] private GameObject m_PrefabInMagazine;
        [SerializeField] private RectTransform m_ContentInMagazine;

        [SerializeField] private GameObject m_ActiveIfEmpty;
        [SerializeField] private GameObject m_ActiveIfFull;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        [NonSerialized] private Character m_CharacterCache;
        [NonSerialized] private ShooterWeapon m_WeaponCache;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
             Character character = this.m_Character.Get<Character>(this.gameObject);
             if (character == null) return;

             ShooterWeapon weapon = this.m_Weapon.Get(this.gameObject) as ShooterWeapon;
             if (weapon == null) return;

             TMunitionValue munition = character.Combat.RequestMunition(weapon);
             if (munition == null) return;
             
             this.m_Args = new Args(character.gameObject);
             
             this.m_CharacterCache = character;
             this.m_WeaponCache = weapon;
             
             this.Refresh();
             munition.EventChange += this.Refresh;
        }

        private void OnDisable()
        {
            if (this.m_CharacterCache == null) return;
            if (this.m_WeaponCache == null) return;
            
            TMunitionValue munition = this.m_CharacterCache.Combat.RequestMunition(this.m_WeaponCache);
            if (munition == null) return;
            
            munition.EventChange -= this.Refresh;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void Refresh()
        {
            if (this.m_CharacterCache == null) return;
            if (this.m_WeaponCache == null) return;
            
            TMunitionValue munition = this.m_CharacterCache.Combat.RequestMunition(this.m_WeaponCache);
            if (munition is not ShooterMunition shooterMunition) return;
            
            GameObject prop = this.m_CharacterCache.Combat
                .RequestStance<ShooterStance>()
                .Get(this.m_WeaponCache)?.Prop;
            
            this.m_Args.ChangeTarget(prop);

            int inMagazine = shooterMunition.InMagazine;
            int inTotal = shooterMunition.Total;
            int inPouch = Mathf.Max(0, inTotal - inMagazine);
            
            this.m_InMagazine.Text = shooterMunition.InMagazine.ToString();
            this.m_InMunition.Text = inPouch.ToString();

            int magazineSize = this.m_WeaponCache.Magazine.GetHasMagazine(this.m_Args)
                ? this.m_WeaponCache.Magazine.GetMagazineSize(this.m_Args)
                : 0;
            
            float ratio = magazineSize >= 1 
                ? inMagazine / (float) magazineSize 
                : 1f;

            if (this.m_MagazineFill != null)
            {
                this.m_MagazineFill.fillAmount = ratio;
            }

            if (this.m_ContentInMagazine != null)
            {
                RectTransformUtils.RebuildChildren(
                    this.m_ContentInMagazine,
                    this.m_PrefabInMagazine,
                    inMagazine
                );
            }

            if (this.m_ActiveIfEmpty != null)
            {
                this.m_ActiveIfEmpty.SetActive(ratio <= float.Epsilon);
            }
            
            if (this.m_ActiveIfFull != null)
            {
                this.m_ActiveIfFull.SetActive(ratio >= 1f);
            }
        }
    }
}
