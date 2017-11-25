using System.Collections.Generic;
using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes{

	[Description("Calls one random output each time In is called")]
	[ContextDefinedOutputs(typeof(Flow), typeof(int))]
	public class Random : FlowControlNode, IMultiPortNode {

		[SerializeField]
		private int _portCount = 4;
		public int portCount{
			get {return _portCount;}
			set {_portCount = value;}
		}

		private int current;

		protected override void RegisterPorts(){
			var outs = new List<FlowOutput>();
			for (var i = 0; i < portCount; i++){
				outs.Add( AddFlowOutput(i.ToString()) );
			}
			AddFlowInput("In", (f)=> {
				current = UnityEngine.Random.Range(0, portCount);
				outs[current].Call(f);
			});
			AddValueOutput<int>("Current", ()=>{ return current; });
		}
	}
}