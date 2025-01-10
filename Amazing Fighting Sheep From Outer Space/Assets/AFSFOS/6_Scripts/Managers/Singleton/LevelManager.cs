using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    LaunchGame,
    Hub_Gravity,
    FinishGame,
    Asteroide,
    Entonnoire
}

public class LevelManager : MonoBehaviour
{
    public Scenes scenes;

    public void LoadScene(Scenes scenes)
    {
        SceneManager.LoadScene(scenes.ToString());
    }

    public void LoadRandomScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Scenes[] scenesArray = (Scenes[])System.Enum.GetValues(typeof(Scenes));
        List<Scenes> validScenes = new List<Scenes>();
        foreach (Scenes scene in scenesArray)
        {
            if (scene.ToString() != currentSceneName)
            {
                validScenes.Add(scene);
            }
        }
        int rd = Random.Range(3, validScenes.Count);
        Scenes randomScene = validScenes[rd];
        SceneManager.LoadScene(randomScene.ToString());
    }
        

    public IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(2);

        Application.Quit();
    }
}
