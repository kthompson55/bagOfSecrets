using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        StartCoroutine("StartGame");
	}

    void Update()
    {
        if(Input.GetButton("Progress"))
        {
            StopCoroutine("StartGame");
            Application.LoadLevel("Mitch'sEdits");
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(10);
        Application.LoadLevel("Mitch'sEdits");
    }
}
