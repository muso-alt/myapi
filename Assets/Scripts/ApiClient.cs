using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    private string baseUrl = "http://localhost:5043/api/users"; // ваш API

    [System.Serializable]
    public class UserData
    {
        public int id;
        public string name;
        public int age;
    }

    // =========================
    // 📌 POST: Create User
    // =========================
    public void CreateUser(string name, int age)
    {
        UserData user = new UserData { name = name, age = age };
        StartCoroutine(PostRequest(user));
    }

    private IEnumerator PostRequest(UserData newUser)
    {
        string json = JsonUtility.ToJson(newUser);

        UnityWebRequest request = new UnityWebRequest(baseUrl, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("User created: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("POST Error: " + request.error);
        }
    }

    // =========================
    // 📌 GET: Get User by ID
    // =========================
    public UniTask<string> GetUser(int id)
    {
        return GetRequest(id);
    }

    private async UniTask<string> GetRequest(int id)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/{id}");
        await request.SendWebRequest();
        
        var result = string.Empty;

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("User data: " + request.downloadHandler.text);

            // Пример десериализации:
            UserData user = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
            result = $"Name: {user.name}, Age: {user.age}";
            Debug.Log(result);
        }
        else
        {
            Debug.LogError("GET Error: " + request.error);
        }
        
        return result;
    }
}

