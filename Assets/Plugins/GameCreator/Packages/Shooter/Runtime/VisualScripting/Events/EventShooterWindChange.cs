using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Shooter
{
    [Title("On Wind Change")]
    [Category("Shooter/Wind/On Wind Change")]
    [Description("Executed when the Wind force or direction changes")]

    [Image(typeof(IconWind), ColorTheme.Type.Green)]
    [Keywords("Wind", "Drift", "Force", "Air", "Storm")]

    [Serializable]
    public class EventShooterWindChange : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            WindManager.Instance.EventChange -= this.OnChange;
            WindManager.Instance.EventChange += this.OnChange;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            WindManager.Instance.EventChange -= this.OnChange;
        }

        private void OnChange()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}