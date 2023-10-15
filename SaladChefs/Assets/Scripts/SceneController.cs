using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Animator fadeAnimator;
    public bool autoLoadNextScene;
    public float autoLoadDelay;

    float fadeAnimationDuration = 1;

    void Start()
    {
        if (autoLoadNextScene)
        {
            StartCoroutine(LoadNextSceneWithDelay(autoLoadDelay));
        }
    }

    public void LoadNextScene(float delay)
    {
        StartCoroutine(LoadNextSceneWithDelay(delay));
    }

    IEnumerator LoadNextSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(autoLoadDelay);
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeAnimationDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
