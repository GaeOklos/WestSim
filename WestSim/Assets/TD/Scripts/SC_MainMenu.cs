using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _panelInRef = null;
    private Animator _animatorPanel = null;


    public void LoadScene(string sceneName)
    {
        // Debug.Log("Change to level "+ sceneName);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _panelInRef.SetActive(true);
        _animatorPanel = _panelInRef.GetComponent<Animator>();
        StartCoroutine(loadNextScene(sceneName));
    }
    public IEnumerator loadNextScene(string sceneName)
    {
        // Lance l'animation
        _animatorPanel.SetTrigger("TriggerFadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
