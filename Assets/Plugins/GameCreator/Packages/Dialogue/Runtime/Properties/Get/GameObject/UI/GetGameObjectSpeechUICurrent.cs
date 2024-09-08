using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Title("Current Speech UI")]
    [Category("Dialogue/UI/Current Speech UI")]
    
    [Image(typeof(IconNodeText), ColorTheme.Type.TextLight, typeof(OverlayDot))]
    [Description("The Speech UI skin currently being played (if any at all)")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectSpeechUICurrent : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args) => SpeechUI.Current != null
            ? SpeechUI.Current.gameObject 
            : null;
        
        public override GameObject Get(GameObject gameObject) => SpeechUI.Current != null
            ? SpeechUI.Current.gameObject 
            : null;

        public override T Get<T>(Args args)
        {
            if (typeof(T) == typeof(SpeechUI)) return SpeechUI.Current as T;
            return base.Get<T>(args);
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectSpeechUICurrent instance = new GetGameObjectSpeechUICurrent();
            return new PropertyGetGameObject(instance);
        }

        public override string String => "Current Speech";
    }
}