using UnityEngine;

using System.IO;

using Newtonsoft.Json;

public class DataManager
{
    const string kScoreDataFile = "/topchefs.json";

    readonly string scoreFilePath = string.Empty;

    public DataManager()
    {
        scoreFilePath = string.Concat(Application.persistentDataPath, kScoreDataFile);
    }

    public void CreateDummy()
    {
        PlayerScoreData playerScoreData = new PlayerScoreData
        {
            scores = new PlayerScore[]
            {
                new PlayerScore
                {
                        Rank = 1,
                        ChefIndex  = 0,
                        PlayerName = "Chef 1",
                        Score = 1234
                    },
                    new PlayerScore
                    {
                        Rank = 2,
                        ChefIndex = 1,
                        PlayerName = "Chef 2",
                        Score = 1100
                    },
                    new PlayerScore
                    {
                        Rank = 3,
                        ChefIndex = 0,
                        PlayerName = "Chef 1",
                        Score = 989
                    }
            }
        };

        string scoreDataString = JsonConvert.SerializeObject(playerScoreData);

        File.WriteAllText(scoreFilePath, scoreDataString);

        Debug.Log(scoreDataString);
    }

    public void ClearScoreDB()
    {
        File.Delete(scoreFilePath);
    }

    public void SaveScore(PlayerScore playerScore)
    {
        PlayerScore[] newScores = null;

        PlayerScoreData playerScoreData = GetPlayerScoreData();

        if (playerScoreData.scores != null)
        {
            int scoreboardSize = playerScoreData.scores.Length + 1 > 10 ? 10 : playerScoreData.scores.Length + 1;
            newScores = new PlayerScore[scoreboardSize];
            bool hasFoundRank = false;

            for (int i = 0; i < playerScoreData.scores.Length; i++)
            {
                if (playerScore.Score > playerScoreData.scores[i].Score)
                {
                    hasFoundRank = true;
                    playerScore.Rank = i + 1;
                    newScores[i] = playerScore;
                    for (int j = i; j < playerScoreData.scores.Length; j++)
                    {
                        newScores[j + 1] = playerScoreData.scores[j];
                        newScores[j + 1].Rank = j + 2;
                    }
                    break;
                }
                newScores[i] = playerScoreData.scores[i];
            }

            if (!hasFoundRank)
            {
                playerScore.Rank = scoreboardSize;
                newScores[playerScoreData.scores.Length] = playerScore;
            }
        }
        else
        {
            playerScore.Rank = 1;
            newScores = new PlayerScore[] { playerScore };
        }

        playerScoreData.scores = newScores;

        string scoreDataString = JsonConvert.SerializeObject(playerScoreData);

        if(!File.Exists(scoreFilePath))
        {
            FileStream fs =  File.Create(scoreFilePath, 1024, FileOptions.RandomAccess);
            fs.Close();
        }

        //Debug.Log("Saving Score...");
        File.WriteAllText(scoreFilePath, scoreDataString);
        //Debug.Log("Saved Score.");
    }

    public PlayerScoreData GetPlayerScoreData()
    {
        PlayerScoreData playerScoreData = new PlayerScoreData();

        if (File.Exists(scoreFilePath))
        {
            string scoreDataString = File.ReadAllText(scoreFilePath);
            if (!string.IsNullOrEmpty(scoreDataString))
            {
                playerScoreData = JsonConvert.DeserializeObject<PlayerScoreData>(scoreDataString);
                return playerScoreData;
            }
        }

        return playerScoreData;
    }
}
