using System.Threading.Tasks;
using Firebase;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    public static FirebaseInitializer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        await dependencyTask;

        Debug.Log("Firebase ready");
    }
}

