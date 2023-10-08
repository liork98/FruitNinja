using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScene : MonoBehaviour
{
    public string scene_name;

    public void ChangeScene()
    {
        SceneManager.LoadScene(scene_name);
    }
}
