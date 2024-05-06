using System;
using System.Collections.Generic;
using System.Threading;

namespace SpinVeriff.LTLSimpleParser
{
	class EventQueue
	{
		public EventQueue(Thread m, AutoResetEvent ar)
		{
			machineThread = m;
			events = new Queue<string>();
			machineAr = ar;
		}
		
		private Thread machineThread;
		private Queue<string> events;
		private static object qLock = new object();
		private AutoResetEvent machineAr;
		
		public void PushEvent(string evt)
		{
			lock (this)
			{
				events.Enqueue(evt);
				machineAr.Set();
			}
		}
		
		public bool IsEmpty()
		{
			lock (this)
			{
				return events.Count == 0;
			}
		}
		
		public string PeekEvent()
		{
			lock (this)
			{
				string res = events.Peek();
				return res;
			}
		}
		
		public string PopEvent()
		{
			lock (this)
			{
				string res = events.Peek();
				events.Dequeue();
				return res;
			}
		}
	}
}
