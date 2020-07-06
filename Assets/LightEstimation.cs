using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class LightEstimation : MonoBehaviour
{
    // Start is called before the first frame update
    private ARCameraManager aRCameraManager;
    private Light currentLight;

    private void Awake()
    {
        currentLight = GetComponent<Light>();
    }
    private void OnEnable()
    {
        aRCameraManager.frameReceived += FrameUpdated;
    }

    private void OnDisable()
    {
        aRCameraManager.frameReceived -= FrameUpdated;  
    }

    private void FrameUpdated(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            currentLight.intensity = args.lightEstimation.averageBrightness.Value;
        }

        if (args.lightEstimation.colorCorrection.HasValue)
        {
            currentLight.color = args.lightEstimation.colorCorrection.Value;
        }

        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            currentLight.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
        }
    }
 }
