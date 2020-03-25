using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace API {
	
	public class JSONData  {

		private readonly Dictionary<string, object> m_data;
		private readonly bool m_isValid = false;
        private string m_rawData = string.Empty;

        public JSONData(string data) {
			try {
				m_data =   SA.Common.Data.Json.Deserialize(data) as Dictionary<string, object>;
				m_rawData = data;
				m_isValid = true;
			} catch(System.Exception e) {
			    Debug.LogError ("Can't parse JSONData. " + e.Message);
			}
		}

		public JSONData(object data) {
			try {
				m_data = (Dictionary<string, object>) data;
				m_isValid = true;
			} catch(System.Exception e) {
                Debug.LogError("Can't parse JSONData. " + e.Message);
            }
        }

        public T TryGetValue<T>(string key) {
            if (HasValue(key)) {
                return GetValue<T>(key);
            }
            else {
                Debug.LogWarningFormat("Key {0} not found in JSONData", key);
                return default(T);
            }
        }

        public JSONData GetNestedObject(string key) {
            if (HasValue(key)) {
                return new JSONData(GetValue<Dictionary<string, object>>(key));
            }
            else {
                throw new ArgumentException(string.Format("Cannot get child with key {0}", key));
            }
        }

        public List<JSONData> GetNestedObjectsList(string key) {
            if (HasValue(key)) {
                var list = GetValue<List<object>>(key);
                return list.Select(jd => new JSONData(jd)).ToList();
            }
            else {
                throw new ArgumentException(string.Format("Cannot get child with key {0}", key));
            }
        }

        public List<JSONData> GetNestedOrEmptyList(string key) {
            if (HasValue(key)) {
                var list = GetValue<List<object>>(key);
                return list.Select(jd => new JSONData(jd)).ToList();
            }
            else {
                Debug.LogWarningFormat("Key {0} not found in JSONData", key);
                return new List<JSONData>();
            }
        }


        public bool HasValue(params string[] keys) { 

			Dictionary<string, object> dict = m_data;
			for(int i = 0; i < keys.Length - 1; i++) {
				dict = (Dictionary<string, object>) dict[keys[i]];
			}
            string valueKey = keys[keys.Length - 1];
            if(dict.ContainsKey(valueKey)) {
				return dict[valueKey] != null;
			} else {
				return false;
			}
		}

		public T GetValue<T>(params string[] keys) {

			var value = default(T);
			var dict = m_data;
			for(int i = 0; i < keys.Length - 1; i++) {
				dict = (Dictionary<string, object>) dict[keys[i]];
			}


			string valueKey = keys[keys.Length - 1];
			object data = dict[valueKey];

			if(typeof (T) == typeof( DateTime)) {
				string dateString = Convert.ToString(data);
				DateTime date;
				bool parsed = SA.Common.Util.General.TryParseRfc3339(dateString, out date);
				if(!parsed) {
				//	U.LogWarning("Date Parsing failed: " + dateString);
				}

				value = (T)Convert.ChangeType (date, typeof(T));

			} else {
				value = (T)Convert.ChangeType (data, typeof(T));
			}
			return value;
		}

		public string RawData {
			get {
                if(string.IsNullOrEmpty(m_rawData)) {
                    m_rawData = SA.Common.Data.Json.Serialize(m_data);
                }
				return m_rawData;
			}
		}

		public Dictionary<string, object> Data  {
			get {
				return m_data;
			}
		}

		public bool IsValid {
			get {
				return m_isValid;
			}
		}
	}
}
