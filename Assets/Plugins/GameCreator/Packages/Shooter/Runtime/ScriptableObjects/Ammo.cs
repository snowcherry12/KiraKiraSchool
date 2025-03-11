using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [CreateAssetMenu(
        fileName = "Ammo", 
        menuName = "Game Creator/Shooter/Ammo",
        order    = 50
    )]
    
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoAmmo.png")]
    
    [Serializable]
    public class Ammo : ScriptableObject
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private UniqueID m_Id = new UniqueID();
        
        [SerializeField] private PropertyGetString m_Title = GetStringString.Create;
        [SerializeField] private PropertyGetString m_Description = GetStringTextArea.Create();

        [SerializeField] private PropertyGetSprite m_Icon = GetSpriteNone.Create;
        [SerializeField] private PropertyGetColor m_Color = GetColorColorsWhite.Create;

        [SerializeField] private bool m_Infinite = true;
        [SerializeField] private PropertySetNumber m_Value = SetNumberMunition.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public IdString Id => this.m_Id.Get;

        public bool IsInfinite => this.m_Infinite;
        
        // GETTERS: -------------------------------------------------------------------------------
        
        public string GetName(Args args) => this.m_Title.Get(args);
        public string GetDescription(Args args) => this.m_Description.Get(args);
        
        public Sprite GetSprite(Args args) => this.m_Icon.Get(args);
        public Color GetColor(Args args) => this.m_Color.Get(args);

        public int Get(Args args) => this.m_Infinite
            ? int.MaxValue
            : (int) this.m_Value.Get(args);

        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal void Remove(int amount, Args args)
        {
            if (this.m_Infinite) return;
            
            int current = (int) this.m_Value.Get(args);
            this.m_Value.Set(Math.Max(0, current - amount), args);
        }
    }
}