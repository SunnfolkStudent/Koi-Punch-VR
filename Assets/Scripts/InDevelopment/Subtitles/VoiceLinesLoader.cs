using Newtonsoft.json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLinesLoader : MonoBehaviour
{
    // Reference to the JSON document in the Unity Inspector
    public TextAsset jsonFile;

    // Dictionary to store the key-value pairs from the JSON
    private Dictionary<string, string> voiceLinesDictionary;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the jsonFile is assigned
        if (jsonFile != null)
        {
            // Load the JSON data from the TextAsset
            string jsonContent = jsonFile.text;

            // Deserialize the JSON data into a Dictionary
            voiceLinesDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);

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
        
        // Add the dictionary to the VoiceLinesContainer
        VoiceLinesContainer.voiceLinesDict = voiceLinesDictionary;
    }

    // Update is called once per frame
    void Update()
    {
        // Your update logic here
    }
}