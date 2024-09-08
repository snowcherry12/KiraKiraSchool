using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Title("Current Dialogue UI")]
    [Category("Dialogue/UI/Current Dialogue UI")]
    
    [Image(typeof(IconDialogue), ColorTheme.Type.TextLight, typeof(OverlayDot))]
    [Description("The Dialogue UI skin currently being played (if any at all)")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectDialogueUICurrent : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args) => DialogueUI.Current != null
            ? DialogueUI.Current.gameObject 
            : null;
        
        public override GameObject Get(GameObject gameObject) => DialogueUI.Current != null
            ? DialogueUI.Current.gameObject 
            : null;

        public override T Get<T>(Args args)
        {
            if (typeof(T) == typeof(DialogueUI)) return DialogueUI.Current as T;
            return base.Get<T>(args);
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectDialogueUICurrent instance = new GetGameObjectDialogueUICurrent();
            return new PropertyGetGameObject(instance);
        }

        public override string String => "Current Skin";
    }
}