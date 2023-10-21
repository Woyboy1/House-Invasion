using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletStats : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private AudioClip impactSFX;

    [Space(10)]

    [SerializeField] private AudioClip[] bloodSFX;

    [Header("Settings")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private GameObject impactVFX;
    [SerializeField] private GameObject bloodImpactVFX;

    const string environmentLayer = "EnvironmentLayer";
    const string playerLayer = "PlayerLayer";
    const string enemyLayer = "EnemyLayer";

    private void OnTriggerEnter(Collider other)
    {
        int randSFX = Random.Range(0, bloodSFX.Length);

        if (other.CompareTag("Environment") || other.gameObject.layer == LayerMask.NameToLayer(environmentLayer))
        {
            Instantiate(impactVFX, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(impactSFX, other.transform.position);
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Enemy") || other.gameObject.layer == LayerMask.NameToLayer(enemyLayer))
        {
            other.TryGetComponent<ZombieController>(out ZombieController controller);
            controller?.TakeDamage(damageAmount);
            AudioSource.PlayClipAtPoint(bloodSFX[randSFX], other.transform.position);

            Instantiate(bloodImpactVFX, other.transform.position, other.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
