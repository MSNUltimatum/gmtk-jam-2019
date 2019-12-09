using UnityEngine;

public class NetRequestExample : NetRequestSender
{
    protected void Update()
    {
        if (!infoSent && RelodScene.isVictory)
        {
            Debug.Log("Victory condition met. Sending data...");

            // GET
            StartCoroutine(GetRequest("https://stats.gd64.karmanline.ru/testurl?data=victory"));

            // POST
            var form = new WWWForm();
            form.AddField("data", "victory");
            StartCoroutine(PostRequest("https://stats.gd64.karmanline.ru/testurl/", form));

            // POST /w JSON
            string json = "{\"user_id\": 1," +
                "\"type\": 1," +
                "\"is_winner\": false," + 
                "\"killed_by\": \"enemy1\"," +
                "\"enemies\": {" +
                "\"enemy1\": 102," +
                "\"enemy2\": 20," +
                "\"enemy3\": 4" +
                "}," +
                "\"round_duration\": 124213," +
                "\"shots_fired\": 200," +
                "\"units_walked\": 303.4," +
                "\"avg_fps\": 50.3," +
                "\"machine\": {}" +
                "}";
            
            StartCoroutine(PostRequestJson("https://stats.gd64.karmanline.ru/addstats/", json));

            infoSent = true;
        }
    }

    private bool infoSent = false;
}