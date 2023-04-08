using DG.Tweening;
using UnityEngine;

public class DeckBar : MonoBehaviour
{
    [SerializeField] private Transform deckRoot;

    private State _state;

    public void Hide()
    {
        if (_state == State.Hidden)
        {
            return;
        }

        UIManager.Instance.DefenseButton.SetActive(true);
        UIManager.Instance.ProductionButton.SetActive(true);

        deckRoot.DOLocalMoveY(-500f, 0.25f);
        _state = State.Hidden;
        GameManager.Instance.SelectedDeck = null;
        AudioManager.Instance.PlayUI1();
    }

    public void Show()
    {
        if (_state == State.Shown)
        {
            return;
        }

        UIManager.Instance.DefenseButton.SetActive(false);
        UIManager.Instance.ProductionButton.SetActive(false);
        deckRoot.DOLocalMoveY(0f, 0.25f);
        _state = State.Shown;
        GameManager.Instance.SelectedDeck = this;
    }

    public void Lower()
    {
        if (_state == State.Lowered)
        {
            return;
        }

        deckRoot.DOLocalMoveY(-260f, 0.25f);
        _state = State.Lowered;
        AudioManager.Instance.PlayUI1();
    }

    public void OnClicked()
    {
        if (_state == State.Lowered)
        {
            Show();
        }
        else if (_state == State.Shown)
        {
            Lower();
        }
    }

    private enum State
    {
        Hidden,
        Shown,
        Lowered
    }
}