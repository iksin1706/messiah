using UnityEngine;

namespace Assets.scripts
{
    class CharactersContainer : MonoBehaviour
    {
        public static CharactersContainer instance;

        void Awake()
        {

            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
           // DontDestroyOnLoad(gameObject);
        }
    }
}
