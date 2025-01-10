using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private Dictionary<string, PlayerData> players = new Dictionary<string, PlayerData>();
    private GameObject cam;
    public int maxScore = 7;


    public void SaveGame()
    {
        PlayerPrefs.SetString(GetPlayer(allPlayersObject[0].name).Name, GetPlayer(allPlayersObject[0].name).Score.ToString());
        PlayerPrefs.SetString(GetPlayer(allPlayersObject[1].name).Name, GetPlayer(allPlayersObject[1].name).Score.ToString());
        Debug.Log("saved");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    #region Camera //////////////////////////////////////////////////////////////////////////////
    public GameObject GetCamera()
    {        
        return cam;
    }
    public void SetCamera(GameObject camera)
    {
        cam = camera;
    }
    #endregion

    #region Player Save Zone //////////////////////////////////////////////////////////////////////////////

    public List<GameObject> allPlayersObject = new List<GameObject>();

    public PlayerData GetPlayer(string playerName)
    {
        if (players.TryGetValue(playerName, out PlayerData playerData))
        {
            return playerData;
        }
        return null;
    }

    public void RestartScore()
    {
        foreach (var item in allPlayersObject)
        {
            PlayerData player = GetPlayer(item.name);
            player.Score = 0;
        }
    }

    public GameObject GetPlayerObject(PlayerData playerName)
    {
        foreach (var player in allPlayersObject) 
        {
            if (player.gameObject.name == playerName.Name)
            {
                return player;
            }
        }

        return null;
    }

    public GameObject GetOtherPlayerObject(PlayerData playerName)
    {
        foreach (var player in allPlayersObject)
        {
            if (player.gameObject.name != playerName.Name)
            {
                return player;
            }
        }

        return null;
    }

    public GameObject[] GetPlayers()
    {
        return allPlayersObject.ToArray();
    }

    public List<int> GetPlayersScores()
    {
        List<int> result = new List<int>();

        foreach (var player in allPlayersObject) 
        {
            PlayerData playerData = GetPlayer(player.name);
            result.Add(playerData.Score);
        }

        return result;
    }

    public void AddPlayer(string playerName, int score)
    {
        if (!players.ContainsKey(playerName))
        {           
            players[playerName] = new PlayerData(playerName, score);
        }
        else
        {
            players[playerName].Score += score;
        }
    }
    #endregion
}

[System.Serializable]
public class PlayerData
{
    public string Name { get; private set; }
    public int Score { get; set; }

    public PlayerData(string name, int score)
    {
        Name = name;
        Score = score;
    }
}
