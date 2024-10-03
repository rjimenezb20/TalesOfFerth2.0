using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{

    private SelectionController SC;
    private bool optionsMenuOpen = false;

    void Start()
    {
        SC = FindObjectOfType<SelectionController>();
    }

    //WatchTower
    public void OutOfTowerEvent()
    {
        SC.selectedBuilding.GetComponent<WatchTower>().UnitOut();
    }

    public void ChangeScene(string mapName)
    {
        SceneManager.LoadScene(mapName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToggleOptionsMenu(Animation anim)
    {
        if (!optionsMenuOpen)
        {
            anim["OptionsMenu"].speed = 1;
            optionsMenuOpen = true;
        }
        else
        {
            anim["OptionsMenu"].speed = -1;
            anim["OptionsMenu"].time = anim["OptionsMenu"].length;
            optionsMenuOpen = false;
        }
        anim.Play();
    }

    public void ToggleOptionsMenu1(Animation anim)
    {
        if (!optionsMenuOpen)
        {
            anim["OptionsMenu1"].speed = 1;
            optionsMenuOpen = true;
        }
        else
        {
            anim["OptionsMenu1"].speed = -1;
            anim["OptionsMenu1"].time = anim["OptionsMenu1"].length;
            optionsMenuOpen = false;
        }
        anim.Play();
    }

    public void DestroyBuilding()
    {
        if (SC.selectedBuilding != null)
            SC.selectedBuilding.Destroy();
    }
}