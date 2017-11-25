using System.Reflection;
using ParadoxNotion;

namespace FlowCanvas.Nodes
{
    //Encapsulation of EventInfo
	abstract public class SharpEvent {
		public object instance;
		public EventInfo eventInfo;
		
		public static SharpEvent Create(EventInfo eventInfo){
			if (eventInfo == null){ return null; }
			var wrapper = (SharpEvent)typeof(SharpEvent<>).RTMakeGenericType(eventInfo.EventHandlerType).CreateObject();
			wrapper.eventInfo = eventInfo;
			return wrapper;
		}

		//helper
		public void StartListening(ReflectedDelegateEvent reflectedEvent, ReflectedDelegateEvent.DelegateEventCallback callback){
			if (reflectedEvent == null || callback == null){ return; }
			reflectedEvent.Add(callback);
			eventInfo.AddEventHandler(instance, reflectedEvent.AsDelegate());
		}

		//helper
		public void StopListening(ReflectedDelegateEvent reflectedEvent, ReflectedDelegateEvent.DelegateEventCallback callback){
			if (reflectedEvent == null || callback == null){ return; }
			reflectedEvent.Remove(callback);
			eventInfo.RemoveEventHandler(instance, reflectedEvent.AsDelegate());
		}
	}

	//typeof(T) is the event handler type, so that it can be accessed without having an instance of SharpEvent
	public class SharpEvent<T> : SharpEvent {
		
	}

}