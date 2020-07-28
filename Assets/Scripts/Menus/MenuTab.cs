using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(Button))]
    public class MenuTab : MonoBehaviour
    {
        [SerializeField] private Sprite activeBtnSprite;
        [SerializeField] private Sprite inactiveBtnSprite;
        [SerializeField] private int panelSiblingIdx; // make sure the panel sibling orders dont change
        
        private GameObject panelParent;
        private GameObject menuPanel;
        
        private GameObject btnParent;
        private Button menuBtn;
        private Button[] allBtns;


        private void Start()
        {
            panelParent = GameObject.FindGameObjectWithTag("Menu Panel Parent");
            menuPanel = panelParent.transform.GetChild(panelSiblingIdx).gameObject;
            
            btnParent = transform.parent.gameObject;
            menuBtn = GetComponent<Button>();
            allBtns = btnParent.GetComponentsInChildren<Button>();
            
            menuBtn.onClick.AddListener(ChoosePanel);
            menuBtn.onClick.AddListener(ChooseBtn);
        }

        private void ChoosePanel()
        {
            foreach (Transform child in panelParent.transform)
            {
                child.gameObject.SetActive(false);
            }

            menuPanel.SetActive(true);
        }

        private void ChooseBtn()
        {
           foreach (Button btn in allBtns)
           {
               btn.image.sprite = inactiveBtnSprite;
           }
            
           menuBtn.gameObject.SetActive(true);
           menuBtn.transform.SetSiblingIndex(btnParent.transform.childCount - 1);
           menuBtn.image.sprite = activeBtnSprite;
        }
    }
}