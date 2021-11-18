using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Lib.Test
{
	public class UnitTest1
	{
		[Test]
		public void _00_Instrument()
		{
			FineCover.Instrumentation.Instrument("Lib.dll", "Lib.dll.FineCover.dll", "Lib.dll.FineCover.txt");
		}

		[Test]
		public void _01_Run()
		{
			Assembly.LoadFrom("Lib.dll.FineCover.dll").GetType("Lib.Class1").GetMethod("Method1").Invoke(null, null);
		}
	}
}