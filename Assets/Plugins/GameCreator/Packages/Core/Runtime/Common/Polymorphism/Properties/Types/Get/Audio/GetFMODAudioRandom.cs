using System;
using UnityEngine;
using FMODUnity;

namespace GameCreator.Runtime.Common
{
    [Title("Random FMOD Audio")]
    [Category("Random/Random FMOD Audio")]
    
    [Image(typeof(IconDice), ColorTheme.Type.Yellow)]
    [Description("A random FMOD Audio asset from a list")]

    [Serializable] [HideLabelsInEditor]
    public class GetFMODAudioRandom : PropertyTypeGetFMODAudio
    {
        [SerializeField] protected FMODAudio[] m_Values = Array.Empty<FMODAudio>();

        public override FMODAudio Get(Args args)
        {
            if ((this.m_Values?.Length ?? 0) == 0) return null;

            int index = UnityEngine.Random.Range(0, this.m_Values.Length);
            return this.m_Values[index];
        }

        public static PropertyGetFMODAudio Create => new PropertyGetFMODAudio(
            new GetFMODAudioRandom()
        );

        public override string String => "Random FMOD Audio";
    }
}