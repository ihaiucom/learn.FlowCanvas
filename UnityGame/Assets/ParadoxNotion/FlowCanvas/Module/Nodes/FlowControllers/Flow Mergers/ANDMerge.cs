using UnityEngine;
using System.Collections;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes{

	[Name("AND")]
	[Category("Flow Controllers/Flow Merge")]
	[Description("Calls Out when all inputs are called together in the same frame")]
	public class ANDMerge : FlowControlNode, IMultiPortNode {
		
		[SerializeField]
		private int _portCount = 2;

		private FlowOutput fOut;
		private int[] calls;
		private int lastFrameCall;

		public int portCount{
			get {return _portCount;}
			set {_portCount = value;}
		}

		protected override void RegisterPorts(){
			calls = new int[portCount];
			fOut = AddFlowOutput("Out");
			for (var _i = 0; _i < portCount; _i++){
				var i = _i;
				AddFlowInput(i.ToString(), (f)=> { Check(i, f); } );
			}
		}

		void Check(int index, Flow f){
			calls[index] = Time.frameCount; 
			for (var i = 0; i < calls.Length; i++){
				if (calls[i] != calls[index]){
					return;
				}
			}
			if (Time.frameCount != lastFrameCall){
				lastFrameCall = Time.frameCount;
				fOut.Call(f);
			}
		}
	}
}