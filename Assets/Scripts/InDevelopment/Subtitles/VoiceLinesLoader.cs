using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;

public static class VoiceLinesLoader
{
    public static Dictionary<string, string> voiceLinesDict = new Dictionary<string, string>();

    public static void LoadCategoryKeysAndValues(string category)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "SubtitlesTXTFiles/VoiceLines.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        JArray voiceLinesArray = (JArray)jsonObject["VoiceLines"];

        if (voiceLinesArray == null)
        {
            Debug.LogError("VoiceLines array not found.");
            return;
        }

        // Clear the existing dictionary before populating with new data.
        voiceLinesDict.Clear();

        // Iterate through each object in the "VoiceLines" array.
        foreach (JObject voiceLineObj in voiceLinesArray)
        {
            // Check if the current object belongs to the specified category.
            if (voiceLineObj["Category"].ToString() == category)
            {
                // Iterate through each property (key-value pair) in the current JObject.
                foreach (var property in voiceLineObj.Properties())
                {
                    // Add the property name (key) and its string representation (value) to the dictionary.
                    voiceLinesDict[property.Name] = property.Value.ToString();
                }
            }
        }
    }
}