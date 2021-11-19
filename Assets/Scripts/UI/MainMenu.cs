using Manager;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField inputField;
        
        [SerializeField]
        private int maxNameLenght;

        private bool _submitted;

        private void Start()
        {
            inputField.characterLimit = maxNameLenght;
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
}