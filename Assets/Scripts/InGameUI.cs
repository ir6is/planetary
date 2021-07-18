using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Button m_pauseButton;
    [SerializeField] private Button m_restartButton;
    [SerializeField] private Button m_saveButton;
    [SerializeField] private Button m_loadButton;

    [SerializeField] private MainService m_mainService;

    private void Awake()
    {
        m_pauseButton.onClick.AddListener(() =>
        {
            m_mainService.IsPause = !m_mainService.IsPause;
            m_pauseButton.GetComponentInChildren<Text>().text = m_mainService.IsPause ? "Pause" : "Resume";
        });

        m_restartButton.onClick.AddListener(() => { m_mainService.Restart(); });

        m_saveButton.onClick.AddListener(() => { m_mainService.Save(); });

        m_loadButton.onClick.AddListener(() => { m_mainService.Load(); });
    }
}