
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public void BtnPlay()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void BtnOption()
    {
        Application.LoadLevel("OptionMenu");
    }

    public void BtnQuit_Option()
    {
        Application.LoadLevel("Menu game");
    }

    public void BtnHard()
    {
        Application.LoadLevel("Hard");
    }

    public void BtnMedium()
    {
        Application.LoadLevel("Tetris-Normal");
    }
    
    public void BtnMenu()
    {
        Application.LoadLevel("Menu game");
    }
    public void BtnGuide()
    {
        Application.LoadLevel("guide");
    }

    public void BtnThoat()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void BtnQuit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    
}
