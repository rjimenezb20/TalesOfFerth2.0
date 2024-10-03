using UnityEngine;

public class Unit : MonoBehaviour
{
    [HideInInspector] public bool inGroup = false;
    public GameObject selectionCircle;
    public UnitData data;

    private SelectionController SC;
    private ObjectPool pool;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        SC = FindAnyObjectByType<SelectionController>();
        pool = FindAnyObjectByType<ObjectPool>();
    }
    
    void Update()
    {
        CheckIfSelected();
    }

    public void SetSelected(bool a)
    {
        selectionCircle.SetActive(a);
    }

    private void CheckIfSelected()
    {
        //Selection rectangle
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = SelectionController.InvertMouseY(camPos.y);

            if (SelectionController.selection.Contains(camPos, true))
                SC.AddSelectedUnit(this);
        }
    }

    public void Death()
    {
        SC.DeselectUnit(this);
        Component[] components = GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component is Behaviour && !(component is Animator))
            {
                ((Behaviour)component).enabled = false;
            }
        }

        GetComponent<CapsuleCollider>().enabled = false;

        animator.CrossFade("Death", 0.1f);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}