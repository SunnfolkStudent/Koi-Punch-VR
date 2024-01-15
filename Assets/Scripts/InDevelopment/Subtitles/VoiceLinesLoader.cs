using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;

public static class VoiceLinesLoader
{
    // public static Dictionary<string, string> voiceLinesDict = new Dictionary<string, string>();

    public static Dictionary<string, string> LoadCategoryKeysAndValues(string category)
    {
        // Create a local dictionary to hold the data
        Dictionary<string, string> voiceLinesDict = new Dictionary<string, string>();

        string filePath = Path.Combine(Application.streamingAssetsPath, "SubtitlesTXTFiles/VoiceLines.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return voiceLinesDict; 
        }

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        JArray voiceLinesArray = (JArray)jsonObject["VoiceLines"];

        if (voiceLinesArray == null)
        {
            Debug.LogError("VoiceLines array not found.");
            return voiceLinesDict; 
        }
        
        voiceLinesDict.Clear();
        
        foreach (JObject voiceLineObj in voiceLinesArray)
        {
            // Check if the current object belongs to the specified category.
            if (voiceLineObj["Category"].ToString() == category)
            {
                
                foreach (var property in voiceLineObj.Properties())
                {
                    voiceLinesDict[property.Name] = property.Value.ToString();
                }
            }
        }

        // Return the populated dictionary
        return voiceLinesDict;
    }
}
