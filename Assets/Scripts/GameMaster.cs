using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }
    [SerializeField]
    private int startingMoney;
    public static int Money;
    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }
    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public float spawnFreeze = 0.5f;
    public GameObject spawnPrefab;
    public string respawnCoundownSound = "Respawn Countdown";
    public string spawnSound = "Spawn";
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject upgradeMenu;
    [SerializeField]
    private WaveSpawner waveSpawner;
    public delegate void UpgradeMenuCallBack(bool active);
    public UpgradeMenuCallBack onToggleUpgradeMenu;
    private AudioManager audioManager;
    [SerializeField]
    public string levelBGM = "LevelBGM";
    public string gameOverSound = "Game Over";
    void Start()
    {
        _remainingLives = maxLives;
        Money = startingMoney;
        StartCoroutine(_StartMusic(levelBGM));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }
    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }
    public IEnumerator _RespawnPlayer()
    {
        audioManager.PlaySound(respawnCoundownSound);
        yield return new WaitForSeconds(spawnDelay);
        Rigidbody2D playerRb = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Rigidbody2D>();
        playerRb.Sleep();
        Collider2D coll = playerRb.gameObject.GetComponent<Collider2D>();
        coll.enabled = false;
        audioManager.PlaySound(spawnSound);
        GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(clone, 3f);
        yield return new WaitForSeconds(spawnFreeze);
        playerRb.WakeUp();
        coll.enabled = true;
    }
    public IEnumerator _StartMusic(string sound)
    {
        yield return new WaitForSeconds(0.1f);
        audioManager = AudioManager.instance;
        audioManager.PlaySound(sound);
    }
    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        _remainingLives--;
        if (_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }

    }
    void PlayDeathSound(Player player)
    {
        audioManager.PlaySound(player.deathSound);
    }
    public void EndGame()
    {
        audioManager.PlaySound(gameOverSound);
        gameOverUI.SetActive(true);
    }
    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }
    public void _KillEnemy(Enemy _enemy)
    {
        audioManager.PlaySound(_enemy.deathSoundName);
        Money += _enemy.moneyDrop;
        audioManager.PlaySound("Money");
        GameObject _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity).gameObject;
        Destroy(_clone, 5f);
        Destroy(_enemy.gameObject);
    }
}
