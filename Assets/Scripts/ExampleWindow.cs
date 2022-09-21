using UnityEngine;
using UnityEditor;

public class ExampleWindow : EditorWindow
{
    [MenuItem("Window/Colorizer")]
    public static void ShowWindow()
    {
        //Create Instance of Example Window
        GetWindow<ExampleWindow>("Colorizer");
    }

    private Color color;
    private void OnGUI()
    {
        //Show a label 
        GUILayout.Label("Color the selected objects!",EditorStyles.boldLabel);
        //Show Input text field  Name: m_Name
        //string name = EditorGUILayout.TextField("Name", name);
        //name = EditorGUILayout.TextField("Name", name);
        color = EditorGUILayout.ColorField("Color", color);
        
        //Button 
        if (GUILayout.Button("COLORIZE!"))
        {
            //Colorize objs which are selected
            Renderer renderer;
            foreach (GameObject obj in Selection.gameObjects)
            {
                renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial.color = color;
                }
            }
        }
    }
}
