using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] GameObject itemUI;
    [SerializeField] float speedRotation;

    [SerializeField] MeshRenderer[] models;
    [SerializeField] public int currentItem;

    [SerializeField] GameObject settingsObject;
    [SerializeField] GameObject mainObject;

    void Update()
    {
        itemUI.transform.Rotate(0f, speedRotation * Time.deltaTime, 0f);
        for (int i = 0; i < models.Length; i++)
        {
            if (i == currentItem)
                models[i].enabled = true;
            else models[i].enabled = false;
        }
    }
    public void StartGame()
    {
        Debug.Log("GAME STARTED");
        SceneManager.LoadScene("Demo");
    }
    public void SettingsButton()
    {
        settingsObject.SetActive(true);
        mainObject.SetActive(false);
    }
    public void ContentButton()
    {
        
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void ReturnButton()
    {
        settingsObject.SetActive(false);
        mainObject.SetActive(true);
    }
}