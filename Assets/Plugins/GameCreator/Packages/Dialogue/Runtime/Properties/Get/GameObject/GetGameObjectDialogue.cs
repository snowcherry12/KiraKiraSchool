using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Dialogue")]
    [Category("Dialogue/Dialogue")]
    
    [Image(typeof(IconDialogue), ColorTheme.Type.Blue)]
    [Description("A Game Object scene reference or prefab with a Dialogue component")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectDialogue : PropertyTypeGetGameObject
    {
        [SerializeField] private Dialogue m_Dialogue;

        public override GameObject Get(Args args) => this.m_Dialogue != null
            ? this.m_Dialogue.gameObject 
            : null;
        
        public override GameObject Get(GameObject gameObject) => this.m_Dialogue != null
            ? this.m_Dialogue.gameObject 
            : null;

        public override T Get<T>(Args args)
        {
            if (typeof(T) == typeof(Dialogue)) return this.m_Dialogue as T;
            return base.Get<T>(args);
        }

        public GetGameObjectDialogue() : base()
        { }

        public GetGameObjectDialogue(Dialogue dialogue) : this()
        {
            this.m_Dialogue = dialogue;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectDialogue instance = new GetGameObjectDialogue();
            return new PropertyGetGameObject(instance);
        }
        
        public static PropertyGetGameObject Create(Dialogue dialogue)
        {
            GetGameObjectDialogue instance = new GetGameObjectDialogue
            {
                m_Dialogue = dialogue
            };
            
            return new PropertyGetGameObject(instance);
        }

        public override string String => this.m_Dialogue != null
            ? this.m_Dialogue.gameObject.name
            : "(none)";

        public override GameObject EditorValue => this.m_Dialogue != null
            ? this.m_Dialogue.gameObject
            : null;
    }
}