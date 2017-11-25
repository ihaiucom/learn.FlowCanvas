using UnityEngine;
using System.Collections.Generic;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes{

	[Name("Toggle (Multi)")]
	[Description("Whenever In is called the 'current' output is called as well. Calling '+' or '-' changes the current output respectively up or down.")]
	[Category("Flow Controllers/Togglers")]
	[ContextDefinedOutputs(typeof(int))]
	public class MultiToggle : FlowControlNode, IMultiPortNode {
		
		[SerializeField]
		private int _portCount = 4;
		public int portCount{
			get {return _portCount;}
			set {_portCount = value;}
		}

		public int current;
		private int original;

		public override string name {
			get {return base.name + " " + string.Format( "[{0}]", current.ToString() );}
		}

		public override void OnGraphStarted(){ current = Mathf.Clamp(current, 0, portCount-1); original = current; }
		public override void OnGraphStoped(){ current = original; }

		protected override void RegisterPorts(){
			var outs = new List<FlowOutput>();
			for (int i = 0; i < portCount; i++){
				outs.Add( AddFlowOutput(i.ToString()) );
			}
			AddFlowInput("In", (f)=> { outs[current].Call(f); });
			AddFlowInput("-", (f)=> { current = (int)Mathf.Repeat( current - 1, portCount); });
			AddFlowInput("+", (f)=> { current = (int)Mathf.Repeat( current + 1, portCount); });
			AddValueOutput<int>("Current", ()=>{ return current; });
		}
	}
}