using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton instance;

    public HostGameManager GameManager { get; private set; }

    public static HostSingleton Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType<HostSingleton>();

            if (instance == null)
            {
                Debug.LogError("No HostSnigleton in the scene");
                return null;
            }

            return instance;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        GameManager = new HostGameManager();
    }
}
