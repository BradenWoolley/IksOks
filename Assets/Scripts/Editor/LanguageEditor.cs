using System.IO;
using UnityEditor;
using UnityEngine;


public class LanguageEditor : EditorWindow
{

    #region Consts

    private const string FileExtension = ".txt";

    private const string FolderPath = "Assets/Localisation/Translations";

    private const string MenuPath = "Tools/Localisation Editor";

    #endregion


    #region Fields

    private string keyName = "";

    private string languageName = "";

    private string translation = "";

    #endregion


    #region Methods

    [MenuItem(MenuPath)]
    public static void ShowWindow()
    {
        GetWindow<LanguageEditor>("Localisation Editor");
    }

    private void AddEntry()
    {
        if (string.IsNullOrWhiteSpace(languageName))
        {
            EditorUtility.DisplayDialog("Error", "Language name cannot be empty.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(keyName))
        {
            EditorUtility.DisplayDialog("Error", "Key name cannot be empty.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(translation))
        {
            EditorUtility.DisplayDialog("Error", "Translation cannot be empty.", "OK");
            return;
        }

        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }

        string filePath = Path.Combine(FolderPath, languageName + FileExtension);
        string entry = $"{keyName},\"{translation}\"";

        // Read existing content before opening the writer to avoid sharing violation
        string existingContent = File.Exists(filePath) ? File.ReadAllText(filePath) : "";

        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            if (existingContent.Length > 0 && !existingContent.EndsWith("\n"))
            {
                writer.WriteLine();
            }
            writer.WriteLine(entry);
        }

        AssetDatabase.Refresh();
        Debug.Log($"[LocalisationEditor] Added entry to {filePath}: {entry}");

        // Clear key and translation fields, keep language name for convenience
        keyName = "";
        translation = "";
    }

    private void OnGUI()
    {
        GUILayout.Label("Localisation Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        languageName = EditorGUILayout.TextField("Language Name", languageName);
        keyName = EditorGUILayout.TextField("Key Name", keyName);
        translation = EditorGUILayout.TextField("Translation", translation);

        EditorGUILayout.Space();

        bool canAdd = !string.IsNullOrWhiteSpace(languageName)
                   && !string.IsNullOrWhiteSpace(keyName)
                   && !string.IsNullOrWhiteSpace(translation);

        GUI.enabled = canAdd;

        if (GUILayout.Button("Add Entry"))
        {
            AddEntry();
        }

        GUI.enabled = true;

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            $"Files are saved to: {FolderPath}/LanguageName{FileExtension}\nFormat: KeyName,\"Translation\"",
            MessageType.Info);
    }

    #endregion

}