using UnityEngine;
using UnityStandardAssets._2D;
using System.Collections;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    public int fallBoundry = -20;
    [SerializeField]
    private StatusIndicator statusIndicator;
    public string damageSound = "Player Damage";
    public string deathSound = "Player Death";
    private AudioManager audioManager;
    private PlayerStats stats;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        stats = PlayerStats.instance;
        stats.curHealth = stats.maxHealth;
        if (statusIndicator == null)
        {
            Debug.Log("No status indicator for Player");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;
        InvokeRepeating("RegenHealth", 1f / stats.healthRegenRate, 1f / stats.healthRegenRate);
        audioManager = AudioManager.instance;
    }
    void Update()
    {
        if (transform.position.y <= fallBoundry)
        {
            DamagePlayer(999999);
        }
    }
    void RegenHealth()
    {
        stats.curHealth++;
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }
    void OnUpgradeMenuToggle(bool active)
    {
        GetComponent<Platformer2DUserControl>().enabled = !active;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if (_weapon != null)
        {
            _weapon.enabled = !active;
        }
    }
    public void DamagePlayer(int damage)
    {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0)
        {
            audioManager.PlaySound(deathSound);
            GameMaster.KillPlayer(this);
        }
        else
        {
            audioManager.PlaySound(damageSound);
        }
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }
    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
