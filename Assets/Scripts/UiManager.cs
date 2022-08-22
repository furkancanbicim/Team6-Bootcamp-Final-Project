using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameData data;
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject maxCapacityPanel;
    [SerializeField] private Button meleeButton;
    [SerializeField] private Button rangedButton;
    [SerializeField] private Button fightButton;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject startButton;
    private void Start()
    {
        ChangeButtonStatus(false);
        moneyText.text = data.money.ToString();
    }
    private void OnEnable()
    {
        EventManager.isGridFull += ChangeButtonStatus;
        EventManager.loseGame += ShowLosePanel;
        EventManager.winGame += ShowWinPanel;
        EventManager.characterDeath += UpdateMoney;
    }
    private void OnDisable()
    {
        EventManager.isGridFull -= ChangeButtonStatus;
        EventManager.loseGame -= ShowLosePanel;
        EventManager.winGame -= ShowWinPanel;
        EventManager.characterDeath -= UpdateMoney;
    }

    void ChangeButtonStatus(bool isFull = false)
    {
        if (data.money < 50 || isFull)
        {
            rangedButton.interactable = false;
            meleeButton.interactable = false;
            if (isFull)
            {
                maxCapacityPanel.SetActive(true);

                Invoke("SetDeActivePanel", 1f);
            }

        }
        else
        {
            rangedButton.interactable = true;
            meleeButton.interactable = true;
        }
        moneyText.text = data.money.ToString();
    }
    void UpdateMoney()
    {
        moneyText.text = data.money.ToString();
    }
    void SetDeActivePanel()
    {
        maxCapacityPanel.SetActive(false);
    }
    void ShowWinPanel()
    {
        data.currentLevel++;
        winPanel.SetActive(true);
    }
    void ShowLosePanel()
    {
        losePanel.SetActive(true);

    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        data.enemies.Clear();
        data.players.Clear();
    }
   public void LoadMergeSceneAfterWin()
    {
       
        LoadMergeScene();
    }
   public void LoadMergeScene()
    {
        data.enemies.Clear();
        data.players.Clear();
        SceneManager.LoadScene("Level 1");
    }
    public void BuyMelee()
    {
        if(EventManager.buyCharacter.Invoke(CharacterType.Melee)&&data.money>50)
        {
            data.money -= 50;
        }
        else
        {
            ChangeButtonStatus();
        }
        
    }
    public void BuyRanged()
    {
        if (EventManager.buyCharacter.Invoke(CharacterType.Ranged) && data.money > 50)
        {
            data.money -= 50;
        }
        else
        {
            ChangeButtonStatus();
        }

    }
    public void StartFight()
    {
        startButton.SetActive(false);
        EventManager.startGame?.Invoke();
    }
    public void GoFightScene()
    {
        EventManager.goFightScene?.Invoke();
        fightButton.interactable = false;
    }
}
