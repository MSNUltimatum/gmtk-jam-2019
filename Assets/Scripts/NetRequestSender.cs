using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetRequestSender : MonoBehaviour
{
    public bool debugMode = false;

    protected IEnumerator GetRequest(string uri)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        yield return webRequest.SendWebRequest();

        DebugLogNetworkRequest(webRequest);
    }

    protected IEnumerator PostRequest(string url, WWWForm form)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
        yield return webRequest.SendWebRequest();

        DebugLogNetworkRequest(webRequest);
    }

    protected IEnumerator PostRequestJson(string url, string json)
    {
        var webRequest = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        DebugLogNetworkRequest(webRequest);
    }

    private void DebugLogNetworkRequest(UnityWebRequest webRequest)
    {
        if (!debugMode) return;

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("Error: " + webRequest.error);
        }
        else
        {
            Debug.Log("Received: " + webRequest.downloadHandler.text);
        }
    }
}