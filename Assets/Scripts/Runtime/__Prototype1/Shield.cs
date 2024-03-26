using UnityEngine;
using UnityEngine.Events;

namespace TNNL.Prototype_1
{
    public class Shield : MonoBehaviour
    {
        public static UnityAction<float> ShieldChanged;
        SphereCollider sphereCollider;
        Player player;
        float shieldStartingScale;
        float colliderStartingRadius;
        [SerializeField] StatusText statusText;
        void Awake()
        {
            player = GetComponentInParent<Player>();
            sphereCollider = GetComponent<SphereCollider>();
            shieldStartingScale = transform.localScale.x;
            colliderStartingRadius = sphereCollider.radius;
        }
        private void OnTriggerEnter(Collider other)
        {
            LevelCube levelCube = other.GetComponentInParent<LevelCube>();

            if (levelCube == null) return;

            if (levelCube.Type == LevelCube.CubeType.DEFAULT)
            {
                DamageShield(levelCube.Damage);
            }
            else if (levelCube.Type == LevelCube.CubeType.MINE)
            {
                DamageShield(levelCube.Damage);
                statusText.MineHit();
            }
            else if (levelCube.Type == LevelCube.CubeType.SHIELD)
            {
                HealShield(levelCube.Damage);
                statusText.ShieldCollected();
            }

            levelCube.Container.SetActive(false);
        }

        public void DamageShield(float amount)
        {
            AdjustShield(amount);

            if (transform.localScale.x <= 1f)
            {
                // Death
                player.IsDead = true;
                sphereCollider.enabled = false;
            }
        }

        public void HealShield(float amount)
        {
            AdjustShield(amount);
        }

        private void AdjustShield(float amount)
        {
            transform.localScale -= new Vector3(amount, amount, 0f);
            sphereCollider.radius = colliderStartingRadius * transform.localScale.x / shieldStartingScale;

            ShieldChanged.Invoke((transform.localScale.x - 1f) / (shieldStartingScale - 1f));
        }


        public void ResetShield()
        {
            sphereCollider.radius = colliderStartingRadius;
            transform.localScale = new Vector3(shieldStartingScale, shieldStartingScale, transform.localScale.z);
            sphereCollider.enabled = true;
        }

        public void SetShieldHealth(float totalHealth)
        {
            // the shield health should be tracked separately from the transform size.
            // the transform is just a display mechanism for the underlying data

            ShieldChanged.Invoke((transform.localScale.x - 1f) / (shieldStartingScale - 1f));
        }

    }
}