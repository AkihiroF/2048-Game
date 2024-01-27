using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class FinishView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup finishPanel;
        [SerializeField] private Button restartButton;

        private void Awake()
        {
            EnableFinishPanel(false);
            restartButton.onClick.AddListener(
                ()=> SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        }

        public void EnableFinishPanel(bool isEnable = true)
        {
            finishPanel.alpha = isEnable ? 1:0;
            finishPanel.interactable = isEnable;
            finishPanel.blocksRaycasts = isEnable;
        }
        
    }
}