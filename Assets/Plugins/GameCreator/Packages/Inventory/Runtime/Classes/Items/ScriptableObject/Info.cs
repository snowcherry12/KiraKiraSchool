using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Info
    {
        [SerializeField] private PropertyGetString m_Name = GetStringString.Create;
        [SerializeField] private PropertyGetString m_Description = GetStringTextArea.Create();
        
        [SerializeField] private PropertyGetSprite m_Sprite = GetSpriteInstance.Create();
        [SerializeField] private PropertyGetColor m_Color = GetColorColorsWhite.Create;

        [SerializeField] private RunInstructionsList m_OnCreate = new RunInstructionsList();
        [SerializeField] private bool m_ExecuteFromParent;
        
        // METHODS: -------------------------------------------------------------------------------

        public string Name(Args args) => this.m_Name.Get(args);
        public string Description(Args args) => this.m_Description.Get(args);
        
        public Sprite Sprite(Args args) => this.m_Sprite.Get(args);
        public Color Color(Args args) => this.m_Color.Get(args);
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        internal void RunOnCreate(Item item, Args args)
        {
            if (this.m_ExecuteFromParent && item.Parent != null)
            {
                item.Parent.Info.RunOnCreate(item.Parent, args);
            }
            
            _ = this.m_OnCreate.Run(args);
        }
    }
}