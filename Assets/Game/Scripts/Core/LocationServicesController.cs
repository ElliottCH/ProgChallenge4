using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationServicesController : MonoBehaviour
{
    public Text longitudeText;
    public Text latitudeText;

    void Start()
    {
        longitudeText.text = "";
        latitudeText.text = "";
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
            longitudeText.text = "Timed out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            longitudeText.text = "Failed to determine device location";
            yield break;
        }
        else
        {
            longitudeText.text = Input.location.lastData.longitude.ToString();
            latitudeText.text = Input.location.lastData.latitude.ToString();
        }

        Input.location.Stop();
    }
}
