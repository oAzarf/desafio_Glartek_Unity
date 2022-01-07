using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System;
using System.IO;

public class WeatherGetter : MonoBehaviour
{
    [SerializeField]
    Text date;
    [SerializeField]
    Text temp;
    [SerializeField]
    RawImage sunImageRaw;
    [SerializeField]
    Text temp_Min;
    [SerializeField]
    Text temp_Max;
    [SerializeField]
    Dropdown citySelected;
    [SerializeField]
    FakeDictionaryToDictionary<int, int> dropDownToAPI;
    public WeatherInfo informations;

    float apiCheckCountdown = 0;
    private const float API_CHECK_MAXTIME = 1f* 60f; 
    private const string API_KEY = "d4d1daa1a6131e8ea5bcd99882cc20e6";
    // Start is called before the first frame update    
    void Start()
    {
        dropDownToAPI?.WriteToDictionary(); // 

       
        //StartCoroutine(DownloadImage(10));

        //informations = GetWeather();

    }

    
    void Update()
    {
        GetDate();
        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            CallGetWeather();
            Debug.LogWarning("FindedInfo");
        }
    }

    public void CallGetWeather() // calls GetWeather and change texts being displayed
    {
        apiCheckCountdown = API_CHECK_MAXTIME;
        
        StartCoroutine( GetWeather());
        
    }

    void GetDate()
    {
        var hour = System.DateTime.Now.Hour;
        string stringHour = hour < 10 ? "0"+hour : ""+hour;
        var minute = System.DateTime.Now.Minute;
        string stringMinute = minute < 10 ? "0" + minute : "" + minute;
        var seconds = System.DateTime.Now.Second;
        string stringSeconds = seconds < 10 ? "0" + seconds : "" + seconds;

        var day = System.DateTime.Now.Day;
        string stringDay = day < 10 ? "0" + day : "" + day;
        var month = System.DateTime.Now.Month;
        string stringMonth = month < 10 ? "0" + month : "" + month;
        date.text = ($"{stringDay}-{stringMonth}-{System.DateTime.Now.Year} {stringHour}:{stringMinute}:{stringSeconds}"  );
    }
    //private WeatherInfo GetWeather()
    //{

    //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", dropDownToAPI.theRealDictionary[citySelected.value],API_KEY ));
    //    if (request==null)
    //    {
    //        Debug.LogWarning("No RequestForYou");
    //        return null;
    //    }
    //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    //    if (request == null)
    //    {
    //        Debug.LogWarning("No ResponseForYou");
    //        return null;
    //    }
    //    StreamReader reader = new StreamReader(response.GetResponseStream());
    //    string jsonResponse = reader.ReadToEnd();
    //    WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);

    //    return info;
    //}
    private IEnumerator  GetWeather()
    {
        yield return null;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", dropDownToAPI.theRealDictionary[citySelected.value], API_KEY));
        if (request == null)
        {
            Debug.LogWarning("No RequestForYou");
            yield break;
        }
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        if (request == null)
        {
            Debug.LogWarning("No ResponseForYou");
            yield break;
        }
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        
        informations = JsonUtility.FromJson<WeatherInfo>(jsonResponse);

        if (informations == null)
        {
            Debug.LogWarning("FoundNothing");
            yield break;
        }
        StartCoroutine(DownloadImage(informations.weather[0].icon));

        temp.text = KelvinToCelcios(informations.main.temp).ToString() + "º";
        temp_Max.text =  KelvinToCelcios(informations.main.temp_max).ToString() + "º";
        temp_Min.text =KelvinToCelcios(informations.main.temp_min).ToString() + "º";
    }

    private float KelvinToCelcios(float inKevin)
    {
        var tempInCelcios = inKevin - 273.15f;
        var aprox = Mathf.Round(tempInCelcios);

        return aprox;
    }


    IEnumerator DownloadImage(string weatherIcon)
    {

        string url = $"http://openweathermap.org/img/wn/{weatherIcon}@2x.png";
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            
            sunImageRaw.texture= ((DownloadHandlerTexture)request.downloadHandler).texture;
            
        }
            
    }
    IEnumerator DownloadImage(RawImage imageToGetTheTexture,string thisIsWithGivenURL)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(thisIsWithGivenURL);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {

            imageToGetTheTexture.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

        }

    }

}

[System.Serializable]
public class FakeDictionary <Tkey,Tvalue>
{
    [SerializeField] public Tkey key;
    [SerializeField] public Tvalue value;
}
[System.Serializable]
public class FakeDictionaryToDictionary<Tkey, Tvalue>
{
    [SerializeField] List<FakeDictionary<Tkey, Tvalue>> theCreatedfakeDictionary;
    public  Dictionary<Tkey, Tvalue> theRealDictionary = new Dictionary<Tkey, Tvalue>();

    public FakeDictionaryToDictionary()
    {
                
    }
    public void WriteToDictionary()
    {
        if (theCreatedfakeDictionary != null)
        {
            foreach (var item in theCreatedfakeDictionary)
            {
                theRealDictionary.Add(item.key, item.value);
            }
            Debug.Log($"Nice Tenho um Dicionario com isto {theRealDictionary.Count}");
        }
    }

}


[Serializable]
public class Weather
{
    public int id;
    public string main;
    public string description;
    public string icon;
}
[Serializable]
public class Main
{
    public float temp;
    public float temp_min;
    public float temp_max;

}
[Serializable]
public class WeatherInfo
{
    public int id;
    public string name;
    public List<Weather> weather;

    public Main main;
}