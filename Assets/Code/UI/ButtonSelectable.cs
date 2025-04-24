using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Used in buttons
/// Scale up and hover up buttons on mouse over
/// </summary>
public class ButtonSelectable : MonoBehaviour,ISelectHandler,IDeselectHandler,IPointerEnterHandler,IPointerExitHandler
{
    [Header("Controls")]
    [SerializeField] private float _upScale;
    [SerializeField] private float _moveScale;
    [SerializeField] private float _moveTime = 1.1f;

    private Vector3 _startPos;
    private Vector3 _startScale;

    private bool isMouseOver = false;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
    }
    private IEnumerator MoveButton(bool isMouseOver)
    {
        // on mouse over
        // move the button slightly above
        // scale the button a lil bit
        Vector3 lerpedScale;
        Vector3 lerpedPosition;
        float timeElapsed = 0.0f;
        while (timeElapsed < _moveTime)
        {
            timeElapsed += Time.deltaTime;
            Debug.Log(timeElapsed);
            if (isMouseOver)
            {
                lerpedScale = Vector3.Lerp(_startScale, _startScale + new Vector3(_moveScale, _moveScale, _moveScale), (timeElapsed / _moveTime));
                lerpedPosition = Vector3.Lerp(_startPos, _startPos + new Vector3(0, _upScale, 0), (timeElapsed / _moveTime));
            }
            else
            {
                lerpedScale = _startScale;
                lerpedPosition = _startPos;
            }

            yield return null;
            transform.localScale = lerpedScale;
            transform.position = lerpedPosition;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(MoveButton(false));
    }

    public void OnSelect(BaseEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(MoveButton(true));
    }
}
