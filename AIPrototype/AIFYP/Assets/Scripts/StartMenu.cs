using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public GameObject menu1;
    public GameObject menu2;
    bool swap = true;

    public void Level1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Adaptive Ai");
    }

    public void HowToPlay()
    {
        swap = !swap;
        menu1.SetActive(swap);
        menu2.SetActive(!swap);
    }
}
