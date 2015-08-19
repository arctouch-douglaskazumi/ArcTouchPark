using System;
using System.Threading.Tasks;

using Parse;
using System.Collections.Generic;

namespace ArcTouchPark
{
	public class ParseApi
	{
		public ParseApi ()
		{
		}

		public static async Task<string> SaveAsync (Parseable obj)
		{
			ParseObject parseObj = obj.ToParse ();
			await parseObj.SaveAsync ();

			return parseObj.ObjectId;
		}

		public static async Task<T> GetAsync<T> (string objectId) where T: Parseable
		{
			Type objectType = typeof(T);

			var query = ParseObject.GetQuery (objectType.Name).WhereEqualTo (Const.OBJECT_ID, objectId);
			ParseObject parseObject = await query.FirstOrDefaultAsync ();
			if (parseObject != null) {
				var obj = (T)Activator.CreateInstance (objectType);
				obj.LoadFromParse (parseObject);
				return obj;
			}

			return default(T);
		}

		public static async Task<List<T>> GetAllAsync<T> () where T: Parseable
		{
			Type objectType = typeof(T);
			List<T> objList = new List<T> ();

			var query = ParseObject.GetQuery (objectType.Name);
			var parseObjectList = await query.FindAsync ();
			foreach (var parseObject in parseObjectList) {
				var obj = (T)Activator.CreateInstance (objectType);
				obj.LoadFromParse (parseObject);
				objList.Add (obj);
			}

			return objList;
		}
	}
}

