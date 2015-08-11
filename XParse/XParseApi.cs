using System;
using Parse;
using System.Threading.Tasks;

namespace XParse
{
	public class XParseApi
	{
		public XParseApi ()
		{
		}

		public static async Task SaveAsync ()
		{
			var obj = new ParseObject ("Note");
			obj ["name"] = "Jackson";
			await obj.SaveAsync ();
		}
	}
}

