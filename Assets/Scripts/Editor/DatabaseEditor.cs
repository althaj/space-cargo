using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PSG.SpaceCargo.Core;
using System;
using System.IO;
using System.Linq;

namespace PSG.SpaceCargo.Editor
{
    public class DatabaseEditor : EditorWindow
    {
        public Database database;

        private List<bool> foldouts;
        private static GUIStyle headerStyle;
        private static GUIStyle itemStyle;

        private Texture2D bgTexture;


        private Vector2 scrollPosition = Vector2.zero;

        #region Menu items
        /// <summary>
        /// Open the database editor.
        /// </summary>
        [MenuItem("Space Cargo/Database Editor")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(DatabaseEditor), false, "Database Editor");
        }
        #endregion

        #region Editor methods
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
            #region GUI styles
            headerStyle = new GUIStyle();
            headerStyle.fontSize = 12;
            headerStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1);
            headerStyle.wordWrap = true;

            itemStyle = new GUIStyle();
            itemStyle.fontSize = 10;
            itemStyle.normal.textColor = Color.grey;
            itemStyle.active.textColor = Color.white;

            if (bgTexture == null)
            {
                Color color = new Color(0.1f, 0.1f, 0.1f, 1);
                Color[] pixels = new Color[] { color, color, color, color };

                bgTexture = new Texture2D(2, 2);
                bgTexture.SetPixels(pixels);
                bgTexture.Apply();
            }
            itemStyle.normal.background = bgTexture;
            #endregion

            #region Header
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
            #endregion

            #region Database
            if (database != null)
            {
                if (foldouts == null || foldouts.Count != database.HexList.Count)
                    foldouts = database.HexList.Select(x => false).ToList();

                GUILayout.BeginVertical();

                float width = position.width * 0.94f;
                float[] fieldWidths = { width * 0.46f, width * 0.15f, width * 0.15f, width * 0.15f, width * 0.09f };

                #region Top buttons
                GUILayout.Space(30);

                GUILayout.BeginHorizontal();
                GUILayout.Space(width * 0.02f);
                if (GUILayout.Button("Add hex", GUILayout.ExpandWidth(false)))
                {
                    AddHex();
                }

                if (GUILayout.Button("Expand all", GUILayout.ExpandWidth(false)))
                {
                    ExpandAll();
                }

                if (GUILayout.Button("Collapse all", GUILayout.ExpandWidth(false)))
                {
                    CollapseAll();
                }
                GUILayout.EndHorizontal();
                #endregion

                #region Hex header
                GUILayout.Space(10);

                GUILayout.BeginHorizontal(headerStyle);
                GUILayout.Space(width * 0.02f);
                GUILayout.Label("Title", headerStyle, GUILayout.Width(fieldWidths[0] * 1.03f));
                GUILayout.Label("Spaceship spaces", headerStyle, GUILayout.Width(fieldWidths[1] * 1.03f));
                GUILayout.Label("Required spaceships", headerStyle, GUILayout.Width(fieldWidths[2] * 1.03f));
                GUILayout.Label("Required credits", headerStyle, GUILayout.Width(fieldWidths[3] * 1.03f));
                GUILayout.EndHorizontal();
                #endregion
                
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

                #region Hex items
                for (int i = 0; i < database.HexList.Count; i++)
                {
                    GUILayout.Space(5);

                    #region Hex item
                    GUILayout.BeginVertical(itemStyle);

                    #region Hex fields
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

                    database.HexList[i].SpaceShipSpaces = EditorGUILayout.IntField("", database.HexList[i].SpaceShipSpaces, GUILayout.Width(fieldWidths[1]));
                    database.HexList[i].RequiredSpaceShips = EditorGUILayout.IntField("", database.HexList[i].RequiredSpaceShips, GUILayout.Width(fieldWidths[2]));
                    database.HexList[i].RequiredCredits = EditorGUILayout.IntField("", database.HexList[i].RequiredCredits, GUILayout.Width(fieldWidths[3]));

                    if (GUILayout.Button(new GUIContent("X", "Remove hex"), GUILayout.Width(fieldWidths[4])))
                    {
                        DeleteHex(i);
                    }

                    GUILayout.EndHorizontal();
                    #endregion

                    #region Card item
                    if (database.HexList[i].Card != null)
                    {
                        CardData card = database.HexList[i].Card;

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(width * 0.02f);
                        GUILayout.BeginVertical();

                        foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "Card", true);
                        if (foldouts[i])
                        {
                            #region Card headers
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Title", headerStyle, GUILayout.Width(fieldWidths[0] * 1.03f));
                            GUILayout.Label("Credits", headerStyle, GUILayout.Width(fieldWidths[1] * 1.03f));
                            GUILayout.Label("Actions", headerStyle, GUILayout.Width(fieldWidths[2] * 1.03f));
                            GUILayout.Label("Cards", headerStyle, GUILayout.Width(fieldWidths[3] * 1.03f));
                            GUILayout.Label("Image", headerStyle, GUILayout.Width(fieldWidths[4]));
                            GUILayout.EndHorizontal();
                            #endregion

                            #region Card fields
                            GUILayout.BeginHorizontal();

                            EditorGUI.BeginChangeCheck();
                            card.Title = EditorGUILayout.TextField("", card.Title, GUILayout.Width(fieldWidths[0]));
                            if (EditorGUI.EndChangeCheck())
                            {
                                AssetHelpers.RenameAsset(AssetDatabase.GetAssetPath(card.GetInstanceID()), card.Title);
                            }
                            EditorGUI.EndChangeCheck();
                            card.Credits = EditorGUILayout.IntField("", card.Credits, GUILayout.Width(fieldWidths[1]));
                            card.Actions = EditorGUILayout.IntField("", card.Actions, GUILayout.Width(fieldWidths[2]));
                            card.Cards = EditorGUILayout.IntField("", card.Cards, GUILayout.Width(fieldWidths[3]));
                            card.Image = (Sprite)EditorGUILayout.ObjectField("", card.Image, typeof(Sprite), allowSceneObjects: false, GUILayout.Width(fieldWidths[4]));

                            GUILayout.EndVertical();
                            #endregion
                        }
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }
                    #endregion

                    GUILayout.EndVertical();
                    #endregion

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(database.HexList[i]);
                        EditorUtility.SetDirty(database.HexList[i].Card);
                    }
                }
                #endregion

                #region Empty database
                if (database.HexList == null || database.HexList.Count == 0)
                {
                    GUILayout.Label("This database is empty.");
                }
                #endregion

                GUILayout.EndScrollView();

                GUILayout.EndVertical();

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(database);
                }
            }
            #endregion
        }

        public void OnInspectorUpdate()
        {
            this.Repaint();
        }
        #endregion

        #region Database methods
        /// <summary>
        /// Create a database asset.
        /// </summary>
        /// <param name="path">Path to the database.</param>
        /// <returns>Returns the new database.</returns>
        public Database CreateDatabase(string path)
        {
            Database asset = CreateInstance<Database>();

            AssetHelpers.CreateFolder(path);

            asset.HexList = new List<HexData>();
            foldouts = new List<bool>();

            AssetDatabase.CreateAsset(asset, path + "/Database.asset");
            AssetDatabase.SaveAssets();

            string databasePath = AssetDatabase.GetAssetPath(asset);
            EditorPrefs.SetString(Constants.DATABASE_PATH_KEY, databasePath);

            return asset;
        }

        /// <summary>
        /// Create a database asset.
        /// </summary>
        /// <returns>Returns the new database.</returns>
        public Database CreateDatabase()
        {
            return CreateDatabase(Constants.DATABASE_PATH);
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
                    database = CreateDatabase();
                }
            }
            else
            {
                database = CreateDatabase();
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

                    foldouts = database.HexList.Select(x => false).ToList();
                }
            }
        }

        /// <summary>
        /// Expand all foldouts.
        /// </summary>
        private void ExpandAll()
        {
            if(foldouts != null)
                for (int i = 0; i < foldouts.Count; i++)
                    foldouts[i] = true;
        }

        /// <summary>
        /// Collapse all foldouts.
        /// </summary>
        private void CollapseAll()
        {
            if (foldouts != null)
                for (int i = 0; i < foldouts.Count; i++)
                    foldouts[i] = false;
        }
        #endregion

        #region Hex methods
        /// <summary>
        /// Add a new hex to the database.
        /// </summary>
        private void AddHex()
        {
            database.HexList.Add(CreateHex());
            foldouts.Add(false);
        }

        /// <summary>
        /// Create a new empty hex asset. Alongside a new hex a card is also created.
        /// </summary>
        /// <returns></returns>
        private HexData CreateHex()
        {
            CardData card = CreateInstance<CardData>();
            card.Title = "New card";

            AssetHelpers.CreateFolder(Constants.CARD_PATH);
            AssetDatabase.CreateAsset(card, AssetDatabase.GenerateUniqueAssetPath(Constants.CARD_PATH + "/New card.asset"));
            AssetDatabase.SaveAssets();

            HexData newHex = CreateInstance<HexData>();
            newHex.Title = "New hex";
            newHex.Card = card;

            AssetHelpers.CreateFolder(Constants.HEX_PATH);
            AssetDatabase.CreateAsset(newHex, AssetDatabase.GenerateUniqueAssetPath(Constants.HEX_PATH + "/New hex.asset"));
            AssetDatabase.SaveAssets();

            return newHex;
        }

        /// <summary>
        /// Delete a hex from the database.
        /// </summary>
        /// <param name="index">Index of the hex to delete.</param>
        private void DeleteHex(int index)
        {
            string path = AssetDatabase.GetAssetPath(database.HexList[index].Card);
            AssetDatabase.DeleteAsset(path);

            path = AssetDatabase.GetAssetPath(database.HexList[index]);
            AssetDatabase.DeleteAsset(path);
            database.HexList.RemoveAt(index);

            foldouts.RemoveAt(index);
        }
        #endregion
    }
}
