using System.Collections.Generic;
using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes{

	[Name("Switch Integer")]
	[Category("Flow Controllers/Switchers")]
	[Description("Branch the Flow based on an integer value. The Default output is called when the Index value is out of range.")]
	[ContextDefinedInputs(typeof(int))]
	public class SwitchInt : FlowControlNode, IMultiPortNode {

		[SerializeField]
		private int _portCount = 4;
		
		public int portCount{
			get {return _portCount;}
			set {_portCount = value;}
		}

		protected override void RegisterPorts(){
			var index = AddValueInput<int>("Index");
			var outs = new List<FlowOutput>();
			for (var i = 0; i < portCount; i++){
				outs.Add( AddFlowOutput(i.ToString()) );
			}
			var def = AddFlowOutput("Default");
			AddFlowInput("In", (Flow f)=>
			{
				var i = index.value;
				if (i >= 0 && i < outs.Count){
					outs[i].Call(f);
				} else {
					def.Call(f);
				}
			});
		}
	}
}