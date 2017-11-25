using System.Collections.Generic;
using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes{

	[Description("Split the Flow in multiple directions. Calls all outputs in the same frame but in order")]
	public class Split : FlowControlNode, IMultiPortNode {

		[SerializeField]
		private int _portCount = 4;
		public int portCount{
			get {return _portCount;}
			set {_portCount = value;}
		}

		protected override void RegisterPorts(){
			var outs = new List<FlowOutput>();
			for (var i = 0; i < portCount; i++){
				outs.Add( AddFlowOutput(i.ToString()) );
			}
			AddFlowInput("In", (f)=> {
				for (var i = 0; i < portCount; i++){
					if (!graph.isRunning){
						break;
					}
					outs[i].Call(f);
				}
			});
		}
	}
}