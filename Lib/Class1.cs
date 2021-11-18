namespace Lib
{
	public static class Class1
	{
		public static string Method1()
		{
			string x = "xxx";
			string y = "yyy";
			string xy = x
				+
				y
				+
				"zz"
				;
			xy.ToString();

			foreach (var a in xy)
			{
				a.ToString();
			}

			foreach (var a in x)
			{
				a.ToString();
			}

			for (var c = 0; c < xy.Length; c++)
			{
				xy[c].ToString();
			}

			if (x.Length > y.Length)
			{
				"x".ToString();
			}
			else if (y.Length > x.Length)
			{
				"y".ToString();
			}
			else
			{
				"z".ToString();
			}

			switch (x)
			{
				case "x":
					x.ToString();
					break;

				case "y":
					y.ToString();
					break;

				default:
					xy.ToString();
					break;
			}

			return xy.ToString();
		}
	}
}