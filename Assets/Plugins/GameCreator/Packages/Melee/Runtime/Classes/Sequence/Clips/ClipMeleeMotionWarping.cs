using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class ClipMeleeMotionWarping : Clip
    {
        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();
        
        [SerializeField] private Easing.Type m_Easing = Common.Easing.Type.QuadInOut;
        
        [SerializeField] private PropertyGetLocation m_Self = GetLocationMeleeSelfToTarget.Create;
        [SerializeField] private PropertyGetLocation m_Target = GetLocationNone.Create;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Easing.Type Easing => this.m_Easing;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ClipMeleeMotionWarping() : this(0f, DEFAULT_TIME)
        { }

        public ClipMeleeMotionWarping(float time) : base(time, 0f)
        { }
        
        public ClipMeleeMotionWarping(float time, float duration) : base(time, duration)
        { }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool CheckConditions(Args args) => this.m_Conditions.Check(args);
        
        public Location GetLocationSelf(Args args) => this.m_Self?.Get(args) ?? Location.None;
        public Location GetLocationTarget(Args args) => this.m_Target?.Get(args) ?? Location.None;
    }
}