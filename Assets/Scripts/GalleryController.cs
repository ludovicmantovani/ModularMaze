using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour
{
    [SerializeField] private GameObject _camTarget;

    [SerializeField] private List<GameObject> _modelPrefabs;

    [SerializeField] private Text _modelName;

    private GameObject _currentModel;
    private int _index = 0;
    void Start()
    {
        DisplayModel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _index++;
            if (_modelPrefabs != null && _modelPrefabs.Count <= _index)
                _index = 0;
            DisplayModel();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _index--;
            if (_modelPrefabs != null && _index < 0)
                _index = _modelPrefabs.Count - 1;
            DisplayModel();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }


    private void DisplayModel()
    {
        if (_modelPrefabs != null && _modelPrefabs.Count > _index)
        {
            if (_currentModel != null)
            {
                Destroy(_currentModel);
            }
            _currentModel = Instantiate(_modelPrefabs[_index], _camTarget.transform.position, Quaternion.identity, _camTarget.transform);
            _modelName.text = _currentModel.name.Substring(0, _currentModel.name.Length - 7);
        }
    }
}
