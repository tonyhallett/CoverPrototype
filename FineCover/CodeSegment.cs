using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace FineCover
{
	public class CodeSegment
	{
		public int StartLine { get; set; }
		public int EndLine { get; set; }
		public Instruction[] Instructions { get; set; }
		public string FilePath { get; set; }
	}
}
