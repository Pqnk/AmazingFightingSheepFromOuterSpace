using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class LaunchGame : MonoBehaviour
{
    [Header("Timer Between Play")]
    public int timeWait;
    public int timeStop;

    [Header("Screen GameObject")]
    public GameObject first;
    public GameObject second;
    public GameObject third;
    public GameObject langue;

    private Animator firstAnim;
    private Animator secondAnim;
    private Animator thirdAnim;

    public GameObject superManager;

    void Start()
    {
        GameObject superManagerObject =  Instantiate(superManager);
        superManagerObject.AddComponent<PersistOnLoad>();

        first.SetActive(false);
        second.SetActive(false);
        third.SetActive(false);

        firstAnim = first.GetComponent<Animator>();
        secondAnim = second.GetComponent<Animator>();
        thirdAnim = third.GetComponent<Animator>();

        firstAnim.SetBool("Next", false);
        secondAnim.SetBool("Next", false);
        thirdAnim.SetBool("Next", false);

        SuperManager.instance.langageManager.ChangeLanguage("En");
        StartCoroutine("ThirdScreen");
        langue.SetActive(false);
        Cursor.visible = false;

    }

    public void Action_LanguageSelected(string language)
    {
        SuperManager.instance.langageManager.ChangeLanguage(language);
    }

    public void Action_ValidateLanguageSelected()
    {
        StartCoroutine("ThirdScreen");
        langue.SetActive(false);
        Cursor.visible = false;
    }

    public IEnumerator FirstScreen()
    {        
        first.SetActive(true);
        firstAnim.Play("AnimStart");

        yield return new WaitForSeconds(timeWait);                // Attent ( temps entre les anims ) en seconde

        firstAnim.SetBool("Next", true);

        yield return new WaitForSeconds(timeStop);                // Temps avant le prochain screen en seconde

        first.SetActive(false);
        StartCoroutine("SecondScreen");

        firstAnim.SetBool("Next", false);
    }

    public IEnumerator SecondScreen()
    {
        second.SetActive(true);
        secondAnim.Play("AnimLaunch");

        yield return new WaitForSeconds(timeWait);                // Attent ( temps entre les anims ) en seconde

        secondAnim.SetBool("Next", true);

        yield return new WaitForSeconds(timeStop);                // Temps avant le prochain screen en seconde

        second.SetActive(false);
        StartCoroutine("ThirdScreen");

        secondAnim.SetBool("Next", false);
    }

    public IEnumerator ThirdScreen()
    {
        third.SetActive(true);
        thirdAnim.Play("AnimLaunch");

        yield return new WaitForSeconds(3);                // Attent ( temps entre les anims ) en seconde

        thirdAnim.SetBool("Next", true);

        yield return new WaitForSeconds(4);                // Temps avant le prochain screen en seconde

        third.SetActive(false);

        thirdAnim.SetBool("Next", false);

        ChangeScene();
    }

    public void ChangeScene()
    {
        SuperManager.instance.levelManager.LoadScene(Scenes.Hub_Gravity);
    }
}
