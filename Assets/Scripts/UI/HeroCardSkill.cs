using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCardSkill : MonoBehaviour
{
    public int id;
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button button;
    [SerializeField] private GameObject startGo;
    //
    public GameObject windowSkill;
    public Animator UIMenu;
    public SkillTreeUI skillTreeUI;


    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }

    private void OnEnable() {
        button.onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        windowSkill.SetActive(true);
        skillTreeUI.RenderSkillCard(id);
    }

    public void SetDataCard(Sprite thumbnailSprite, string title, int start)
    {
        thumbnail.sprite = thumbnailSprite;
        this.title.text = title;
        for(int i = 0; i < start; i++)
        {
            startGo.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
