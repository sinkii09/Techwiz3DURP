using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Not use
/// </summary>
public class SaveLoadUIModel : MonoBehaviour
{
    [SerializeField] Button btnPrefab;

    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] Transform content;
    [SerializeField] Button backBtn;

    List<Button> BtnList = new List<Button>();
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void Start()
    {
        backBtn.onClick.AddListener(() => { gameObject.SetActive(false); AudioManager.Instance.PlaySFX("BtnClick"); });
    }
    private void OnDestroy()
    {
        backBtn.onClick.RemoveAllListeners();
    }
    //public void ShowAsSaveWindow()
    //{
    //    ClearBtnList();
    //    headerText.text = "Save";

    //    var allData = SaveLoadManager.Instance.GetAllSaveDataName();

    //    foreach ( var data in allData )
    //    {
    //        var button = Instantiate(btnPrefab, content);
    //        button.GetComponentInChildren<TextMeshProUGUI>().text = data;
    //        button.onClick.AddListener(() => { GameManager.Instance.SaveGame(data); });
    //        BtnList.Add(button);
    //    }
    //    var newBtn = Instantiate(btnPrefab, content);
    //    newBtn.GetComponentInChildren<TextMeshProUGUI>().text = "New Slot";
    //    newBtn.onClick.AddListener(() => { GameManager.Instance.SaveGame(""); });
    //    BtnList.Add(newBtn);

    //    gameObject.SetActive(true);
    //}
    //public void ShowAsLoadWindow()
    //{
    //    ClearBtnList();

    //    headerText.text = "Load";

    //    var allData = SaveLoadManager.Instance.GetAllSaveDataName().ToList();
    //    foreach (var data in allData)
    //    {
    //        var button = Instantiate(btnPrefab, content);
    //        button.GetComponentInChildren<TextMeshProUGUI>().text = data.ToString();
    //        button.onClick.AddListener(() => { GameManager.Instance.LoadGame(data); });
    //        BtnList.Add(button);
    //    }
    //    gameObject.SetActive(true);
    //}
    void ClearBtnList()
    {
        foreach ( var button in BtnList )
        {
            Destroy(button.gameObject);
        }
        BtnList.Clear();
    }
}
