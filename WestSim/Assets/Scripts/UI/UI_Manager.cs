using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    private string _sceneName = null;

    private void Start() {
		_sceneName = SceneManager.GetActiveScene().ToString();
        if (_sceneName != "MainMenu") {
            FadeOutCoroutine();
        }
    }

    public IEnumerator FadeOutCoroutine()
    {
        // Lance l'animation
        Debug.Log("Holé");

        // _panelOutRef.SetActive(true);
        // _animatorPanelOut.SetTrigger("TriggerFadeOut");
        yield return new WaitForSeconds(1f);
        // _panelOutRef.SetActive(false);
        // // SceneManager.LoadScene(sceneName);
    }
}
