using UnityEngine;
using System.Collections;

public class AudioClipData : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] string samplesString;
    [SerializeField] int frequency;
    [SerializeField] int channels;
    void OnValidate()
    {
        StartCoroutine(GetAudioData());
    }

    IEnumerator GetAudioData()
    {
        // Wait for sample data to be loaded
        while (clip.loadState != AudioDataLoadState.Loaded)
        {
            yield return null;
        }

        var numSamples = clip.samples * clip.channels;
        var samples = new float[numSamples];
        clip.GetData(samples, 0);
        frequency = clip.frequency;
        channels = clip.channels;
        samplesString = "";
        for (int i = 0; i < samples.Length; ++i)
        {
            samplesString += samples[i] + "f,";
            if (i%5 == 0)samplesString += "\n";
            else samplesString += " ";
        }

        clip.SetData(samples, 0);
    }
}