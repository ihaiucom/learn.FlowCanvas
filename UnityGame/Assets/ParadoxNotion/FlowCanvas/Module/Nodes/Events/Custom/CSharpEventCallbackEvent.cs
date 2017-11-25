using System;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace FlowCanvas.Nodes
{
    [Name("C# Event Callback", 2)]
	[Category("Events/Custom")]
	[Description("Providing a C# Event, Register a callback to be called when that event is raised.")]
	[ContextDefinedInputs(typeof(SharpEvent))]
	public class CSharpEventCallback : EventNode {

		[SerializeField]
		private SerializedTypeInfo _type;
		[SerializeField]
		private bool _autoHandleRegistration;

		private ReflectedDelegateEvent reflectedEvent;
		private FlowOutput flowCallback;
		private ValueInput eventInput;
		private object[] args;

		private Type type{
			get {return _type != null? _type.Get() : null;}
			set
			{
				if (_type == null || _type.Get() != value){
					_type = new SerializedTypeInfo(value);
				}
			}
		}

		private bool autoHandleRegistration{
			get {return _autoHandleRegistration;}
			set
			{
				if (_autoHandleRegistration != value){
					_autoHandleRegistration = value;
					GatherPorts();
				}
			}
		}

		public override void OnGraphStarted(){
			if (autoHandleRegistration){
				var sharpEvent = eventInput.value as SharpEvent;
				if (sharpEvent != null){
					sharpEvent.StartListening(reflectedEvent, Callback);
				}
			}
		}

		public override void OnGraphStoped(){
			if (autoHandleRegistration){
				var sharpEvent = eventInput.value as SharpEvent;
				if (sharpEvent != null){
					sharpEvent.StopListening(reflectedEvent, Callback);
				}
			}
		}

		protected override void RegisterPorts(){
			type = type != null? type : typeof(SharpEvent);
			eventInput = AddValueInput("Event", type);
			if (type == typeof(SharpEvent)){
				return;
			}

			var delegateType = type.RTGetGenericArguments()[0];
			if (reflectedEvent == null){
				reflectedEvent = new ReflectedDelegateEvent(delegateType);
			}
			var parameters = delegateType.RTGetDelegateTypeParameters();
			for (var _i = 0; _i < parameters.Length; _i++){
				var i = _i;
				var parameter = parameters[i];
				AddValueOutput(parameter.Name, "arg" + i, parameter.ParameterType, ()=>{ return args[i]; });
			}

			flowCallback = AddFlowOutput("Callback");
			if (!autoHandleRegistration){
				AddFlowInput("Register", Register, "Add");
				AddFlowInput("Unregister", Unregister, "Remove");
			}
		}

		void Register(Flow f){
			var sharpEvent = eventInput.value as SharpEvent;
			if (sharpEvent != null){
				sharpEvent.StopListening(reflectedEvent, Callback);
				sharpEvent.StartListening(reflectedEvent, Callback);
			}
		}

		void Unregister(Flow f){
			var sharpEvent = eventInput.value as SharpEvent;
			if (sharpEvent != null){
				sharpEvent.StopListening(reflectedEvent, Callback);
			}
		}

		void Callback(params object[] args){
			this.args = args;
			flowCallback.Call(new Flow());
		}

		public override Type GetNodeWildDefinitionType(){
			return typeof(SharpEvent);
		}

		public override void OnPortConnected(Port port, Port otherPort){
			if (port == eventInput){
				type = otherPort.type;
				GatherPorts();
			}
		}

		///----------------------------------------------------------------------------------------------
		///---------------------------------------UNITY EDITOR-------------------------------------------
		#if UNITY_EDITOR
		
		protected override void OnNodeInspectorGUI(){
			var content = new GUIContent("Auto Handle Registration", "If enabled, registration will be handled on graph Enable/Disable automatically");
			autoHandleRegistration = UnityEditor.EditorGUILayout.Toggle(content, autoHandleRegistration);
			base.OnNodeInspectorGUI();
		}
		
		#endif
		///----------------------------------------------------------------------------------------------

	}

}