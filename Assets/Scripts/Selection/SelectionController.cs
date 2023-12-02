using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.CanvasScaler;

public class SelectionController : MonoBehaviour
{
    [Header("Selection texture")]
    public Texture2D selectionHighlight = null;
    public static Rect selection = new Rect(0, 0, 0, 0);

    [HideInInspector] public Building selectedBuilding;

    private List<Unit> unitsSelected = new List<Unit>();
    private UIManager UI;
    private UnitsController UC;
    private Vector3 startClick = -Vector3.one;

    private void Start()
    {
        UI = GetComponent<UIManager>();
        UC = GetComponent<UnitsController>();
    }

    void Update()
    {
        SelectionRectangle();
        Select();

        if (selectedBuilding != null)
        {
            if (selectedBuilding.data.buildingName == "Barracks")
            {
                if (selectedBuilding.GetComponent<Barracks>().creating)
                    UI.ShowHideUnitQueue(true);
                else
                    UI.ShowHideUnitQueue(false);
            }
            else 
            {
                UI.ShowHideUnitQueue(false);
            }
        }
    }

    private void Select()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (!UC.agresiveMove)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (raycastHit.transform.tag == "Environment")
                        {
                            ClearSelection();
                            UI.ShowUtilities(null);
                            UI.ShowHideUnitQueue(false);
                        }
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        ClearSelection();

                        if (raycastHit.transform.tag == "Unit")
                        {
                            AddSelectedUnit(raycastHit.transform.GetComponent<Unit>());
                            UI.ShowUtilities(unitsSelected[0].data.unitName);
                            UI.ShowUnitInfo(unitsSelected[0]);
                        }

                        if (raycastHit.transform.tag == "Building")
                        {
                            selectedBuilding = raycastHit.transform.GetComponent<Building>();

                            if (selectedBuilding.Placed == true)
                            {
                                selectedBuilding.SetSelected(true);
                                UI.ShowUtilities(selectedBuilding.data.buildingName);
                                UI.ShowBuildingInfo(selectedBuilding);
                            }
                        }
                    }
                }
            }
        }          
    }

    private void SelectionRectangle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startClick = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selection.width < 0)
            {
                selection.x += selection.width;
                selection.width = -selection.width;
            }

            if (selection.height < 0)
            {
                selection.y += selection.height;
                selection.height = -selection.height;
            }
            startClick = -Vector3.one;
        }

        if (Input.GetMouseButton(0))
        {
            selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));
        }
    }

    public static float InvertMouseY(float y)
    {
        return Screen.height - y;
    }

    //Draw rectangle
    private void OnGUI()
    {
        if (startClick != -Vector3.one)
        {
            GUI.color = new Color(1, 1, 1, 0.5f);
            GUI.DrawTexture(selection, selectionHighlight);
        }
    }

    //Utils ----------------------
    public List<Unit> GetSelectedUnits()
    {
        return unitsSelected;
    }

    public Building GetSelectedBuilding()
    {
        return selectedBuilding;
    }

    public void AddSelectedUnit(Unit unit)
    {
        if(unit != null)
            if(!unitsSelected.Contains(unit))
            {
                unitsSelected.Add(unit);
                unit.SetSelected(true);
            }
    }
    private void ClearSelection()
    {
        if(unitsSelected.Count > 0)
        {
            foreach (Unit unit in unitsSelected)
            {
                unit.SetSelected(false);
            }
            unitsSelected.Clear();
        }

        if (selectedBuilding != null)
        {
            selectedBuilding.SetSelected(false);
            selectedBuilding = null;
        }
    }

    public void DeselectUnit(Unit unit)
    {
        if (unitsSelected.Contains(unit))
        {
            unit.SetSelected(false);
            unitsSelected.Remove(unit);
        }
    }
}