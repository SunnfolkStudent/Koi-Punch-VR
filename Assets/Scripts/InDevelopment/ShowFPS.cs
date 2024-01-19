using System.Globalization;
using UnityEngine;

namespace InDevelopment
{
    [ExecuteInEditMode]
    public class ShowFPS : MonoBehaviour 
    {
        [SerializeField] private TMPro.TextMeshProUGUI fpsText;
        [Header("Enable this bool to enable the FPS Counter and code.")]
        [SerializeField] private bool enableFPSCounter = true;
        
        private int count;
        [Header("Reduce the samples number to update the FPS number more often")]
        public int samples = 100;
        private float totalTime;
 
        public void Start()
        {
            count = samples;
            totalTime = 0f;
        }
 
        public void Update()
        {
            if (enableFPSCounter)
            {
                fpsText.gameObject.SetActive(true);
                count -= 1;
                totalTime += Time.deltaTime;
               
                if (count <= 0) {
                    float fps = samples / totalTime;
                    DisplayFPS(fps);
                    totalTime = 0f;
                    count = samples;
                }  
            }
            else
            {
                fpsText.gameObject.SetActive(false);
            }
        }

        private void DisplayFPS(float valueFPS)
        {
            fpsText.text = Mathf.Ceil(valueFPS).ToString(CultureInfo.InvariantCulture);
        }
    }
}
