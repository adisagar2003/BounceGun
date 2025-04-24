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
        Vector3 lerpedScale;
        Vector3 lerpedPosition;
        Vector3 targetScale;
        Vector3 targetPosition;

        float timeElapsed = 0.0f;
        while (timeElapsed < _moveTime)
        {
            timeElapsed += Time.deltaTime;

            // on mouse over
            // move the button slightly above
            // scale the button a lil bit
            if (isMouseOver)
            {
                targetScale = _startScale + new Vector3(_moveScale, _moveScale, _moveScale);
                targetPosition = _startPos + new Vector3(0, _upScale, 0);
            }
            // reset to initials
            else
            {
                targetScale = _startScale;
                targetPosition = _startPos;
            }

            // lerp values
            float t = (timeElapsed / _moveTime);
            lerpedScale = Vector3.Lerp(transform.localScale, targetScale, t);
            lerpedPosition = Vector3.Lerp(transform.position, targetPosition, t);

            transform.localScale = lerpedScale;
            transform.position = lerpedPosition;

            yield return null;
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(MoveButton(true));
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
