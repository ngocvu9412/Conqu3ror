using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogController : MonoBehaviour
{
    public UnityEngine.UI.Image dialogImage;
    public UnityEngine.UI.Image textboxImage1;
    public UnityEngine.UI.Image textboxImage2;
    public UnityEngine.UI.Image menuImage;
    public UnityEngine.UI.Button menuBtn;

    public KnightMovement character;

    public bool isOpened = false;

    public PointInfo enemyData;

    public Text playerName;
    public Text enemyName;

    public Text playerHealth;
    public Text enemyHealth;

    public Text playerDamage;
    public Text enemyDamage;

    public Text playerLevel;
    public Text enemyLevel;

    public Image playerAvt;
    public Image enemyAvt;

    public Text convo1;
    public Text convo2;

    public Button FightButton;

    public GameObject[] RedPoints;
    public PointInfosDatabase pointDB;

    public void UpdateDialogData()
    {
        // Cập nhật dữ liệu người chơi
        this.playerName.text = GameDataManager.Ins.GetSelectedCharacter().name;
        this.playerHealth.text = GameDataManager.Ins.GetSelectedCharacter().health.ToString();
        this.playerDamage.text = GameDataManager.Ins.GetSelectedCharacter().attack.ToString();
        this.playerLevel.text = "Level " + GameDataManager.Ins.GetSelectedCharacter().Level.ToString();
        this.playerAvt.sprite = GameDataManager.Ins.GetSelectedCharacter().Image;


        // Cập nhật đoạn hội thoại
        //this.convo1.text = conversationText1;
        //this.convo2.text = conversationText2;
    }

    void Update()
    {
        // Kiểm tra nếu nhấn chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

            // Phát hiện collider tại vị trí nhấn
            if (hit.collider != null)
            {
                Debug.Log("Raycast trúng: " + hit.collider.gameObject.name);

                // Hiển thị dialog
                if (hit.collider.gameObject.CompareTag("RedPoint"))
                {
                    int pointIndex = System.Array.IndexOf(RedPoints, hit.collider.gameObject);
                    // Cập nhật dữ liệu đối thủ
                    GameDataManager.Ins.SetPointData(pointDB.GetPointInfos(pointIndex));
                    this.enemyName.text = pointDB.GetPointInfos(pointIndex).EnemyName;
                    this.enemyHealth.text = pointDB.GetPointInfos(pointIndex).EnemyHealth.ToString();
                    this.enemyDamage.text = pointDB.GetPointInfos(pointIndex).EnemyAttack.ToString();
                    this.enemyLevel.text = "Level " + pointDB.GetPointInfos(pointIndex).EnemyLevel.ToString();
                    this.enemyAvt.sprite = pointDB.GetPointInfos(pointIndex).Image;
                    ShowDialog();
                }
            }
        }
    }

    // Hàm hiển thị dialog
    public void ShowDialog()
    {
        HideTextbox(textboxImage2);
        UpdateDialogData();
        dialogImage.gameObject.SetActive(true);
        isOpened = true;
    }

    // Gọi hàm này để ẩn dialog (nút Close hoặc sau khi hoàn thành hành động)
    public void HideDialog()
    {
        dialogImage.gameObject.SetActive(false);
        isOpened = false;

    }
    public void ShowTextBox()
    {
        textboxImage1.gameObject.SetActive(true);
        isOpened = true;

    }
    public void ShowNextTextbox()
    {
        HideTextbox(textboxImage1);
        textboxImage2.gameObject.SetActive(true);
        isOpened = true;


    }
    public void HideTextbox(UnityEngine.UI.Image textbox)
    {
        textbox.gameObject.SetActive(false);
        isOpened = false;

    }

    public void OpenMenu()
    {
        menuImage.gameObject.SetActive(true);
        menuBtn.interactable = false;
        isOpened = true;

    }
    public void HideMenu()
    {
        menuImage.gameObject.SetActive(false);
        menuBtn.interactable = true;
        isOpened = false;

    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Main Game");
    }

}
