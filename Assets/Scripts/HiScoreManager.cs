
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HiScoreManager : MonoBehaviour
{
    [SerializeField] Text yourScoreText;
    [SerializeField] Text hiScoreScoreText;
    [SerializeField] InputField nameInput;
    [SerializeField] GameObject saveScoreUI;
    [SerializeField] GameObject yourScoreUI;

    void Start()
    {
        DisplayHiScores();
        if (MenuManager.Instance.GetCurrentScore() > 0)
        {
            DisplayYourScore();
        }
    }

    void DisplayHiScores()
    {
        hiScoreScoreText.text = "";
        foreach (Score score in MenuManager.Instance.GetHiScores())
        {
            hiScoreScoreText.text += score.name + " : " + score.score + "\n";
        }
    }

    void DisplayYourScore()
    {
        yourScoreUI.SetActive(true);
        yourScoreText.text = "You: " + MenuManager.Instance.GetCurrentScore();
        if (MenuManager.Instance.IsHiScore())
        {
            Debug.Log("Activate save hi score ui");
            saveScoreUI.SetActive(true);    
        }
    }

    public void SaveHiScore()
    {
        if (nameInput.text != "")
        {
            MenuManager.Instance.SaveHiScore(nameInput.text);
            DisplayHiScores();
            saveScoreUI.SetActive(false);
            MenuManager.Instance.SetCurrentScore(0);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
