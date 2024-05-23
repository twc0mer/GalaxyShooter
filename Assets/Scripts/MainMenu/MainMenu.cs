using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene("SP_L1");
    }

    public void LoadCoopGame()
    {
        SceneManager.LoadScene("CP_L1");
    }
}
