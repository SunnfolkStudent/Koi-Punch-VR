using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightingLoader : MonoBehaviour
{
   [SerializeField] private String scene;

   private void Start()
   {
      SceneManager.LoadScene(scene, LoadSceneMode.Additive);
   }
}
