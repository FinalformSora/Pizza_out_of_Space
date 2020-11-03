using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public Dropdown resolutionDropDown;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> options = new List<string>();
        int current = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                current = i;
            }
        }
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = current;
        resolutionDropDown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
    }

    public void SetResolution(int resoolutionIndex)
    { 
        Screen.SetResolution(resolutions[resoolutionIndex].width, resolutions[resoolutionIndex].height, Screen.fullScreen);
    }
}
