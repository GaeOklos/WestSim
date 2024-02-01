using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SC_PauseMenu : MonoBehaviour
{
		private bool _isPause = false;
		[SerializeField]
    	private GameObject PauseMenuUI;
		[SerializeField]
		private Button buttonPause;
		// [SerializeField]
		// private Button buttonSpeed;
		private bool _isDead = false;
		private float _speedOfLevel = 1f;
		private string sceneName;
		// [SerializeField] private TextMeshProUGUI _textSpeedOfLevel;

	    [SerializeField]
    	private GameObject[] BouttonAnimation;

		private void Start()
		{
			for (int i = 0; i < BouttonAnimation.Length; i++)
				BouttonAnimation[i].GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
		}

		public void Pause()
		{
			if (_isDead == false) {
				PauseMenuUI.SetActive(true);
				buttonPause.gameObject.SetActive(false);
				// buttonSpeed.gameObject.SetActive(false);
				_isPause = true;
				Time.timeScale = 0;
			}
		}
		public void Resume()
		{
			if (_isDead == false) {
           		// _textSpeedOfLevel.SetText("x1");
				PauseMenuUI.SetActive(false);
				buttonPause.gameObject.SetActive(true);
				// buttonSpeed.gameObject.SetActive(true);
				_isPause = false;
				Time.timeScale = 1;
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

		public void ChangeSpeedOfLevel()
		{
			
			if (_speedOfLevel < 8)
				_speedOfLevel *= 2;
			else
				_speedOfLevel = 0.5f;
			Time.timeScale = _speedOfLevel;
            // _textSpeedOfLevel.SetText("x" + _speedOfLevel);
		}

		private void Update()
		{
			if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && _isDead == false)
			{
				if (_isPause) {
					Resume();
				}
				else {
					Pause();
				}
			}
		}
}
