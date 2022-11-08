using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickTransitioner : MonoBehaviour
{
    [SerializeField] private int _sceneBuildIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(_sceneBuildIndex);
    }
}
