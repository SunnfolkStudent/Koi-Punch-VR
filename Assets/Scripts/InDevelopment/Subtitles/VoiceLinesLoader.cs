using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;

public static class VoiceLinesLoader
{
    public static string GetValueForKey(string key, int _randomIndex)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "SubtitlesTXTFiles/VoiceLines.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return null;
        }

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        JArray voiceLinesArray = (JArray)jsonObject["VoiceLines"];

        if (voiceLinesArray == null)
        {
            Debug.LogError("VoiceLines array not found.");
            return null;
        }

        foreach (JObject voiceLineObj in voiceLinesArray)
        {
            foreach (var property in voiceLineObj.Properties())
            {
                // If the key is "OnPunch" and the category is "Punch Comment"
                if (key == "OnPunch" && voiceLineObj["Category"].ToString() == "Punch Comment" && property.Name == "OnPunch")
                {
                    JArray punchCommentsArray = (JArray)property.Value;
                    if (punchCommentsArray != null && punchCommentsArray.Count > 0)
                    {
                        //int randomIndex = Random.Range(0, punchCommentsArray.Count);
                        return punchCommentsArray[_randomIndex].ToString();
                    }
                }

                // If the key matches the property name
                if (property.Name == key)
                {
                    return property.Value.ToString();
                }
            }
        }

        Debug.LogError("Key not found in the voice lines data.");
        return null;
    }
}
