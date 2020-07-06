using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using SimpleJSON;

[RequireComponent(typeof(ARRaycastManager))]

public class SpawnObject : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnObject;

    public GameObject ActiveCasesPrefab;
    public GameObject RecoveredCasesPrefab;
    public GameObject DeathCasesPrefab;

    private readonly string covid_url = "https://api.apify.com/v2/key-value-stores/toDWvRj1JpTXiM8FF/records/LATEST?disableRedirect=true";
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private int recovered;
    private int active_cases;
    private int deaths;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        StartCoroutine(GetCovidData());
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPos = s_Hits[0].pose;
            Debug.Log(active_cases);
            
            if(spawnObject == null)
            {
                for(int i = 0; i < active_cases / 10000; i++)
                {
                    spawnObject = Instantiate(ActiveCasesPrefab, new Vector3(hitPos.position.x, hitPos.position.y + 0.7f, hitPos.position.z), Random.rotation);
                }

                for (int i = 0; i < deaths / 10000; i++)
                {
                    spawnObject = Instantiate(DeathCasesPrefab, new Vector3(hitPos.position.x, hitPos.position.y + 0.7f, hitPos.position.z), Random.rotation);
                }

                for (int i = 0; i < recovered / 10000; i++)
                {
                    spawnObject = Instantiate(RecoveredCasesPrefab, new Vector3(hitPos.position.x, hitPos.position.y + 0.7f, hitPos.position.z), Random.rotation);
                }
            }


        }
    }

    IEnumerator GetCovidData()
    {
        UnityWebRequest requestInfo = UnityWebRequest.Get(covid_url);
        yield return requestInfo.SendWebRequest();
        if (requestInfo.isNetworkError || requestInfo.isHttpError)
        {
            Debug.LogError(requestInfo.error);
            yield break;
        }
        JSONNode covid_info = JSON.Parse(requestInfo.downloadHandler.text);
        active_cases = covid_info["activeCases"];
        recovered = covid_info["recovered"];
        deaths = covid_info["deaths"];
    }
}
