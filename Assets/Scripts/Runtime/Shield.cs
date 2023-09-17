using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
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
        LevelCube levelCube = other.GetComponent<LevelCube>();

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

        other.gameObject.SetActive(false);
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
    }


    public void ResetShield()
    {
        sphereCollider.radius = colliderStartingRadius;
        transform.localScale = new Vector3(shieldStartingScale, shieldStartingScale, transform.localScale.z);
        sphereCollider.enabled = true;
    }

}