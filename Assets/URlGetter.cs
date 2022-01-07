using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

using UnityEngine.Networking;

public class URlGetter : MonoBehaviour
{
    [SerializeField]
    VideoPlayer video;
    [SerializeField]
    RawImage image;
    [SerializeField]
    Image hold;
    [SerializeField]
    GameObject buton;

    int LoadedAll;

    private const string videoURL = "https://file-examples-com.github.io/uploads/2017/04/file_example_MP4_480_1_5MG.mp4";
    private const string imageURL = "https://images.squarespace-cdn.com/content/v1/56d8ba4ab654f9a47f6d39fa/1480447528456-YVOR69SOKYNTMBV9WXMZ/glartek_logo.png";

    IEnumerator DownloadVideo(string videoUrl)
    {

        video.url = videoUrl;

        UnityWebRequest request = UnityWebRequest.Get(videoUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Debug.Log("Play now");

            video.Play();
            LoadedAll++;

        }

    }
    IEnumerator DownloadImage(string imageUrl)
    {

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Debug.Log("Set Image now");
            image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            image.color = Color.white;
            LoadedAll++;
        }

    }

    public void CallFromButton()
    {
        StartCoroutine(WaintingForBoth());
        buton.SetActive(false);

    }

    IEnumerator WaintingForBoth()
    {
        //StartCoroutine(Loading());
        StartCoroutine(DownloadVideo(videoURL));
        StartCoroutine(DownloadImage(imageURL)); ;
        while (LoadedAll<1)
        {
            yield return null;
        }


        yield return new WaitForSeconds(1f);
    }

}
