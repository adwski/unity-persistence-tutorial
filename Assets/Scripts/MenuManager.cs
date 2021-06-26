using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    string filename;
    const int maxScores = 8;
    int currentScore;

    List<Score> scores;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        filename = Application.persistentDataPath + "/save.dat";
        Debug.Log("Save file: " + filename);
        LoadScore();
    }

    public List<Score> GetHiScores()
    {
        return scores;
    }

    public void SetCurrentScore(int score)
    {
        currentScore = score;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public bool IsHiScore()
    {
        if (scores.Count < maxScores)
        {
            return true;
        }
        foreach (Score sc in scores)
        {
            if (currentScore > sc.score)
            {
                return true;
            }
        }
        return false;
    }

    public void SaveHiScore(string name)
    {
        if (!IsHiScore())
        {
            return;
        }
        AddScore(currentScore, name);
    }

    public Score GetBestScore()
    {
        scores.Sort((a, b) => b.score.CompareTo(a.score));
        return scores[0];
    }

    void AddScore(int score, string name)
    {
        if (scores == null)
        {
            scores = new List<Score>();
        }

        scores.Add(new Score
        {
            score = score,
            name = name
        });
        scores.Sort((a, b) => b.score.CompareTo(a.score));

        if (scores.Count > maxScores)
        {
            scores.RemoveAt(scores.Count - 1);
            Debug.Log("Removing lowest hi score");
        }

        SaveScores(scores);
    }

    void SaveScores(List<Score> scores)
    {
        FileStream fs = new FileStream(filename, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, scores);
        fs.Close();
    }

    public void LoadScore()
    {
        if (File.Exists(filename))
        {
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                scores = (List<Score>)bformatter.Deserialize(stream);
            }
        } else if (scores == null)
        {
            scores = new List<Score>();
        }

        Debug.Log("Scores count: " + scores.Count);
    }
}

[System.Serializable]
public class Score
{
    public string name;
    public int score;
}
