using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
namespace SpinVeriff.LTLSimpleParser
{
	public class ThreadManager
	{
		
		class AParam
		{
			//public Thread thread;
			public AutoResetEvent ar;
			public EventQueue events;
		}
		
		/// <summary>
		/// Создаем два потока классов AStarter.
		/// </summary>
		public void StartWork()
		{
			Thread st1 = new Thread(st1Thread);
			AutoResetEvent sta1Ar = new AutoResetEvent(false);
			st1Events = new EventQueue(st1, sta1Ar);
			AParam st1p = new AParam { ar = sta1Ar, events = st1Events };
			st1.Start(st1p);
			
			while (true)
			{
				var s = Console.In.ReadLine();
				st1Events.PushEvent(s);
			}
		}
		
		private EventQueue st1Events;
		public void st1Thread(Object param)
		{
			AParam aparam = param as AParam;
			AStarter st1 = new AStarter();
			while (true)
			{
				if (aparam.events.IsEmpty())
				{
					//Уснуть.
					Console.Out.WriteLine("Going to sleep!");
					aparam.ar.WaitOne();
				}
				string evt = aparam.events.PopEvent();
				Console.Out.WriteLine("event handled: {0}!", evt);
				switch (evt)
				{
					case "e1":
					Console.Out.WriteLine("Next state!");
					break;
					default:
					Console.Out.WriteLine("Fail!");
					break;
				}
			}
		}
	}
}
}
}
