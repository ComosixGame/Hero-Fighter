using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyCustomAttribute;

public class SkillCard : MonoBehaviour
{
    public int skillId;
    public int heroId;
    public GameObject windowSkillTreeDetail;
    public Animator UIMenu;
    public Image sprite;
    private Button button;
    private GameManager gameManager;
    [SerializeField] private SkillTreeManager skillTreeManager;

    private void Awake()
    {
        button = GetComponent<Button>();
        gameManager = GameManager.Instance;
        UIMenu = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        
    }

    //Handle Buy Skill
    private void ClickBuy()
    {
        Debug.Log("ABC");
    }
}
