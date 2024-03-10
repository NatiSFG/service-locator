using System;
using System.Collections.Generic;
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
                ServiceLocator prefab = Resources.Load<ServiceLocator>("Service Locator");
                Component.Instantiate(prefab);
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

    public T GetSystem<T>() {
        T service = GetComponentInChildren<T>();

        if (service == null) 
            Debug.LogWarning("Service of type " + typeof(T).Name + " not found.");
        return service;
    }
}
