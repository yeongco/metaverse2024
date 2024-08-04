using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCMenu : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    public void Start()
    {
        Menu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
