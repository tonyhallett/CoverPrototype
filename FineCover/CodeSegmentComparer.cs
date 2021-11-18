using System;
using System.Collections.Generic;
using System.Text;

namespace FineCover
{
	public class CodeSegmentComparer : EqualityComparer<CodeSegment>
	{
		public override bool Equals(CodeSegment x, CodeSegment y)
		{
			return Id(x).Equals(Id(y));
		}

		public override int GetHashCode(CodeSegment obj)
		{
			return obj == null ? 0 : Id(obj).GetHashCode();
		}

		private static string Id(CodeSegment codeSegment)
		{
			return $"{codeSegment?.FilePath}:{codeSegment?.EndLine}".ToLower();
		}
	}
}
