using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject crosshair;
    [SerializeField] float panelDelay = 1.5f;
    [SerializeField] int menuSceneIndex = 0;

    int enemiesLeft;
    bool gameOver;

    void OnEnable()
    {
        Health.Died += OnEntityDied;
    }

    void OnDisable()
    {
        Health.Died -= OnEntityDied;
    }

    void Start()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void OnEntityDied(Health health)
    {
        if (gameOver) return;

        if (health.CompareTag("Player"))
        {
            gameOver = true;
            Invoke(nameof(ShowLose), panelDelay);
        }
        else if (health.CompareTag("Enemy"))
        {
            enemiesLeft--;

            if (enemiesLeft <= 0)
            {
                gameOver = true;
                Invoke(nameof(ShowWin), panelDelay);
            }
        }
    }

    void ShowWin()
    {
        ShowPanel(winPanel);
    }

    void ShowLose()
    {
        ShowPanel(losePanel);
    }

    void ShowPanel(GameObject panel)
    {
        if (crosshair != null) crosshair.SetActive(false);
        panel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void Restart()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneIndex);
    }
}