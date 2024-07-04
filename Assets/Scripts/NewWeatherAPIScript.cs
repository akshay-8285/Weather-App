using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class NewWeatherAPIScript : MonoBehaviour
{
    public TMP_InputField cityInputField;
    public Button getWeatherButton;
    public TMP_Text weatherText;
    public Image image;
    private string apiKey = "02f89fe67abd27dc3dfdd3c8350d46fc";

    private Vector3 originalScale;

    void Start()
    {
        originalScale = image.rectTransform.localScale;
        getWeatherButton.onClick.AddListener(GetWeather);
    }

    void GetWeather()
    {
        string city = cityInputField.text;
        if (!string.IsNullOrEmpty(city))
        {
            StartCoroutine(GetWeatherCoroutine(city));
        }
    }

    private IEnumerator GetWeatherCoroutine(string city)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                weatherText.text = "Error: " + webRequest.error;
            }
            else
            {
                string json = webRequest.downloadHandler.text;
                WeatherInfo weatherInfo = JsonUtility.FromJson<WeatherInfo>(json);
                LeanTween.scale(image.rectTransform, originalScale * 0.5f, 0.5f).setOnComplete(() =>
                {
                   
                    weatherText.text = $"Temperature: {weatherInfo.main.temp}°C\n" +
                                       $"Weather: {weatherInfo.weather[0].description}";
                    LeanTween.scale(image.rectTransform, originalScale, 0.5f).setOnComplete(() =>
                    {
                        // Reset the scale to the original scale to ensure animation runs again
                        image.rectTransform.localScale = originalScale;
                    });
                });

                
            }
        }
    }
}

[System.Serializable]
public class WeatherInfo
{
    public Main main;
    public Weather[] weather;
}

[System.Serializable]
public class Main
{
    public float temp;
}

[System.Serializable]
public class Weather
{
    public string description;
}
