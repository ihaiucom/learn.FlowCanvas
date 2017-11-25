using System;
using UnityEngine;
using UnityEngine.Events;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using ParadoxNotion.Serialization;

namespace FlowCanvas.Nodes{

	[Name("Unity Event Callback", 3)]
	[Category("Events/Custom")]
	[Description("Register a callback on a UnityEvent.\nWhen that event is raised, this node will get called.")]
	[ContextDefinedInputs(typeof(UnityEventBase))]
	public class UnityEventCallbackEvent : EventNode {

		[SerializeField]
		private bool _autoHandleRegistration;
		
		[SerializeField]
		private SerializedTypeInfo _type;

		private Type eventType{
			get {return _type != null? _type.Get() : null;}
			set
			{
				if (_type == null || _type.Get() != value){
					_type = new SerializedTypeInfo(value);
				}
			}
		}

		private object[] argValues;
		private ValueInput eventInput;
		private FlowOutput callback;
		private ReflectedUnityEvent reflectedEvent;

		public bool autoHandleRegistration{
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
				var unityEvent = eventInput.value as UnityEventBase;
				if (unityEvent != null){
					reflectedEvent.StartListening( unityEvent, OnEventRaised );
				}
			}
		}

		public override void OnGraphStoped(){
			if (autoHandleRegistration){
				var unityEvent = eventInput.value as UnityEventBase;
				if (unityEvent != null){
					reflectedEvent.StopListening( unityEvent, OnEventRaised );
				}
			}
		}

		protected override void RegisterPorts(){
			eventType = eventType != null? eventType : typeof(UnityEventBase);
			eventInput = AddValueInput("Event", eventType);
			if (eventType == typeof(UnityEventBase)){
				return;
			}

			if (reflectedEvent == null){
				reflectedEvent = new ReflectedUnityEvent(eventType);
			}
			if (reflectedEvent.eventType != eventType){
				reflectedEvent.InitForEventType(eventType);
			}

			argValues = new object[reflectedEvent.parameters.Length];
			for (var _i = 0; _i < reflectedEvent.parameters.Length; _i++){
				var i = _i;
				var parameter = reflectedEvent.parameters[i];
				AddValueOutput(parameter.Name, "arg" + i, parameter.ParameterType, ()=> { return argValues[i]; });
			}

			callback = AddFlowOutput("Callback");
			if (!autoHandleRegistration){
				AddFlowInput("Register", Register, "Add");
				AddFlowInput("Unregister", Unregister, "Remove");
			}
		}

		void Register(Flow f){
			var unityEvent = eventInput.value as UnityEventBase;
			if (unityEvent != null){
				reflectedEvent.StopListening( unityEvent, OnEventRaised );
				reflectedEvent.StartListening( unityEvent, OnEventRaised );
			}
		}

		void Unregister(Flow f){
			var unityEvent = eventInput.value as UnityEventBase;
			if (unityEvent != null){
				reflectedEvent.StopListening( unityEvent, OnEventRaised );
			}
		}

		void OnEventRaised(params object[] args){
			this.argValues = args;
			callback.Call(new Flow());
		}

		public override System.Type GetNodeWildDefinitionType(){
			return typeof(UnityEventBase);
		}

		public override void OnPortConnected(Port port, Port otherPort){
			if (port == eventInput && otherPort.type.RTIsSubclassOf(typeof(UnityEventBase)) ){
				eventType = otherPort.type;
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