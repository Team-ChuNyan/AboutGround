using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private SceneType _currentScene;

    private void Awake()
    {
        InitializeScene(_currentScene);
    }

    private void InitializeScene(SceneType type)
    {
        switch (type)
        {
            case SceneType.Title:
                break;
            case SceneType.Main:
                LoadMainScene();
                break;
            case SceneType.Loading:
                break;
        }
    }

    private void LoadMainScene()
    {

    }
}
