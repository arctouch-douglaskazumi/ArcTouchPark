using System;
using System.Linq;

using Parse;
using System.Reflection;

namespace XParse
{
	public class Parseable
	{
		public Parseable ()
		{
		}

		public string ObjectId {
			get;
			set;
		}

		public ParseObject ToParse ()
		{
			var thisType = this.GetType ();
			ParseObject parseObj = new ParseObject (thisType.Name);

			foreach (PropertyInfo prop in thisType.GetRuntimeProperties ()) {
				parseObj [prop.Name] = prop.GetValue (this);
			}

			return parseObj;
		}

		public void LoadFromParse (ParseObject parseObj)
		{
			
			var thisType = this.GetType ();

			foreach (PropertyInfo prop in thisType.GetRuntimeProperties ()) {
				if (prop.Name != "ObjectId") {
					prop.SetValue (this, parseObj.Get<object> (prop.Name));
				}
			}

			this.ObjectId = parseObj.ObjectId;
		}
	}
}

