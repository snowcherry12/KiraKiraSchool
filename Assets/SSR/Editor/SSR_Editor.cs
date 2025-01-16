using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEditor.Rendering.Universal
{
    [CustomEditor(typeof(ScreenSpaceReflections))]
    sealed class SSR_Editor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;

        SerializedDataParameter downsample;

        SerializedDataParameter steps;
        SerializedDataParameter stepSize;
        SerializedDataParameter thickness;

        SerializedDataParameter samples;
        SerializedDataParameter minSmoothness;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<ScreenSpaceReflections>(serializedObject);

            enabled = Unpack(o.Find(x => x.enabled));
            downsample = Unpack(o.Find(x => x.downsample));
            steps = Unpack(o.Find(x => x.steps));
            stepSize = Unpack(o.Find(x => x.stepSize));
            thickness = Unpack(o.Find(x => x.thickness));
            samples = Unpack(o.Find(x => x.samples));
            minSmoothness = Unpack(o.Find(x => x.minSmoothness));
        }

        public override void OnInspectorGUI()
        {
            ScreenSpaceReflections ssr = (ScreenSpaceReflections)target;

            //Create Property Fields
            PropertyField(enabled);
            PropertyField(downsample);
            PropertyField(steps);
            PropertyField(stepSize);
            PropertyField(thickness);
            PropertyField(samples);
            PropertyField(minSmoothness);
        }
    }
}
