using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject ShockwavePrefab;
    public float shockwaveMinDistance;
    public GameObject arms;
    public MinionSpawner minionSpawner;
    public HUDController hud;
    public GameObject player;
    public BossAnimations bossAnimations;
    public Transform shockwaveSpawnPoint;
    float currentHealth = 2;
    float lowHealthThreshold = 1;
    bool canShockwave = true;
    bool isDamaged = false;

    AudioSource audioSource;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        CancelInvoke();
        canShockwave = true;
    }

    void Update()
    {
        if (isDamaged) return;
        if (IsPlayerCloseEnough())
        {
            StartShockwave();
        }
    }

    bool IsPlayerCloseEnough()
    {
        return (player.transform.position - transform.position).magnitude <= shockwaveMinDistance;
    }


    void StartShockwave()
    {
        if (!canShockwave) return;
        if (isDamaged) return;
        canShockwave = false;
        bossAnimations.StartShockwave();
        Invoke("ResetShockwave", 10f);
    }

    public void DoShockwave()
    {
        if (isDamaged) return;
        audioSource.Play();
        CreateShockwave(new Vector3(0, 0, -5));
        CreateShockwave(new Vector3(0, 0, -90));
        CreateShockwave(new Vector3(0, 0, -135));
        CreateShockwave(new Vector3(0, 0, -45));
        CreateShockwave(new Vector3(0, 0, -175));

    }
    void CreateShockwave(Vector3 shockwaveRot)
    {
        GameObject newObj = Instantiate(ShockwavePrefab, shockwaveSpawnPoint.position, Quaternion.Euler(shockwaveRot));
        newObj.transform.parent = transform;
    }

    public void GotShot()
    {
        currentHealth -= 1;
        if (currentHealth <= lowHealthThreshold)
        {
            ChangeToDamaged();
        }
        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    void ChangeToDamaged()
    {
        arms.SetActive(false);
        isDamaged = true;
    }

    void Dead()
    {
        minionSpawner.Cleanup();
        hud.ShowWinText();
        Destroy(gameObject);
    }

    void ResetShockwave()
    {
        canShockwave = true;
    }
}
