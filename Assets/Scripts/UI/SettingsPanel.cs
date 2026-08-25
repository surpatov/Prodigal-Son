using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject graphicsPanel;

    [SerializeField] private RectTransform panelsPoint;

    public void ShowAudioPanel()
    {
        audioPanel.gameObject.SetActive(true);
        audioPanel.transform.position = panelsPoint.position;
    }
    public void ShowControlsPanel()
    {
        controlsPanel.gameObject.SetActive(true);
        controlsPanel.transform.position = panelsPoint.position;
    }
    public void ShowGraphicsPanel()
    {
        graphicsPanel.gameObject.SetActive(true);
        graphicsPanel.transform.position = panelsPoint.position;
    }
}
