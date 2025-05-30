using UnityEngine;

public class PlayerHome : MonoBehaviour
{
    float _hp;
    public float PlayerHomeHp => _hp;

    public int _currentHomeLevel;
    public int CurrentPlayerHomeLevel => _currentHomeLevel;

    [SerializeField] Sprite[] _homeSprites;

    public SpriteRenderer _currentHomeSpriteRenderer;
    Vector3 PlayerHomeSize;
    


    private void Awake()
    {
        _currentHomeSpriteRenderer = GetComponent<SpriteRenderer>();
        SetHomeNextLevel();
        PlayerHomeSize = GetComponent<SpriteRenderer>().bounds.size;
    }


    public void SetHomeNextLevel()
    {
        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];
        _currentHomeLevel++;
    }

   
}
