using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelCollision : MonoBehaviour
{

    public GameObject nextLevelUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            nextLevelUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
