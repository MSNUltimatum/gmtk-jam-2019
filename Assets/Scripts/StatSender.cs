using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatSender : NetRequestSender
{
    public const string URL_ADDSTATS = "https://stats.gd64.karmanline.ru/addstats/";

    private void Awake()
    {
        isWaiting = true;
        if (!PlayerPrefs.HasKey("user_id"))
        {
            var newID = Mathf.FloorToInt(Random.Range(0, 1000000));
            PlayerPrefs.SetInt("user_id", newID);
            PlayerPrefs.Save();
        }
        userID = PlayerPrefs.GetInt("user_id", -1);
    }

    private void Update()
    {
        // Tracing the variables 
        if (!isWaiting) return;

        string json = "";
        if (CharacterLife.isDeath)
        {
            json = BuildJsonForm1(false, "null");
        }
        else if (RelodScene.isVictory)
        {
            json = BuildJsonForm1(true, "null");
        }

        if (json != "")
        {
            StartCoroutine(PostRequestJson(URL_ADDSTATS, json));
            isWaiting = false;
        }
    }

    private string BuildJsonForm1(bool isWinner, string killedBy)
    {
        string sceneName = '\"' + SceneManager.GetActiveScene().name + '\"';
        var json = new StringBuilder();
        json.Append('{');
        AddJsonPair(json, "type", "1");
        AddJsonPair(json, "user_id", userID.ToString());
        AddJsonPair(json, "level", sceneName);
        AddJsonPair(json, "is_winner", isWinner.ToString().ToLower());
        AddJsonPair(json, "killed_by", killedBy);
        AddJsonPair(json, "enemies", "{}");
        AddJsonPair(json, "round_duration", "-1");
        AddJsonPair(json, "shots_fired", "-1");
        AddJsonPair(json, "units_walked", "-1");
        AddJsonPair(json, "avg_fps", "-1");
        AddJsonPair(json, "machine", "{}");
        // Getting rid of the last comma
        json.Remove(json.Length - 1, 1);
        json.Append('}');
        return json.ToString();
    }

    private void AddJsonPair(StringBuilder sb, string key, string param)
    {
        sb.Append('\"' + key + '\"' + ':' + param + ',');
    }

    private int userID = -1;
    private bool isWaiting;
}