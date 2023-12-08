using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Patterns.Singleton
{
    public abstract class Singleton<T> : RyoMonoBehaviour where T : RyoMonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get 
            { 
                if (Singleton<T>.instance == null)
                {
                    instance = GameObject.FindObjectOfType<T>();

                    if (Singleton<T>.instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        Singleton<T>.instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString();
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return instance; 
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (Singleton<T>.instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }


    }

}
