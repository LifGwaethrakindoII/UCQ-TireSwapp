using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Ulyssess;

public class Button3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
    public UnityEvent onClick;

    public float crossFadeDuration;
    public float pushFadeDuration;
    public float rotationReturnDuration;

    public int materialIndex;

    private float normalizedCrossFade;
    private float normalizedPushFade;

    private SkinnedMeshRenderer skinnedMeshRenderer;


    private Coroutine crossFade;
    private Coroutine pushFade;
    private Coroutine rotationFade;

    public Material material;
    public Material materialCoche;
    public Color defaultColor;
    public Color highlightedColor;
    
    public string colorPropertyTag;
    private int colorPropertyID;
    private bool pointerDown;
    private bool sighted;
    private Quaternion originalRotation;
    public float moveSpeed = 10f;
    private bool inAnimation = false;

    public bool _selected;
    public Button3D _pairButton;

    private void OnDisable()
    {
        skinnedMeshRenderer.materials[materialIndex].SetColor(colorPropertyID, defaultColor);
        skinnedMeshRenderer.SetBlendShapeWeight(0, 0.0f);
    }

    private void OnEnable()
    {
        originalRotation = transform.rotation;
    }

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        materialCoche = GetComponent<Renderer>().material;
        normalizedCrossFade = 0.0f;
        normalizedPushFade = 0.0f;
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        crossFade = null;
        pushFade = null;

        colorPropertyID = Shader.PropertyToID(colorPropertyTag);
        pointerDown = false;

        skinnedMeshRenderer.materials[materialIndex].SetColor(colorPropertyID, defaultColor);
        skinnedMeshRenderer.SetBlendShapeWeight(0, 0.0f);

        
    }

    public void Update()
    {
        if(!inAnimation && transform.forward == new Vector3 (0,0,1))
        {
            transform.Rotate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        //Debug.Log(inAnimation);

        this.StartCoroutine(PushFade(_selected), ref pushFade);
    }

    public void OnPointerExit(PointerEventData _data)
    {
        //Debug.Log(transform.forward);
        inAnimation = false;
        this.StartCoroutine(ReturnToOriginalRotation(), ref rotationFade);
    }

    public void OnPointerEnter(PointerEventData _data)
    {
        inAnimation = true;
        //Debug.Log("COLOR");
        transform.Rotate(Vector3.up * moveSpeed * Time.deltaTime);
        this.DispatchCoroutine(ref rotationFade);
        //Debug.Log(transform.forward);
    }

    public void OnPointerClick(PointerEventData _data)
    {

        pushFade = this.StartCoroutine(PushFade(false), ref pushFade);
        onClick.Invoke();
    }

    public void OnPointerUp(PointerEventData _data)
    {
        //pushFade = this.StartCoroutine(PushFade(false), ref pushFade);
        if (pointerDown) onClick.Invoke();
        pointerDown = false;
        this.StartCoroutine(CrossFade(false), ref crossFade);
    }

    public void OnPointerDown(PointerEventData _data)
    {
        pointerDown = true;
        _selected = true;
        if(_pairButton != null) _pairButton._selected = false;
        //this.StartCoroutine(PushFade(), ref pushFade);
        crossFade = this.StartCoroutine(CrossFade(), ref crossFade);
        
    }

    private IEnumerator CrossFade(bool up = true)
    {
        while((up && normalizedCrossFade < 1.0f) || (!up && normalizedCrossFade > 0.0f))
        {
            float additionalTime = (Time.deltaTime / crossFadeDuration);
            normalizedCrossFade += up ? (Time.deltaTime / crossFadeDuration) : -(Time.deltaTime / crossFadeDuration);
            Color destinyColor = Color.Lerp(defaultColor, highlightedColor, normalizedCrossFade);
            skinnedMeshRenderer.materials[materialIndex].SetColor(colorPropertyID, destinyColor);
            yield return null;
        }

        normalizedCrossFade = up ? 1.0f : 0.0f;
    }

    private IEnumerator PushFade(bool up)
    {
        while ((up && normalizedPushFade < 1.0f) || (!up && normalizedPushFade > 0.0f))
        {
            float additionalTime = (Time.deltaTime / pushFadeDuration);
            normalizedPushFade += up ? (Time.deltaTime / pushFadeDuration) : -(Time.deltaTime / pushFadeDuration);
            skinnedMeshRenderer.SetBlendShapeWeight(0, normalizedPushFade * 100.0f);
            yield return null;
        }

        normalizedPushFade = up ? 1.0f : 0.0f;
        skinnedMeshRenderer.SetBlendShapeWeight(0, normalizedPushFade * 100.0f);
    }

    private IEnumerator ReturnToOriginalRotation()
    {
        Quaternion startingRotation = transform.rotation;
        float n = 0.0f;

        while(n < 1.0f)
        {
            transform.rotation = Quaternion.Lerp(startingRotation, originalRotation, n);
            n += (Time.deltaTime / rotationReturnDuration);
            yield return null;
        }

        transform.rotation = originalRotation;
        this.DispatchCoroutine(ref rotationFade);
    }
}