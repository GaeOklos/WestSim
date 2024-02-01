using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    	#region Singleton
	public static GameManager _instance = null;
	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
			{
				var instances = FindObjectsOfType<GameManager>();
				if (instances == null || instances.Length == 0)
				{
					Debug.LogError("No instance of GameManager found");
					return null;
				}
				else if (instances.Length > 1)
				{
					Debug.LogError("Multiples instances of GameManager found, there MUST be only one.");
					return null;
				}
				_instance = instances[0];
			}
			return _instance;
		}
	}
	#endregion Singleton

	#region Fields


	[SerializeField]
	// private EndGameMenu _endGameMenu = null;


	[System.NonSerialized]
	private bool _isGameon = false;


	[System.NonSerialized]
	private bool _isWin = false;

	[System.NonSerialized]
	private float _timeInSeconds = 0f;

	[System.NonSerialized]
	private int _score = 0;

	private bool _isPause = false;
	private bool _isTheEnd = false;
	private bool _isDead = false;

	public Button boutonPause;
	public Button boutonPlay;
	public GameObject[] BoutonAnimation;
    public GameObject PauseMenuUI;

	#endregion Fields
	Scene m_Scene;
	string _sceneName;
  	private void Start()
    {
		// _sceneName = SceneManager.GetActiveScene().ToString();
        // // Pour que tout les boutons puissent faire leurs animations
        // for (int i = 0; i < BoutonAnimation.Length; i++)
        //     BoutonAnimation[i].GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
		// if (_sceneName != "MainMenu")
        //     StartGame();
		FadeOutCoroutine();
    }

    public IEnumerator FadeOutCoroutine()
    {
        Debug.Log("Holé");
        yield return new WaitForSeconds(1f);
    }



    public void StartGame()
    {
        if (_isGameon == false)
            _isGameon = true;
		// m_Scene = SceneManager.GetActiveScene();
		// _sceneName = m_Scene.name;
        // // Pour que tout les boutons puissent faire leurs animations
        // for (int i = 0; i < BoutonAnimation.Length; i++)
        //     BoutonAnimation[i].GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
    }




    public void Pause()
	{
		if (_isDead == false) {
			PauseMenuUI.SetActive(true);
			_isPause = true;
			_isGameon = false;
			Time.timeScale = 0;
            if (boutonPause)
                boutonPause.gameObject.SetActive(false);
			if (boutonPlay)
                boutonPlay.gameObject.SetActive(true);
		}
	}

    public void Resume()
	{
		if (_isDead == false) {
			PauseMenuUI.SetActive(false);
			_isPause = false;
			_isGameon = true;
			Time.timeScale = 1;
            if (boutonPause)
                boutonPause.gameObject.SetActive(true);
			if (boutonPlay)
                boutonPlay.gameObject.SetActive(false);
		}
	}

    public void CurLoadScene()
    {
		if (_isPause) {
			Resume();
        	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		else
        	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
		if (_isPause) {
			Resume();
        	SceneManager.LoadScene(sceneName);
		}
		else
        	SceneManager.LoadScene(sceneName);
    }

    public void AddScore(int _additionalScore)
	{
		// _score++;
        _score += _additionalScore;
		// _endGameMenu.UpdateMenu(_score);
	}

    public void CheckWinEndGame(bool isWin)
	{
		_isWin = isWin;
		if (isWin == false) {
			ActivateEndGame();
		}
        else
			ActivateEndGame();
	}

    private void Update()
	{
		if (_isGameon == true) {
			_timeInSeconds += Time.deltaTime;
		}
		if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && _isTheEnd == false && _isDead == false) {
            if (_isPause)
                Resume();
            else
                Pause();
        }
	}

    private void ActivateEndGame()
	{
		if (_sceneName == "") {
			_isTheEnd = true;
			// _isPause = true;
			// _isGameon = false;
		}
		else {
			// _endGameMenu.CheckWinEndGame(_isWin);
			_isPause = true;
			_isGameon = false;
			Time.timeScale = 0;
			// Pause();
		}
	}
}