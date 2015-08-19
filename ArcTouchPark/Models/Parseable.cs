using System;
using System.Linq;

using Parse;
using System.Reflection;

namespace ArcTouchPark
{
	public class Parseable
	{
		public Parseable ()
		{
		}

		public string objectId {
			get;
			set;
		}

		public ParseObject ToParse ()
		{
			var thisType = this.GetType ();
			ParseObject parseObj = new ParseObject (thisType.Name);

			foreach (PropertyInfo prop in thisType.GetRuntimeProperties ()) {
				if (!prop.Name.Equals (Const.OBJECT_ID, StringComparison.OrdinalIgnoreCase)) {
					parseObj [prop.Name] = prop.GetValue (this);
				}
			}

			if (!string.IsNullOrWhiteSpace (this.objectId)) {
				parseObj.ObjectId = this.objectId;
			}

			return parseObj;
		}

		public void LoadFromParse (ParseObject parseObj)
		{
			
			var thisType = this.GetType ();

			foreach (PropertyInfo prop in thisType.GetRuntimeProperties ()) {
				if (!prop.Name.Equals (Const.OBJECT_ID, StringComparison.OrdinalIgnoreCase)) {
					object value = null;
					if (parseObj.ContainsKey (prop.Name)) {
						value = parseObj.Get<object> (prop.Name);
					}
					prop.SetValue (this, value);
				}
			}

			this.objectId = parseObj.ObjectId;
		}
	}
}

