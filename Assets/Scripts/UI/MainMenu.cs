using System;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    private bool _submitted;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Submit();
        }
    }

    public void Submit()
    {
        if (!_submitted && !string.IsNullOrEmpty(inputField.text))
        {
            _submitted = true;
            GameController.PlayerInfo = new PlayerInfo()
            {
                Name = inputField.text,
                Color = Random.ColorHSV()
            };
            SceneManager.LoadScene("Galaxy");
        }
    }
}