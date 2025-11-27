using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyApi
{
    public class TestApi : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_InputField _inputField;
        
        public ApiClient client;

        private void Awake()
        {
            _button.onClick.AddListener(GetValues);
        }

        private async void GetValues()
        {
            var id = Convert.ToInt32(_inputField.text);
            var value = await client.GetUser(id);

            _resultText.text = value;
        }

        [ContextMenu("Try It")]
        private void TryIt()
        {
            // Создание пользователя
            client.CreateUser("Hello World", 24);
        }

        [ContextMenu("get user")]
        private void GetUser()
        {
            client.GetUser(4);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }

}