using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour
{
    //Loads Scene by Integer(Numbers)
    //Calls SceneManager Semi-asynchronously To Load Level
    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
        Debug.Log("Scene Loaded");
    }
}
