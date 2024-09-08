using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Task of Interest")]
    [Image(typeof(IconPointInterest), ColorTheme.Type.Yellow)]
    
    [Category("Quests/Task of Interest")]
    [Description(
        "Determines the position of a specific Task's Point of Interest in order to show it " +
        "when it's active on a minimap or compass"
    )]
    
    [Keywords("Quest", "Task")]

    [Serializable]
    public class SpotQuestsTaskPoi : TSpotPoi
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();
        [SerializeField] private PickTask m_Task = new PickTask();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Point of Interest for {this.m_Task}";

        [field: NonSerialized] private Journal Journal { get; set; }

        public override string GetName => this.m_Task.IsValid
            ? this.m_Task.Quest.GetTask(this.m_Task.TaskId).GetName(this.Args)
            : string.Empty;
        
        public override Sprite GetSprite => this.m_Task.IsValid
            ? this.m_Task.Quest.GetTask(this.m_Task.TaskId).GetSprite(this.Args)
            : null;
        
        public override Color GetColor => this.m_Task.IsValid
            ? this.m_Task.Quest.GetTask(this.m_Task.TaskId).GetColor(this.Args)
            : Color.white;

        // CALLBACKS: -----------------------------------------------------------------------------

        protected override void OnHotspotActivate()
        {
            this.Journal = this.m_Journal.Get<Journal>(this.Args);
            
            if (this.Journal == null) return;
            if (!this.m_Task.IsValid) return;

            this.Journal.EventTaskActivate -= this.OnTaskChange;
            this.Journal.EventTaskActivate += this.OnTaskChange;
            
            this.Journal.EventTaskComplete -= this.OnTaskChange;
            this.Journal.EventTaskComplete += this.OnTaskChange;
            
            this.Journal.EventTaskAbandon -= this.OnTaskChange;
            this.Journal.EventTaskAbandon += this.OnTaskChange;
            
            this.Journal.EventTaskFail -= this.OnTaskChange;
            this.Journal.EventTaskFail += this.OnTaskChange;

            this.Journal.EventTaskDeactivate -= this.OnTaskChange;
            this.Journal.EventTaskDeactivate += this.OnTaskChange;
            
            this.Refresh();
        }
        
        protected override void OnHotspotDeactivate()
        {
            if (this.Journal == null) return;
            
            this.Journal.EventTaskActivate -= this.OnTaskChange;
            this.Journal.EventTaskComplete -= this.OnTaskChange;
            this.Journal.EventTaskAbandon -= this.OnTaskChange;
            this.Journal.EventTaskFail -= this.OnTaskChange;
            this.Journal.EventTaskDeactivate -= this.OnTaskChange;
            
            this.Refresh();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnTaskChange(Quest quest, int taskId)
        {
            if (!this.m_Task.IsValid) return;
            if (this.m_Task.Quest != quest || this.m_Task.TaskId != taskId) return;
            
            this.Refresh();
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override void Refresh()
        {
            if (this.Journal == null) return;
            
            if (this.HasBeenDisabled)
            {
                PointsOfInterest.Remove(this.Id);
                return;
            }
            
            bool taskActive = this.Journal.IsTaskActive(this.m_Task.Quest, this.m_Task.TaskId);
            
            switch (this.Hotspot.IsActive && this.Hotspot.isActiveAndEnabled && taskActive)
            {
                case true: PointsOfInterest.Insert(this.Id, this); break;
                case false: PointsOfInterest.Remove(this.Id); break;
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override bool Filter(bool hiddenQuests, bool hideUntracked)
        {
            if (!this.m_Task.IsValid) return false;

            bool isTracking = this.Journal.IsQuestTracking(this.m_Task.Quest);
            if (hideUntracked && !isTracking) return false;
            
            return this.m_Task.Quest.Type == QuestType.Normal || hiddenQuests;
        }
    }
}
