using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Dialogue Playing")]
    [Category("Dialogue/Dialogue Playing")]
    
    [Image(typeof(IconDialogue), ColorTheme.Type.Blue, typeof(OverlayDot))]
    [Description("The Dialogue game object that is currently being played (if any)")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectDialoguePlaying : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args) => Dialogue.Current != null
            ? Dialogue.Current.gameObject 
            : null;
        
        public override GameObject Get(GameObject gameObject) => Dialogue.Current != null
            ? Dialogue.Current.gameObject 
            : null;

        public override T Get<T>(Args args)
        {
            if (typeof(T) == typeof(Dialogue)) return Dialogue.Current as T;
            return base.Get<T>(args);
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectDialoguePlaying instance = new GetGameObjectDialoguePlaying();
            return new PropertyGetGameObject(instance);
        }

        public override string String => "Dialogue Playing";
    }
}