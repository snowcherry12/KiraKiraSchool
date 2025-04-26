using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Date Time")]
    [Category("Time/Date Time")]
    
    [Image(typeof(IconClock), ColorTheme.Type.Yellow)]
    [Description("Returns the current time in the specified format")]
    
    [Serializable]
    public class GetStringDateTime : PropertyTypeGetString
    {
        [SerializeField] private string m_Format = "f";
        
        public override string Get(Args args) => this.GetTime();

        public override string Get(GameObject gameObject) => this.GetTime();

        private string GetTime()
        {
            DateTime currentTime = DateTime.Now;
            return currentTime.ToString(this.m_Format);
        }

        public static PropertyGetString Create => new PropertyGetString(
            new GetStringDateTime()
        );

        public override string String => "Time";
    }
}