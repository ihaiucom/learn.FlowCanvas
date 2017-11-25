using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes{

	[Name("On Variable Change")]
	[Category("Events/Other")]
	[Description("Called when the target variable change. (Not whenever it is set).")]
	public class VariableChangedEvent : EventNode {

		[BlackboardOnly]
		public BBParameter<object> targetVariable;

		private FlowOutput fOut;
		private object newValue;

		public override string name{
			get {return string.Format("{0} [{1}]", base.name, targetVariable);}
		}

		public override void OnGraphStarted(){
			if (targetVariable.varRef != null){
				targetVariable.varRef.onValueChanged += OnChanged;
			}
		}

		public override void OnGraphStoped(){
			if (targetVariable.varRef != null){
				targetVariable.varRef.onValueChanged -= OnChanged;
			}
		}

		protected override void RegisterPorts(){
			#if UNITY_EDITOR
			targetVariable.onVariableReferenceChanged -= OnVariableRefChange;
			targetVariable.onVariableReferenceChanged += OnVariableRefChange;
			#endif
			if (targetVariable.varRef != null){
				fOut = AddFlowOutput("Out");
				AddValueOutput("Value", targetVariable.refType, ()=>{ return newValue; });
			}
		}

		void OnChanged(string name, object value){
			newValue = value;
			fOut.Call(new Flow());
		}

		void OnVariableRefChange(Variable newVarRef){
			GatherPorts();
		}
	}
}