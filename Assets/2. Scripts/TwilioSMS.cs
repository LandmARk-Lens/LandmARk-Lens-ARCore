using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwilioSMS : MonoBehaviour
{

    public void sendLandmarkCityToWeb()
    {
        string landmarkCity = gameObject.GetComponent<LandmarkDetect>().landmarkCity;
        // if landmarkName is empty, set it as Paris as default landmarkCity
        landmarkCity = landmarkCity.Equals("") ? "Paris" : landmarkCity;
        string sendLandmarkUrl = "http://joshuawang.me/get_landmark_city?landmarkCity=" + landmarkCity;

        WWW wwwGetLandmark = new WWW(sendLandmarkUrl);
        StartCoroutine(WaitForLandmarkRequest(wwwGetLandmark, landmarkCity));
    }

    IEnumerator WaitForLandmarkRequest(WWW wwwGetLandmark, string landmarkCity)
    {
        yield return wwwGetLandmark;

        // check for errors
        if (wwwGetLandmark.error == null)
        {
            string from = "+13174276065";
            string to = "+14703995427";
            string accountSid = "AC3beec661c21e890d91c3848ec56bab3c";
            string authToken = "0d817e48b138e7e1ec76c24d05c8da86";
            string body = "When are you planning to travel " + landmarkCity + "? (ex 4/1 ~ 4/7)";
            WWWForm form = new WWWForm();

            form.AddField("To", to);
            form.AddField("From", from);
            form.AddField("Body", body);
            string url = "https://" + accountSid + ":" + authToken + "@api.twilio.com/2010-04-01/Accounts/" + accountSid + "/Messages.json";
            WWW www = new WWW(url, form);
            StartCoroutine(WaitForSMSRequest(www));
            Debug.Log("WWW getLandmark ok: " + wwwGetLandmark.text);
        }
        else
        {
            Debug.Log("WWW Error: " + wwwGetLandmark.error);
        }
    }

    IEnumerator WaitForSMSRequest(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW succeeded!: " + www.text);
        }
        else
        {
            Debug.Log("WWW error!: " + www.error);
        }
    }
}
