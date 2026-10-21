using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class RenderPuissance4Plateau : MonoBehaviour
{
    [SerializeField] GameObject board2D;
    [SerializeField] GameObject board3D;
    [SerializeField] GameObject jeton2D;
    [SerializeField] GameObject jeton3D;

    GameObject currentJeton2D;
    GameObject currentJeton3D;

    bool isPieceDropping = false;
    float droppingTimer;
    float lineCountDropping;

    Vector3 jeton2DBeginPos;
    Vector3 jeton2DObjectivPos;
    Vector3 jeton3DBeginPos;
    Vector3 jeton3DObjectivPos;
    bool isPlayer1;

    List<GameObject> jetons = new List<GameObject>();

    void Start()
    {
        board2D.SetActive(false);
        board3D.SetActive(true);

        isPlayer1 = true;
        AddJetonToBoard(isPlayer1);
    }

    void Update()
    {
        //If a piece is dropping and the game is not paused
        if (!GameManager.Instance.isGamePaused && isPieceDropping)
        {
            droppingTimer -= Time.deltaTime;
            //Update the piece position
            if (droppingTimer > 0f)
            {
                float timerConvertedIn1 = droppingTimer * (6 / lineCountDropping);
                currentJeton2D.transform.localPosition = Vector3.Lerp(jeton2DBeginPos, jeton2DObjectivPos, (1 - timerConvertedIn1));
                currentJeton3D.transform.localPosition = Vector3.Lerp(jeton3DBeginPos, jeton3DObjectivPos, (1 - timerConvertedIn1));
            }
            else
            {
                currentJeton2D.transform.localPosition = jeton2DObjectivPos;
                currentJeton2D = null;
                currentJeton3D.transform.localPosition = jeton3DObjectivPos;
                currentJeton3D = null;
                isPieceDropping = false;
                AddJetonToBoard(!isPlayer1);
            }
        }
    }

    public void UpdateBoard(int _currentIndex)
    {
        UpdateBoard2D(_currentIndex);
        UpdateBoard3D(_currentIndex);
    }

    public void DropJeton(int _dropColumnIndex, int _dropLineObjectiv, bool _isPlayer1)
    {
        isPlayer1 = _isPlayer1;
        isPieceDropping = true;
        droppingTimer = (float)1 / (float)6 * (float)(_dropLineObjectiv + 1);
        lineCountDropping = _dropLineObjectiv + 1;
        if (board2D.activeSelf)
        {
            DropJeton2D(_dropColumnIndex, _dropLineObjectiv);
            DropJeton3D(_dropColumnIndex, _dropLineObjectiv);
        }
        else
        {
            DropJeton3D(_dropColumnIndex, _dropLineObjectiv);
            DropJeton2D(_dropColumnIndex, _dropLineObjectiv);
        }
    }

    public void AddJetonToBoard(bool _isPlayer1)
    {
        AddJeton2DBoard(_isPlayer1);
        AddJeton3DBoard(_isPlayer1);
    }

    private void UpdateBoard2D(int _currentIndex)
    {
        currentJeton2D.transform.localPosition = new Vector3(-450 + (150 * _currentIndex), 525, 0);
    }

    private void UpdateBoard3D(int _currentIndex)
    {
        currentJeton3D.transform.localPosition = new Vector3(-5.25f + (1.75f * _currentIndex), 6.25f, 0);
    }

    private void DropJeton2D(int _dropColumnIndex, int _dropLineObjectiv)
    {
        //Prepare the animation
        jeton2DBeginPos = new Vector3(-450 + (150 * _dropColumnIndex), 525, 0);
        jeton2DObjectivPos = new Vector3(-450 + (150 * _dropColumnIndex), 525 - (150 * (1 + _dropLineObjectiv)), 0);
        currentJeton2D.transform.localPosition = jeton2DBeginPos;
    }

    private void DropJeton3D(int _dropColumnIndex, int _dropLineObjectiv)
    {
        //Prepare the animation
        jeton3DBeginPos = new Vector3(-5.25f + (1.75f * _dropColumnIndex), 6.25f, 0);
        jeton3DObjectivPos = new Vector3(-5.25f + (1.75f * _dropColumnIndex), 4.375f - (1.75f * _dropLineObjectiv), 0);
        currentJeton3D.transform.localPosition = jeton3DBeginPos;
    }

    private void AddJeton2DBoard(bool _isPlayer1)
    {
        jetons.Add(Instantiate(jeton2D, board2D.transform.Find("Canvas").Find("Plateau")));
        jetons[jetons.Count - 1].transform.SetAsFirstSibling();

        currentJeton2D = jetons[jetons.Count - 1];

        currentJeton2D.transform.localPosition = new Vector3(-450, 525, 0);

        if (_isPlayer1)
        {
            currentJeton2D.GetComponent<RawImage>().color = Color.yellow;
        }
        else
        {
            currentJeton2D.GetComponent<RawImage>().color = Color.red;
        }

    }

    private void AddJeton3DBoard(bool _isPlayer1)
    {
        jetons.Add(Instantiate(jeton3D, board3D.transform.Find("Plateau")));
        jetons[jetons.Count - 1].transform.SetAsFirstSibling();

        currentJeton3D = jetons[jetons.Count - 1];

        currentJeton3D.transform.localPosition = new Vector3(-5.25f, 6.5f, 0);

        if (_isPlayer1)
        {
            currentJeton3D.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else
        {
            currentJeton3D.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public void ChangeRender()
    {
        board2D.SetActive(!board2D.activeSelf);
        board3D.SetActive(!board3D.activeSelf);
    }

    public bool GetIsPieceDropping()
    {
        return isPieceDropping;
    }
}
