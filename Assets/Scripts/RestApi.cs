using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;
using UnityEngine;

public class RestApi : MonoBehaviour
{
    public GameObject dataprefabs;

    public Transform contentPanel;
    [System.Serializable]
    public class Root
    {
        public int userId;
        public int id;
        public string title;
    }
    public List<Root> users = new List<Root>();




    public Button getButton;

    public Text OutputArea;
    string url = "https://jsonplaceholder.typicode.com/albums";

    [System.Obsolete]
    void Start()
    {
        getButton.onClick.AddListener(OnClickGetButton);
        //OutputArea = GameObject.Find("outPutArea").GetComponent<Text>();

    }

    [System.Obsolete]
    public void OnClickGetButton()
    {
        GateData();
    }

    [System.Obsolete]
    void GateData() => StartCoroutine(downloadHttpData());

    [System.Obsolete]
    IEnumerator downloadHttpData()
    {
        OutputArea.text = "Loading....";
        //UnityWebRequest webReq = new UnityWebRequest(url);
        //webReq.downloadHandler = new DownloadHandlerBuffer();


        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                OutputArea.text = request.error;
                Debug.Log(request.error);

            }
            else
            {
                OutputArea.text = request.downloadHandler.text;
                Debug.Log(request.downloadHandler.text);
                users = JsonConvert.DeserializeObject<List<Root>>(request.downloadHandler.text);
                DisplayData();
            }

        }

    }

    public void DisplayData()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (var user in users)
        {
            GameObject newUser = Instantiate(dataprefabs, contentPanel);

            Text[] texts = newUser.GetComponentsInChildren<Text>();

            if (texts.Length <= 3)
            {

                texts[2].text = "" + user.userId;
                texts[1].text = "" + user.id;
                texts[0].text = "" + user.title;

            }
            else
            {
                Debug.Log("Prefabs does not have enough text");
            }

        }
    }
}
