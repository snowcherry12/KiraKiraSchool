using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("FMOD Audio")]
    [Category("FMOD Audio")]
    
    [Image(typeof(IconQuaver), ColorTheme.Type.Yellow)]
    [Description("An FMOD Audio asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetFMODAudio : PropertyTypeGetFMODAudio
    {
        [SerializeField] protected FMODAudio m_Value;

        public override FMODAudio Get(Args args) => this.m_Value;
        public override FMODAudio Get(GameObject gameObject) => this.m_Value;

        public GetFMODAudio() : base()
        { }

        public GetFMODAudio(FMODAudio value) : this()
        {
            this.m_Value = value;
        }

        public static PropertyGetFMODAudio Create => new PropertyGetFMODAudio(
            new GetFMODAudio()
        );

        public override string String => !String.IsNullOrEmpty(this.m_Value.Audio.Path)
            ? this.m_Value.Audio.Path.Split("/")[this.m_Value.Audio.Path.Split("/").Length - 1]
            : "(none)";

        public override FMODAudio EditorValue => this.m_Value;
    }
}