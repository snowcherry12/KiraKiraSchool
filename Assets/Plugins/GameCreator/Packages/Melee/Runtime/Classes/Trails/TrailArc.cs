using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    internal class TrailArc
    {
        private struct Data
        {
            [field: NonSerialized] public int Index { get; }
            [field: NonSerialized] public float Ratio { get; }

            public Data(int index, float ratio)
            {
                this.Index = index;
                this.Ratio = ratio;
            }
        }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly List<Segment> m_Knots;
        
        [NonSerialized] private readonly List<float> m_Lengths;
        [NonSerialized] private readonly List<Data> m_Data;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] private Trail Trail { get; }
        [field: NonSerialized] public List<Segment> Output { get; }

        // INITIALIZERS: --------------------------------------------------------------------------
        
        public TrailArc(Trail trail)
        {
            this.m_Knots = new List<Segment>(Trail.DEFAULT_QUADS);
            this.m_Lengths = new List<float>(Trail.DEFAULT_QUADS);
            this.m_Data = new List<Data>(Trail.DEFAULT_QUADS);

            this.Trail = trail;
            this.Output = new List<Segment>(Trail.DEFAULT_QUADS);
        }
        
        public void OnEnable()
        {
            
        }
        
        public void OnDisable()
        {
            
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Start()
        {
            int numQuads = this.Trail.Quads;
            
            if (this.m_Knots.Capacity < numQuads) this.m_Knots.Capacity = numQuads;
            if (this.m_Lengths.Capacity < numQuads) this.m_Lengths.Capacity = numQuads;
            if (this.m_Data.Capacity < numQuads) this.m_Data.Capacity = numQuads;
            if (this.Output.Capacity < numQuads) this.Output.Capacity = numQuads;
            
            this.m_Knots.Clear();
            this.m_Lengths.Clear();
            this.m_Data.Clear();
        }
        
        public void Stop()
        { }

        public void Update()
        {
            this.UpdateKnots();
            this.UpdateIndexes();
            this.UpdateOutput();
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        private void UpdateKnots()
        {
            Segment knot = new Segment(
                this.Trail.EdgeA,
                this.Trail.EdgeB
            );
            
            this.m_Knots.Add(knot);
        }
        
        private void UpdateIndexes()
        {
            this.m_Lengths.Clear();
            this.m_Data.Clear();
            
            float totalLength = 0f;
            
            for (int i = 1; i < this.m_Knots.Count; ++i)
            {
                float length = this.m_Knots[i].LargeDistance(this.m_Knots[i - 1]);
                
                this.m_Lengths.Add(length);
                totalLength += length;
            }
            
            float section = totalLength / this.Trail.Quads;
            
            int knotIndex = 0;
            float knotSum = 0f;

            float ratioLength = Math.Min(this.Trail.Length, totalLength) / totalLength;
            float sectionsSum = totalLength - totalLength * ratioLength * this.Trail.Visibility;

            while (sectionsSum < totalLength)
            {
                Data data = this.GetData(knotIndex, knotSum, sectionsSum);
                this.m_Data.Add(data);
                
                for (int i = knotIndex; i < data.Index - 1; ++i)
                {
                    knotSum += this.m_Lengths[knotIndex];
                    knotIndex += 1;
                }
                
                sectionsSum += section;
            }
        }

        private void UpdateOutput()
        {
            this.Output.Clear();
            if (this.m_Knots.Count <= 1) return;

            foreach (Data data in this.m_Data)
            {
                switch (data.Index)
                {
                    case 0:
                        this.Output.Add(this.m_Knots[data.Index]);
                        break;
                    
                    case 1:
                        Segment segment = this.m_Knots[0].Lerp(this.m_Knots[1], data.Ratio);
                        this.Output.Add(segment);
                        break;
                    
                    default:
                        Segment p0 = this.GetP0(data.Index);
                        Segment p1 = this.GetP1(data.Index);
                        Segment p2 = this.GetP2(data.Index);
                        Segment p3 = this.GetP3(data.Index);
                        
                        Segment curveSegment = CatmullRom.Get(data.Ratio, p0, p1, p2, p3);
                        this.Output.Add(curveSegment);
                        break;
                }
            }
            
            this.Output.Add(this.m_Knots[^1]);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private Data GetData(int knotIndex, float knotSum, float sectionsSum)
        {
            float knotLength = 0f;
            while (knotSum < sectionsSum)
            {
                knotLength = this.m_Lengths[knotIndex];
                knotSum += knotLength;
                knotIndex += 1;
            }

            float ratio = knotLength > 0 
                ? (sectionsSum - (knotSum - knotLength)) / knotLength
                : 0f;
            
            return new Data(knotIndex, ratio);
        }
        
        private Segment GetP0(int indexP2)
        {
            return indexP2 >= 2 
                ? this.m_Knots[indexP2 - 2] 
                : this.ExtrapolatePrevKnot(indexP2 - 1);
        }
        
        private Segment GetP1(int indexP2)
        {
            return indexP2 >= 1
                ? this.m_Knots[indexP2 - 1] 
                : this.ExtrapolatePrevKnot(indexP2);
        }

        private Segment GetP2(int indexP2)
        {
            return this.m_Knots[indexP2];
        }
        
        private Segment GetP3(int indexP2)
        {
            return indexP2 < this.m_Knots.Count - 1
                ? this.m_Knots[indexP2 + 1]
                : this.ExtrapolateNextKnot(indexP2);
        }
        
        private Segment ExtrapolateNextKnot(int knotIndex)
        {
            Segment s0 = this.m_Knots[knotIndex - 2];
            Segment s1 = this.m_Knots[knotIndex - 1];
            Segment s2 = this.m_Knots[knotIndex - 0];

            Vector3 directionA0 = s1.PointA - s0.PointA;
            Vector3 directionB0 = s1.PointB - s0.PointB;
            
            Vector3 directionA1 = s2.PointA - s1.PointA;
            Vector3 directionB1 = s2.PointB - s1.PointB;

            Vector3 normalA = Vector3.Cross(directionA0, directionA1);
            Vector3 normalB = Vector3.Cross(directionB0, directionB1);
            
            float angleA = Vector3.SignedAngle(directionA0, directionA1, normalA);
            float angleB = Vector3.SignedAngle(directionB0, directionB1, normalB);
            
            Vector3 directionA2 = Quaternion.AngleAxis(angleA, normalA) * directionA1;
            Vector3 directionB2 = Quaternion.AngleAxis(angleB, normalB) * directionB1;

            return new Segment(
                s2.PointA + directionA2,
                s2.PointB + directionB2
            );
        }
        
        private Segment ExtrapolatePrevKnot(int knotIndex)
        {
            Segment s0 = this.m_Knots[knotIndex - 1];
            Segment s1 = this.m_Knots[knotIndex - 0];

            Vector3 directionA = s0.PointA - s1.PointA;
            Vector3 directionB = s0.PointB - s1.PointB;

            return new Segment(
                s0.PointA + directionA,
                s0.PointB + directionB
            );
        }
    }
}