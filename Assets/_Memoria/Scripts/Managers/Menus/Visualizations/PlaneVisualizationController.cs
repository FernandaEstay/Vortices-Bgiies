using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneVisualizationController : MonoBehaviour {
    public VisualizationCanvasController visualizationController;
    string visualizationName = "Plane";

    private void OnEnable()
    {
        if (visualizationController != null)
        {
            SelectThisVisualization();
            visualizationController.scrollDown.LaunchScrollDown("Plane visualization description", "This visualization is designed to be displayed in a monitor and shows objects distribuited in a plane configuration.");
            visualizationController.availableActionsTitle = "Plane visualization actions";
            visualizationController.availableActionsList = "[·]Advance to next plane: If the amount of objects is too big and can't be show in a single screen, the remaining will be placed in another view, called plane. This action highlights the images in the next Plane." +
                "\n[·]Go back to previous plane: If the advance to the next plane action was taken, this action allows to go back and highlight the objects of the previous Plane" +
                "\n[·]Mark/unmark as category: Marks (or, if already marked, unmarks) the current highlighted object as part of one category. There are four different actions, Mark as category 1, 2, 3 and 4." +
                "\n[·]Show category: Shows the objects marked under the category. There are four different actions, Show category 1, 2, 3 and 4. If already in the category and same action is used, goes back go gallery.";
        }
    }

    public void SelectThisVisualization()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;        
        GLPlayerPrefs.SetString(Scope, "CurrentVisualization", visualizationName);
        visualizationController.UpdateCurrentSelectedVisualizationText();
    }

}
