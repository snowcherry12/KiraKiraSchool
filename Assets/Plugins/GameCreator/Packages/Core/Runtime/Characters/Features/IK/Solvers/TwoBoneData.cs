using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    public struct TwoBoneData
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public Vector3 WorldPosition { get; set; }
        [field: NonSerialized] public Quaternion WorldRotation { get; set; }
        [field: NonSerialized] public Vector3 WorldScale { get; set; }

        [field: NonSerialized] public Vector3 RootLocalPosition { get; set; }
        [field: NonSerialized] public Quaternion RootLocalRotation { get; set; }
        [field: NonSerialized] public Vector3 RootLocalScale { get; set; }

        [field: NonSerialized] public Vector3 BodyLocalPosition { get; set; }
        [field: NonSerialized] public Quaternion BodyLocalRotation { get; set; }
        [field: NonSerialized] public Vector3 BodyLocalScale { get; set; }

        [field: NonSerialized] public Vector3 HeadLocalPosition { get; set; }
        [field: NonSerialized] public Quaternion HeadLocalRotation { get; set; }
        [field: NonSerialized] public Vector3 HeadLocalScale { get; set; }

        // ROOT PROPERTIES: -----------------------------------------------------------------------
        
        public Vector3 RootPosition
        {
            get => TransformUtils.TransformPoint(
                this.RootLocalPosition,
                this.WorldPosition,
                this.WorldRotation,
                this.WorldScale
            );
            set => this.RootLocalPosition = TransformUtils.InverseTransformPoint(
                value,
                this.WorldPosition,
                this.WorldRotation,
                this.WorldScale
            );
        }
        
        public Quaternion RootRotation
        {
            get => TransformUtils.TransformRotation(
                this.RootLocalRotation,
                this.WorldPosition,
                this.WorldRotation,
                this.WorldScale
            );
            set => this.RootLocalRotation = TransformUtils.InverseTransformRotation(
                value,
                this.WorldPosition,
                this.WorldRotation,
                this.WorldScale
            );
        }
        
        public Vector3 RootScale
        {
            get => new Vector3(
                this.WorldScale.x * this.RootLocalScale.x,
                this.WorldScale.y * this.RootLocalScale.y,
                this.WorldScale.z * this.RootLocalScale.z
            );
            set => this.RootLocalScale = new Vector3(
                value.x / this.WorldScale.x,
                value.y / this.WorldScale.y,
                value.z / this.WorldScale.z
            );
        }
        
        // BODY PROPERTIES: -----------------------------------------------------------------------
        
        public Vector3 BodyPosition
        {
            get => TransformUtils.TransformPoint(
                this.BodyLocalPosition,
                this.RootPosition,
                this.RootRotation,
                this.RootScale
            );
            set => this.BodyLocalPosition = TransformUtils.InverseTransformPoint(
                value,
                this.WorldPosition,
                this.WorldRotation,
                this.WorldScale
            );
        }
        
        public Quaternion BodyRotation
        {
            get => TransformUtils.TransformRotation(
                this.BodyLocalRotation,
                this.RootPosition,
                this.RootRotation,
                this.RootScale
            );
            set => this.BodyLocalRotation = TransformUtils.InverseTransformRotation(
                value,
                this.RootPosition,
                this.RootRotation,
                this.RootScale
            );
        }
        
        public Vector3 BodyScale
        {
            get => new Vector3(
                this.RootScale.x * this.BodyLocalScale.x,
                this.RootScale.y * this.BodyLocalScale.y,
                this.RootScale.z * this.BodyLocalScale.z
            );
            set => this.BodyLocalScale = new Vector3(
                value.x / this.RootScale.x,
                value.y / this.RootScale.y,
                value.z / this.RootScale.z
            );
        }
        
        // HEAD PROPERTIES: ----------------------------------------------------------------------
        
        public Vector3 HeadPosition
        {
            get => TransformUtils.TransformPoint(
                this.HeadLocalPosition,
                this.BodyPosition,
                this.BodyRotation,
                this.BodyScale
            );
            set => this.HeadLocalPosition = TransformUtils.InverseTransformPoint(
                value,
                this.BodyPosition,
                this.BodyRotation,
                this.BodyScale
            );
        }
        
        public Quaternion HeadRotation
        {
            get => TransformUtils.TransformRotation(
                this.HeadLocalRotation,
                this.BodyPosition,
                this.BodyRotation,
                this.BodyScale
            );
            set => this.HeadLocalRotation = TransformUtils.InverseTransformRotation(
                value,
                this.BodyPosition,
                this.BodyRotation,
                this.BodyScale
            );
        }
        
        public Vector3 HeadScale
        {
            get => new Vector3(
                this.BodyScale.x * this.HeadLocalScale.x,
                this.BodyScale.y * this.HeadLocalScale.y,
                this.BodyScale.z * this.HeadLocalScale.z
            );
            set => this.HeadLocalScale = new Vector3(
                value.x / this.BodyScale.x,
                value.y / this.BodyScale.y,
                value.z / this.BodyScale.z
            );
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TwoBoneData(Transform root, Transform body, Transform head)
        {
            this.WorldPosition = root.parent != null ? root.parent.position : Vector3.zero;
            this.WorldRotation = root.parent != null ? root.parent.rotation : Quaternion.identity;
            this.WorldScale = root.parent != null ? root.parent.lossyScale : Vector3.one;

            this.RootLocalPosition = root.localPosition;
            this.RootLocalRotation = root.localRotation;
            this.RootLocalScale = root.localScale;

            this.BodyLocalPosition = body.localPosition;
            this.BodyLocalRotation = body.localRotation;
            this.BodyLocalScale = body.localScale;

            this.HeadLocalPosition = head.localPosition;
            this.HeadLocalRotation = head.localRotation;
            this.HeadLocalScale = head.localScale;
        }
    }
}