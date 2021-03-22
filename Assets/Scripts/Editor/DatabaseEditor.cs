using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PSG.SpaceCargo.Core;
using System;
using System.IO;

namespace PSG.SpaceCargo.Editor
{
    public class DatabaseEditor : EditorWindow
    {
        public Database database;

        [MenuItem("Space Cargo/Database Editor")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(DatabaseEditor), false, "Database Editor");
        }

        private void OnEnable()
        {
            if (EditorPrefs.HasKey(Constants.DATABASE_PATH_KEY))
            {
                string objectPath = EditorPrefs.GetString(Constants.DATABASE_PATH_KEY);
                database = AssetDatabase.LoadAssetAtPath(objectPath, typeof(Database)) as Database;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Database editor", EditorStyles.boldLabel);

            // Show database button
            if (database != null)
            {
                if (GUILayout.Button("Show database"))
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = database;
                }
            }

            // Open database button
            if (GUILayout.Button("Open database"))
            {
                OpenDatabase();
            }

            // New database button
            if (GUILayout.Button(new GUIContent("New database", "There is no overwrite protection right now! This will erase the current database!")))
            {
                CreateNewDatabase();
            }
            GUILayout.EndHorizontal();

            if (database != null)
            {
                GUILayout.BeginVertical();

                GUILayout.Space(30);

                float width = position.width * 0.96f;
                float[] fieldWidths = { width * 0.45f, width * 0.15f, width * 0.15f, width * 0.15f, width * 0.10f };

                GUILayout.BeginHorizontal();
                GUILayout.Space(width * 0.02f);
                if (GUILayout.Button("Add hex", GUILayout.ExpandWidth(false)))
                {
                    AddHex();
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.Space(width * 0.02f);
                EditorGUILayout.LabelField("Title", GUILayout.Width(fieldWidths[0]));
                EditorGUILayout.LabelField("Worker spaces", GUILayout.Width(fieldWidths[1]));
                EditorGUILayout.LabelField("Required workers", GUILayout.Width(fieldWidths[2]));
                EditorGUILayout.LabelField("Required credits", GUILayout.Width(fieldWidths[3]));

                GUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                for (int i = 0; i < database.HexList.Count; i++)
                {
                    GUILayout.Space(5);

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(width * 0.02f);

                    // Check for updated title, and rename the asset with the new title.
                    EditorGUI.BeginChangeCheck();
                    database.HexList[i].Title = EditorGUILayout.TextField("", database.HexList[i].Title as string, GUILayout.Width(fieldWidths[0]));
                    if (EditorGUI.EndChangeCheck())
                    {
                        AssetHelpers.RenameAsset(AssetDatabase.GetAssetPath(database.HexList[i].GetInstanceID()), database.HexList[i].Title);
                    }
                    EditorGUI.EndChangeCheck();

                    database.HexList[i].WorkerSpaces = EditorGUILayout.IntField("", database.HexList[i].WorkerSpaces, GUILayout.Width(fieldWidths[1]));
                    database.HexList[i].RequiredWorkers = EditorGUILayout.IntField("", database.HexList[i].RequiredWorkers, GUILayout.Width(fieldWidths[2]));
                    database.HexList[i].RequiredCredits = EditorGUILayout.IntField("", database.HexList[i].RequiredCredits, GUILayout.Width(fieldWidths[3]));

                    if (GUILayout.Button(new GUIContent("X", "Remove hex"), GUILayout.Width(fieldWidths[4])))
                    {
                        DeleteHex(i);
                    }

                    GUILayout.EndHorizontal();
                }

                if (database.HexList == null || database.HexList.Count == 0)
                {
                    GUILayout.Label("This database is empty.");
                }

                GUILayout.EndVertical();

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(database);
                }
            }
        }

        /// <summary>
        /// Delete a hex from the database.
        /// </summary>
        /// <param name="index">Index of the hex to delete.</param>
        private void DeleteHex(int index)
        {
            database.HexList[index].DeleteHex();
            database.HexList.RemoveAt(index);
        }

        /// <summary>
        /// Add a new hex to the database.
        /// </summary>
        private void AddHex()
        {
            database.HexList.Add(HexData.CreateHex());
        }

        /// <summary>
        /// Creates a new database. There is no overwrite protection!
        /// </summary>
        private void CreateNewDatabase()
        {
            if (database != null)
            {

                if (EditorUtility.DisplayDialog(
                    "Warning",
                    "Are you sure you want to create a new database? Existing database will be erased.",
                    "Yes",
                    "No"
                    ))
                {
                    database = Database.CreateDatabase();
                }
            }
            else
            {
                database = Database.CreateDatabase();
            }
        }

        /// <summary>
        /// Open an existing database.
        /// </summary>
        private void OpenDatabase()
        {
            string absPath = EditorUtility.OpenFilePanel("Select database", "", "");
            if (absPath.StartsWith(Application.dataPath))
            {
                string databasePath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                database = AssetDatabase.LoadAssetAtPath(databasePath, typeof(Database)) as Database;

                if (database.HexList == null)
                    database.HexList = new List<HexData>();

                if (database)
                {
                    EditorPrefs.SetString(Constants.DATABASE_PATH_KEY, databasePath);
                }
            }
        }
    }
}
