using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLinesLoader : MonoBehaviour
{
    // Reference to the JSON document in the Unity Inspector
    public TextAsset jsonFile;

    // Dictionary to store the key-value pairs from the JSON
    private Dictionary<string, string> voiceLinesDictionary;

    // Start is called before the first frame update
    void Awake()
    {
        // Check if the jsonFile is assigned
        if (jsonFile != null)
        {
            // Load the JSON data from the TextAsset
            string jsonContent = jsonFile.text;

            // Deserialize the JSON data into a dynamic object
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonContent);

            // Extract the "VoiceLines" array from the dynamic object
            var voiceLinesArray = jsonObject["VoiceLines"];

            // Initialize the dictionary
            voiceLinesDictionary = new Dictionary<string, string>();

            // Iterate through the key-value pairs inside the "VoiceLines" array
            foreach (var item in voiceLinesArray)
            {
                foreach (var kvp in item)
                {
                    // Add each key-value pair to the dictionary
                    voiceLinesDictionary.Add(kvp.Name, kvp.Value.ToString()); // Convert values to string
                }
            }

            // Check if deserialization was successful
            if (voiceLinesDictionary != null)
            {
                // Print the contents of the dictionary for testing
                foreach (var kvp in voiceLinesDictionary)
                {
                    Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
                }
            }
            else
            {
                Debug.LogError("Failed to deserialize JSON data.");
            }
        }
        else
        {
            Debug.LogError("JSON file not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Your update logic here
    }
}
