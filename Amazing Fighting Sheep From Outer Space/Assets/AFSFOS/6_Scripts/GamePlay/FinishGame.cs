using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    [Header("Texte Player 1")]
    public TMP_Text name_P1;
    public TMP_Text score_P1;

    [Header("Texte Player 2")]
    public TMP_Text name_P2;
    public TMP_Text score_P2;

    public TMP_Text textVictoire;

    // Start is called before the first frame update
    void Start()
    {
        name_P1.text = SuperManager.instance.saveManager.allPlayersObject[0].name;
        score_P1.text = SuperManager.instance.saveManager.GetPlayer(name_P1.text).Score.ToString();
        name_P2.text = SuperManager.instance.saveManager.allPlayersObject[1].name;
        score_P2.text = SuperManager.instance.saveManager.GetPlayer(name_P2.text).Score.ToString();

        if (SuperManager.instance.saveManager.GetPlayer(name_P1.text).Score > SuperManager.instance.saveManager.GetPlayer(name_P2.text).Score)
        {
            textVictoire.text = name_P1.text + " win !";
        }
        else 
        {
            textVictoire.text = name_P2.text + " win !";
        }

        StartCoroutine(nextScene());
    }

    IEnumerator nextScene()
    {
        yield return new WaitForSeconds(4);

        SuperManager.instance.levelManager.LoadScene(Scenes.Hub_Gravity);
    }
}
