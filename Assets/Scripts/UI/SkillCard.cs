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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ClickButton()
    {
        windowSkillTreeDetail.transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(ClickBuy);
        windowSkillTreeDetail.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillTreeManager.skillTreeStates[heroId].skillStates[skillId].nameSkill;
        //Read file Save
        if (false)
        {
            //Permit Player Buy Skill
            windowSkillTreeDetail.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
            windowSkillTreeDetail.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillTreeManager.skillTreeStates[heroId].skillStates[skillId].price+"";
        }else
        {
            //Alert Player Buy Skill Perivous 
            windowSkillTreeDetail.transform.GetChild(2).GetChild(0).GetComponent<Button>().interactable = false;
            windowSkillTreeDetail.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            windowSkillTreeDetail.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Open Previous Skill";
        }

        if(!windowSkillTreeDetail.activeInHierarchy)
        {
            UIMenu.SetTrigger("OpenSkillDetail");
        }
    }

    //Handle Buy Skill
    private void ClickBuy()
    {
        Debug.Log("ABC");
    }
}
