using System;
using Sirenix.OdinInspector;
using TNNL.Collidables;
using UnityEngine;

namespace TNNL.Player
{
    public class ShieldView : MonoBehaviour
    {
        // events
        public Action<AbstractCollidable> ShieldCollision;
        public bool DoUpdate;
        private ShieldMVC mvc;
        [SerializeField][ReadOnly] private ShieldModel model;

        // member vars
        public float MinScale; // the scale of the shield game object when at 0% health. note, this should match the scale of the ship
        public float MaxScale; // the scale of the shield game object when at 100% health
        public SphereCollider sphereCollider; //
        private float colliderStartingRadius = .5f; //
        private float colliderMinRadius;

        void Start()
        {
            colliderMinRadius = MinScale / MaxScale * colliderStartingRadius;
        }

        public void SetMVC(ShieldMVC shieldMVC)
        {
            mvc = shieldMVC;
            model = mvc.Model;
        }

        void Update()
        {
            if (model != null && DoUpdate)
            {
                UpdateAppearance(model.PercentHealth);
            }
        }

        void FixedUpdate()
        {
            mvc.Controller?.FixedUpdate();
        }

        private void UpdateAppearance(float percentHealth)
        {
            // set the visual scale of the orb and the scale of the collider
            float newScale = MinScale + (MaxScale - MinScale) * percentHealth;
            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            AbstractCollidable collidable = other.GetComponentInParent<AbstractCollidable>();

            if (collidable != null)
            {
                mvc.Controller?.HandleCollision(collidable);
            }
        }
    }
}