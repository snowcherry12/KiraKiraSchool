using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class SettingResolution : MonoBehaviour
{
    private List<Resolution> resolutions;
    void Start()
    {
        var dropdown = GetComponent<Dropdown>();
        resolutions = new List<Resolution>(Screen.resolutions);
        dropdown.ClearOptions();
        dropdown.options = resolutions.ConvertAll(resolution => new Dropdown.OptionData($"{resolution.width}x{resolution.height} {Math.Round(resolution.refreshRateRatio.value)} Hz"));
        dropdown.value = resolutions.IndexOf(Screen.currentResolution);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);
    }
}
