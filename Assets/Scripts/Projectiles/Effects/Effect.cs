using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles.Effects { 

    public abstract class Effect : MonoBehaviour
    {
        public float eventRate = 1;
        public int damage = 2;
        [Tooltip("What Visual Effect to Spawn as a child to the thing we hit")]
        public GameObject visualEffectPrefab;
        [HideInInspector] public Transform hitObject;

        private float destroyTimer;

        public ContactPoint hitContact;

        protected virtual void Start()
        {
            GameObject clone = Instantiate(visualEffectPrefab, hitObject.transform);
            clone.transform.position = transform.position;
            clone.transform.rotation = transform.rotation;
        }

        protected virtual void Update()
        {
            destroyTimer += Time.deltaTime;
            if (destroyTimer >= 1/eventRate)
            {
                RunEffect();
                destroyTimer = 0;
            }
        }

        public abstract void RunEffect();
    }
}
