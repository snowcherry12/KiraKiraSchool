using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Magazine
    {
        [SerializeField] private Ammo m_Ammo;

        [SerializeField] private PropertyGetBool m_HasMagazine = GetBoolTrue.Create;
        [SerializeField] private PropertyGetInteger m_MagazineSize = GetDecimalInteger.Create(10);
        [SerializeField] private PropertyGetBool m_AutoReload = GetBoolTrue.Create;
        
        // GETTER METHODS: ------------------------------------------------------------------------

        public int GetAmmo(Args args)
        {
            return this.m_Ammo != null ? this.m_Ammo.Get(args) : 0;
        }
        
        public bool GetHasMagazine(Args args)
        {
            return this.m_HasMagazine.Get(args);
        }
        
        public int GetMagazineSize(Args args)
        {
            return (int) this.m_MagazineSize.Get(args);
        }
        
        public bool AutoReload(Args args) => this.m_AutoReload.Get(args);
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal bool EnoughAmmo(ShotData shotData, Args args)
        {
            if (this.m_Ammo == null) return false;
            
            Character character = args.Self.Get<Character>();
            if (character == null) return false;
            
            int currentAmmo = this.m_Ammo.Get(args);
            if (this.m_Ammo.IsInfinite == false && shotData.Cartridges > currentAmmo) return false;
            
            if (this.m_HasMagazine.Get(args))
            {
                int magazineSize = (int) this.m_MagazineSize.Get(args);
                
                ShooterMunition munition = character.Combat.RequestMunition(shotData.Weapon) as ShooterMunition;
                int inMagazine = MathUtils.Min(munition?.InMagazine ?? 0, magazineSize, currentAmmo);
                
                if (shotData.Cartridges > inMagazine) return false;
            }
            
            return true;
        }
        
        internal void DiscardMagazine(int amount, Args args)
        {
            if (this.m_HasMagazine.Get(args) == false) return;
            
            if (this.m_Ammo == null) return;
            this.m_Ammo.Remove(amount, args);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public void StageGizmos(StagingGizmos stagingGizmos)
        { }
    }
}