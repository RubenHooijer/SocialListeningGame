﻿using UnityEngine;

namespace Oasez.Extensions.Generics.Singleton
{
    /// <summary>
    /// Generic singleton class
    /// </summary>
    /// <typeparam name="T">typeof(Class)</typeparam>
    /// <typeparam name="A">Calltype for example: interface or the same class</typeparam>
    public abstract class GenericSingleton<T, A> : MonoBehaviour where T : Component, A {

        private static T instance;
        public static A Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<T>();
                    if (instance == null) {
                        GameObject obj = new GameObject(typeof(T).ToString() + " - Singleton");
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
            } else {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        } 

    }

}