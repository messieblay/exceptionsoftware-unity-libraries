using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace ExSoftware.ExEditor {
	public abstract class ExStylesGeneric {

		//		protected virtual string EditorResourcePath{ get { return "Assets/ExSoftware/ExEditor/Editor/Res/"; } }
		protected virtual string EditorResourcePath{ get { return "Assets/ExSoftware/ExEditor/Res/"; } }

		protected virtual string EditorAssets { get; set; }

		protected virtual string EditorSkinName { get { return "ExEditorSkin.guiskin"; } }

		protected virtual string EditorSkinLightName { get { return "ExEditorSkin.guiskin"; } }

		protected bool useDarkSkin = false;

		protected GUISkin Skin;
		protected GUISkin InspectorSkin;
		protected bool stylesLoaded = false;

		protected GUIStyle[] customStyles;

		protected bool LoadStyles () {
			if (stylesLoaded) {
				return true;
			}
//			Debug.Log ("Gen - " + System.IO.Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly ().Location));
			//Correct paths if necessary
			string projectPath = Application.dataPath;
			if (projectPath.EndsWith ("/Assets")) {
				projectPath = projectPath.Remove (projectPath.Length - ("Assets".Length));
			}

			if (!System.IO.File.Exists (projectPath + EditorAssets + "/" + EditorSkinName)) {
				//Initiate search

				System.IO.DirectoryInfo sdir = new System.IO.DirectoryInfo (Application.dataPath);

				Queue<System.IO.DirectoryInfo> dirQueue = new Queue<System.IO.DirectoryInfo> ();
				dirQueue.Enqueue (sdir);

				bool found = false;
				while (dirQueue.Count > 0) {
					System.IO.DirectoryInfo dir = dirQueue.Dequeue ();
					if (System.IO.File.Exists (dir.FullName + "/" + EditorSkinName)) {
						string path = dir.FullName.Replace ('\\', '/');
						found = true;
						//Remove data path from string to make it relative
						path = path.Replace (projectPath, "");

						if (path.StartsWith ("/")) {
							path = path.Remove (0, 1);
						}

						EditorAssets = path;
						Debug.Log ("Localizado directorio de assets de editor en '" + EditorAssets + "'");
						break;
					}
					System.IO.DirectoryInfo[] dirs = dir.GetDirectories ();
					for (int i = 0; i < dirs.Length; i++) {
						dirQueue.Enqueue (dirs [i]);
					}
				}

				if (!found) {
					Debug.LogWarning ("No se han podido encontrar los Asset de estilo");
					return false;
				}
			}

			//End checks
			Skin = AssetDatabase.LoadAssetAtPath (EditorAssets + "/" + EditorSkinName, typeof(GUISkin)) as GUISkin;

			GUISkin inspectorSkin = EditorGUIUtility.GetBuiltinSkin (EditorSkin.Inspector);
			if (Skin != null) {
				Skin.button = inspectorSkin.button;
			} else {
				//Load skin at old path
				Skin = AssetDatabase.LoadAssetAtPath (EditorAssets + "/" + EditorSkinName, typeof(GUISkin)) as GUISkin;
				if (Skin != null) {
					AssetDatabase.RenameAsset (EditorAssets + "/" + EditorSkinName, EditorSkinLightName);
				} else {
					return false;
				}
			} 

			LoadCustomStyles ();
			LoadTextures ();
			try {
				customStyles = Skin.customStyles;
				AddCustomStyles ();
			 
				Skin.customStyles = customStyles;
				//		GUI.skin = Skin;
				stylesLoaded = true;
				MonoBehaviour.print ("Ex: ExStyles Loaded");
			} catch (System.NullReferenceException) {
			}
			return true; 
		}

		protected virtual void LoadCustomStyles () { 
		}

		protected virtual void LoadTextures () { 
		}

		protected virtual void AddCustomStyles () { 
		}

		protected GUIStyle AddCustomStyle (string styleName, GUIStyle style) {
			return AddCustomStyle (ref customStyles, styleName, style);
		}

		protected GUIStyle AddCustomStyle (ref GUIStyle[] customStyles, string styleName, GUIStyle style) {
			int indexStyle = ArrayUtility.FindIndex<GUIStyle> (customStyles, s => s.name == styleName);
			if (indexStyle < 0) {
				ArrayUtility.Add<GUIStyle> (ref customStyles, style);
				return style;
			}  
			customStyles [indexStyle] = style; 
			return style;
		}

		#region resource loading

		protected Texture2D LoadEditorTexture (string fn) {
			Texture2D tx = AssetDatabase.LoadAssetAtPath (fn, typeof(Texture2D)) as Texture2D;
			if (tx == null) {
				Debug.LogWarning ("Failed to load texture: " + fn);
			} else if (tx.wrapMode != TextureWrapMode.Clamp) {
				string path = AssetDatabase.GetAssetPath (tx);
				TextureImporter tImporter = AssetImporter.GetAtPath (path) as TextureImporter;
				tImporter.textureType = TextureImporterType.GUI;
				tImporter.npotScale = TextureImporterNPOTScale.None;
				tImporter.filterMode = FilterMode.Point;
				tImporter.wrapMode = TextureWrapMode.Clamp;
				tImporter.maxTextureSize = 64;
//				tImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
				AssetDatabase.SaveAssets ();
			}
			return tx;
		}

		protected GUIStyle FindStyleInSkin (string name) { 
			return Skin.FindStyle (name);
		}

		protected Texture2D FindTextureBuiltIn (string name) { 
			return EditorGUIUtility.FindTexture (name);
		}

		#endregion
	}

}