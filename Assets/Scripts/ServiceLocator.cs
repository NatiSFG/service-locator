using System;
using UnityEngine;

/// <summary>
/// Represents an object that has a list of all major game systems in the running app (in playmode only).
/// </summary>
[DefaultExecutionOrder(-10000)]
public class ServiceLocator : MonoBehaviour {
    private static ServiceLocator instance;
    public static ServiceLocator Instance {
        get {
            if (instance == null) {
                instance = new GameObject("ServiceLocator").AddComponent<ServiceLocator>();
            }
            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            GameObject.DestroyImmediate(gameObject);
            Debug.LogError("Duplicate " + nameof(ServiceLocator) + " was found and automatically deleted!");
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private readonly System.Collections.Generic.Dictionary<Type, MonoBehaviour> services =
        new System.Collections.Generic.Dictionary<Type, MonoBehaviour>();

    public void RegisterService<T>(T service) where T : MonoBehaviour {
        Type type = typeof(T);
        if (!services.ContainsKey(type))
            services.Add(type, service);
        else Debug.LogWarning("Service of type " + type + " is already registered.");
    }

    public T GetService<T>() {
        Type type = typeof(T);
        if (services.TryGetValue(type, out MonoBehaviour service))
            return service.GetComponentInChildren<T>();
        else {
            Debug.LogWarning("Service of type " + type + " not found.");
            return default(T);
        }
    }
}
