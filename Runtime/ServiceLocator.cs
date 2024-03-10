using System;
using System.Collections.Generic;
using UnityEngine;

namespace SFG.ServiceLocator {
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

        private List<MonoBehaviour> runtimeSystems = new();

        private void Awake() {
            if (instance != null && instance != this) {
                GameObject.DestroyImmediate(gameObject);
                Debug.LogError("Duplicate " + nameof(ServiceLocator) + " was found and automatically deleted!");
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public T GetSystem<T>() where T : class {
            T system = null;
            foreach (MonoBehaviour other in runtimeSystems)
                if (other is T specificType)
                    system = specificType;

            if (system == null)
                system = GetComponentInChildren<T>();

            if (system == null)
                Debug.LogWarning("Service of type " + typeof(T).Name + " not found.");
            return system;
        }

        public bool AddRuntimeSystem<T>(T system) where T : MonoBehaviour {
            if (runtimeSystems.Contains(system))
                return false;
            runtimeSystems.Add(system);
            return true;
        }

        public bool RemoveRuntimeSystem<T>(T system) where T : MonoBehaviour {
            return runtimeSystems.Remove(system);
        }
    }
}
