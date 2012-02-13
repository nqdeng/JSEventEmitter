/**
 * JS Event Emitter
 * Copyright (c) 2012 Alibaba.Com, Inc.
 * MIT Licensed
 * @author Nanqiao Deng
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Alibaba.F2E.Util
{
	/**
	 * A event handler delegate supporting to pass custom data object to event handlers.
	 */
	public delegate void JsEventHandler(Object sender, JsEventArgs e);
	
	/**
	 * A class to add custom event support to JScript.NET.
	 */
	public class JsEventEmitter
	{
		/**
		 * <Type - <Owner - Handler>> mapping table.
		 */
		private Dictionary<string, Dictionary<Object, JsEventHandler>> eventTable;
		
		/**
		 * A object to represent the null owner.
		 */
		private Object NULL;
		
		/**
		 * Create a JsEventEmitter instance.
		 */
		protected JsEventEmitter()
		{
			this.eventTable = new Dictionary<string, Dictionary<Object, JsEventHandler>>();
			this.NULL = new Object();
		}
		
		/**
		 * Add handler for a specific event.
		 * @param type Event type.
		 * @param fn Event handler function.
		 */
		public void AddHandler(string type, JsEventHandler fn)
		{
			// The object which owns the handler function.
			Object owner = fn.Target;
			
			// Dictionary key defaults to NULL if no owner.
			Object key = (owner == null ? this.NULL : owner);
			
			// <Owner - Handler> dictionary.
			Dictionary<Object, JsEventHandler> dic;
			
			// The exist event handler binding to the specific type and owner.
			JsEventHandler handler;
			
			// Create a new <Owner - Handler> dictionary for the first used event.
			if (!this.eventTable.TryGetValue(type, out dic))
			{
				dic = new Dictionary<Object, JsEventHandler>();
				this.eventTable.Add(type, dic);
			}
			
			// Add or append the event handler function.
			if (dic.TryGetValue(key, out handler))
			{
				dic[key] = handler + fn;
			}
			else
			{
				dic.Add(key, fn);
			}
		}
		
		/**
		 * Remove handler of a specific event type.
		 * @param type Event type.
		 * @param fn Event handler function.
		 */
		public void RemoveHandler(string type, JsEventHandler fn)
		{
			// The object which owns the event handler function.
			Object owner = fn.Target;
			
			// Dictionary key defaults to NULL if no owner.
			Object key = (owner == null ? this.NULL : owner);
			
			// <Owner - Handler> dictionary.
			Dictionary<Object, JsEventHandler> dic;
			
			// The exist event handler binding to the specific type and owner.
			JsEventHandler handler;
			
			// Only remove the event handler which matches the type and handler function.
			if (this.eventTable.TryGetValue(type, out dic) && dic.TryGetValue(key, out handler))
			{
				dic[key] = handler - fn;
				// Remove owner from dictionary if it owns nothing.
				if (dic[key] == null)
				{
					dic.Remove(key);
				}
			}
		}
		
		/**
		 * Remove all handlers of a specific event type.
		 * @param type Event type.
		 */
		public void RemoveAllHandlers(string type)
		{
			// <Owner - Handler> dictionary.
			Dictionary<Object, JsEventHandler> dic;
			
			// Clear dictionary of the specific type.
			if (this.eventTable.TryGetValue(type, out dic))
			{
				dic.Clear();
			}
		}
		
		/**
		 * Emit a special event with custom data object.
		 * @param type Event type.
		 * @param data Custom data object passing to the handler function.
		 */
		protected void Emit(string type, Object data)
		{
			// <Owner - Handler> dictionary.
			Dictionary<Object, JsEventHandler> dic;
			
			// Iterate event table to fire every event handler.
			if (this.eventTable.TryGetValue(type, out dic))
			{
				// Copy owners from dictionary to a new array
				// to enable modify dictionary during iteration.
				Object[] owners = new Object[dic.Count];
				dic.Keys.CopyTo(owners, 0);
				
				for (int i = 0; i < owners.Length; ++i)
				{
					// Make thread-safe call to event handler function
					// which is owned by System.Windows.Forms.Control object.
					if (owners[i] is Control)
					{
						Control ctrl = (Control)owners[i];
						if (ctrl.InvokeRequired)
						{
							ctrl.Invoke(dic[owners[i]], new Object[] { this, new JsEventArgs(type, data) });
							continue;
						}
					}
					
					// Call event handler function directly if no invoke needed.
					dic[owners[i]](this, new JsEventArgs(type, data));
				}
			}
		}
		
		/**
		 * Emit a special event without custom data object.
		 * @param type Event type.
		 */
		protected void Emit(string type)
		{
			Emit(type, EventArgs.Empty);
		}
	}
	
	/**
	 * A class to support passing custom data object to event handlers.
	 */
	public class JsEventArgs : EventArgs
	{
		/**
		 * Event type.
		 */
		private string type;
		
		/**
		 * Custom data object.
		 */
		private Object data;
		
		/**
		 * Create a new EventArgsEx instance.
		 * @param type Event type.
		 * @param data Custom data object.
		 */
		public JsEventArgs(string type, object data)
			: base()
		{
			this.type = type;
			this.data = data;
		}
		
		/**
		 * Readonly property for event type.
		 */
		public string Type
		{
			get { return this.type; }
		}
		
		/**
		 * Readonly property for custom data object.
		 */
		public Object Data
		{
			get { return this.data; }
		}
	}
}