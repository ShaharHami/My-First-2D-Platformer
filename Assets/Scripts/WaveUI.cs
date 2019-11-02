using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    WaveSpawner spawner;
    [SerializeField]
    Animator waveAnimator;
    [SerializeField]
    Text waveCountDownText;
    [SerializeField]
    Text waveCountText;
    private WaveSpawner.SpawnState previousState;
    // Start is called before the first frame update
    void Start()
    {
        if (spawner == null)
        {
            Debug.LogError("No spawner present");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("No waveAnimator present");
            this.enabled = false;
        }
        if (waveCountDownText == null)
        {
            Debug.LogError("No waveCountDownText present");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.LogError("No waveCountText present");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (spawner.State)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
        }
        previousState = spawner.State;
    }
    void UpdateCountingUI()
    {
        if (previousState != WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncomingAnim", false);
            waveAnimator.SetBool("WaveCountDownAnim", true);
        }
        waveCountDownText.text = ((int)spawner.WaveCountdown).ToString();
        
    }
    
    void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveCountDownAnim", false);
            waveAnimator.SetBool("WaveIncomingAnim", true);
        }
        waveCountText.text = spawner.NextWave.ToString();
    }
}
