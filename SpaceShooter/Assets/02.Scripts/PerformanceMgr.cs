using UnityEngine;
using UnityEngine.iOS;
using System.Collections;

public class PerformanceMgr : MonoBehaviour {
    
    private LightShadows shadowType = LightShadows.None;
    
    void Awake() {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            switch (Device.generation)
            {
            case DeviceGeneration.iPhone5S:
                shadowType = LightShadows.Hard;
                break;
            case DeviceGeneration.iPhone6:
                shadowType = LightShadows.Soft;
                break;
            case DeviceGeneration.iPhone6Plus:
                shadowType = LightShadows.Soft;
                break;
            default:
                shadowType = LightShadows.None;
                break;
            }
            GameObject.Find("Directional Light").GetComponent<Light>().shadows = shadowType;
        }
    }
}
