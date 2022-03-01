using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private int _nbItems = 1;
    private int _nbCollectedItem = 0;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _miniMap;
    [SerializeField] private GameObject _info;
    [SerializeField] private Text _collectable;

    void Start()
    {
        UpdateCollectableTextInfo();
        _winScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!Application.isEditor)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MazePlay");
        }
        if (_nbCollectedItem >= _nbItems && _winScreen != null && _winScreen.activeSelf == false)
        {
            _miniMap.SetActive(false);
            _info.SetActive(false);
            _winScreen.SetActive(true);
            LevelManager.maseSize += 3;
        }
    }

    private void UpdateCollectableTextInfo()
    {
        _collectable.text = "Récupération : " + _nbCollectedItem + " / " + _nbItems;
    }

    public void SetNbrItems(int nbrItems)
    {
        _nbItems = nbrItems;
        UpdateCollectableTextInfo();
    }

    public void ItemCollected()
    {
        _nbCollectedItem++;
        UpdateCollectableTextInfo();
    }


}
