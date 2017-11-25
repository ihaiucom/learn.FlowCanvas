#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using ParadoxNotion.Design;
using FlowCanvas;
using FlowCanvas.Macros;
using NodeCanvas.Editor;
using NodeCanvas.Framework;

namespace FlowCanvas.Editor{

	static class Commands {

		//Graphs
		[MenuItem("Tools/ParadoxNotion/FlowCanvas/Create/FlowScript Asset", false, 1)]
		public static void CreateFlowScript(){
			var newFS = EditorUtils.CreateAsset<FlowScript>(true);
			Selection.activeObject = newFS;
		}

		[MenuItem("Assets/Create/ParadoxNotion/FlowCanvas/FlowScript Asset")]
		public static void CreateFlowScript2(){
			var path = EditorUtils.GetAssetUniquePath("FlowScript.asset");
			var newGraph = EditorUtils.CreateAsset<FlowScript>(path);
			Selection.activeObject = newGraph;
		}		


		[MenuItem("Tools/ParadoxNotion/FlowCanvas/Create/Macro Asset", false, 1)]
		public static void CreateMacro(){
			var macro = EditorUtils.CreateAsset<Macro>(true);
			Selection.activeObject = macro;
		}

		[MenuItem("Assets/Create/ParadoxNotion/FlowCanvas/Macro Asset")]
		public static void CreateMacro2(){
			var path = EditorUtils.GetAssetUniquePath("FlowMacro.asset");
			var macro = EditorUtils.CreateAsset<Macro>(path);
			Selection.activeObject = macro;
		}
		///

		[MenuItem("Tools/ParadoxNotion/FlowCanvas/Create/Scene Global Blackboard")]
		public static void CreateGlobalBlackboard(){
			Selection.activeObject = GlobalBlackboard.Create();
		}

		[MenuItem("Tools/ParadoxNotion/FlowCanvas/Preferred Types Editor")]
		public static void ShowPrefTypes(){
			PreferedTypesEditorWindow.ShowWindow();
		}

		[MenuItem("Tools/ParadoxNotion/FlowCanvas/Graph Debug Console")]
		public static void OpenConsole(){
			GraphConsole.ShowWindow();
		}

	    [MenuItem("Tools/ParadoxNotion/FlowCanvas/External Inspector Panel")]
	    public static void ShowExternalInspector(){
	    	ExternalInspectorWindow.ShowWindow();
	    }



		//Extra

		[MenuItem("Tools/ParadoxNotion/FlowCanvas/Welcome Window")]
		public static void ShowWelcome(){
			WelcomeWindow.ShowWindow(typeof(FlowScript));
		}

		[MenuItem("Tools/ParadoxNotion/FlowCanvas/Visit Website")]
		public static void VisitWebsite(){
			Help.BrowseURL("http://flowcanvas.paradoxnotion.com");
		}
	}
}

#endif