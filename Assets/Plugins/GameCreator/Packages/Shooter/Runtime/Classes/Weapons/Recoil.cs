using System;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Recoil
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetBool m_UseRecoil = GetBoolCharacterIsPlayer.Create;
        [SerializeField] private PropertyGetGameObject m_Camera = GetGameObjectCameraMain.Create; 
        
        [SerializeField]
        private PropertyGetDecimal m_RecoilX = GetDecimalRandomRange.Create(
            GetDecimalConstantMinusOne.Create,
            GetDecimalConstantOne.Create
        );
        
        [SerializeField] 
        private PropertyGetDecimal m_RecoilY = GetDecimalRandomRange.Create(
            GetDecimalConstantOne.Create,
            GetDecimalConstantTwo.Create
        );

        [SerializeField]
        private PropertyGetDecimal m_RecoilDuration = GetDecimalDecimal.Create(0.15f);
        
        // INTERNAL METHODS: ------------------------------------------------------------------------
        
        internal void OnShoot(Args args)
        {
            if (this.m_UseRecoil.Get(args) == false) return;

            TCamera camera = this.m_Camera.Get<TCamera>(args);
            if (camera == null) return;

            ShotCamera shot = camera.Transition.CurrentShotCamera;
            if (shot == null) return;

            float recoilDuration = (float) this.m_RecoilDuration.Get(args);
            
            Vector2 recoil = new Vector2(
                (float) -this.m_RecoilX.Get(args) / (recoilDuration > 0f ? recoilDuration : 1f),
                (float) -this.m_RecoilY.Get(args) / (recoilDuration > 0f ? recoilDuration : 1f)
            );
            
            shot.ShotType.Recoil.Run(recoilDuration, recoil);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public void StageGizmos(StagingGizmos stagingGizmos)
        { }
    }
}