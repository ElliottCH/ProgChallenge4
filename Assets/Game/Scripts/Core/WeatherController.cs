using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class WeatherController : MonoBehaviour
{
    [Serializable]
    public class Weather
    {
        public int id;
        public string main;
        public string description;
    }

    [Serializable]
    public class WeatherInfo
    {
        public int id;
        public string name;
        public List<Weather> weather;
    }

    private const string API_KEY = "";
    private const float updateTimeDelay = 600.0f;
    private float updateDelay = 0.0f;

    public string CityId = "6092122";
    public Text cityText;
    public Text weatherText;

    void Update()
    {
        updateDelay -= Time.deltaTime;
        if (updateDelay <= 0.0f)
        {
            WeatherInfo info = getWeather();

            cityText.text = info.name;
            if (info.weather.Count > 0)
            {
                weatherText.text = info.weather[0].main;
            }

            updateDelay = updateTimeDelay;
        }
    }

    private WeatherInfo getWeather()
    {

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
            String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", CityId, API_KEY)
            );

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();

        return JsonUtility.FromJson<WeatherInfo>(jsonResponse);
    }
}
