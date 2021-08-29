using Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    private bool _submitted;

    public void Submit()
    {
        if (!_submitted)
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
