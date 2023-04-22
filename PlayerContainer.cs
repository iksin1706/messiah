using UnityEngine;

namespace Assets.scripts
{
    class PlayerContainer : MonoBehaviour
    {
        public static PlayerContainer instance;

        void Awake()
        {

            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}
