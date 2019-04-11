using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationServicesController : MonoBehaviour
{
    public string longitudeText;
    public string latitudeText;

    void Start()
    {
        longitudeText = "";
        latitudeText = "";
        StartCoroutine(LocationServiceUpdate());
    }

    IEnumerator LocationServiceUpdate()
    {
        Input.location.Start();

        int waitTime = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime > 0)
        {
            yield return new WaitForSeconds(1);
            waitTime--;
        }

        if (waitTime <= 0)
        {
            longitudeText = "Timed out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            longitudeText = "Failed to determine device location";
            yield break;
        }
        else
        {
            longitudeText = Input.location.lastData.longitude.ToString();
            latitudeText = Input.location.lastData.latitude.ToString();
        }

        Input.location.Stop();
    }
}