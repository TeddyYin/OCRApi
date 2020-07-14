using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace OCRApi.Models
{
    [DataContract]
    public class RecognizeSetting : INotifyPropertyChanged
    {
        int _RecogLang = 0;
        [DataMember]
        public int RecogLang
        {
            get { return _RecogLang; }
            set { _RecogLang = value; Notify("RecogLang"); }
        }

        bool _RecogCH = true;
        [DataMember]
        public bool RecogCH
        {
            get { return _RecogCH; }
            set { _RecogCH = value; Notify("RecogCH"); }
        }

        bool _RecogENG = true;
        [DataMember]
        public bool RecogENG
        {
            get { return _RecogENG; }
            set { _RecogENG = value; Notify("RecogENG"); }
        }

        bool _RecogNUM = true;
        [DataMember]
        public bool RecogNUM
        {
            get { return _RecogNUM; }
            set { _RecogNUM = value; Notify("RecogNUM"); }
        }

        bool _RecogSYM = true;
        [DataMember]
        public bool RecogSYM
        {
            get { return _RecogSYM; }
            set { _RecogSYM = value; Notify("RecogSYM"); }
        }

        bool _RecogHEX = true;
        [DataMember]
        public bool RecogHEX
        {
            get { return _RecogHEX; }
            set { _RecogHEX = value; Notify("RecogHEX"); }
        }

        bool _RecogSChar = true;
        [DataMember]
        public bool RecogSChar
        {
            get { return _RecogSChar; }
            set { _RecogSChar = value; Notify("RecogSChar"); }
        }

        bool _RecogHChar = true;
        [DataMember]
        public bool RecogHChar
        {
            get { return _RecogHChar; }
            set { _RecogHChar = value; Notify("RecogHChar"); }
        }

        bool _RecogKChar = true;
        [DataMember]
        public bool RecogKChar
        {
            get { return _RecogKChar; }
            set { _RecogKChar = value; Notify("RecogKChar"); }
        }

        bool _CHConvert = true;
        [DataMember]
        public bool CHConvert
        {
            get { return _CHConvert; }
            set { _CHConvert = value; Notify("CHConvert"); }
        }

        int _RecogOrientation = 0;
        [DataMember]
        public int RecogOrientation
        {
            get { return _RecogOrientation; }
            set { _RecogOrientation = value; Notify("RecogOrientation"); }
        }

        int _RecogDegree = 0;
        [DataMember]
        public int RecogDegree
        {
            get { return _RecogDegree; }
            set { _RecogDegree = value; Notify("RecogDegree"); }
        }

        bool _RecogAuto = true;
        [DataMember]
        public bool RecogAuto
        {
            get { return _RecogAuto; }
            set { _RecogAuto = value; Notify("RecogAuto"); }
        }

        int _RegWndLang = 0;
        [DataMember]
        public int RegWndLang
        {
            get { return _RegWndLang; }
            set { _RegWndLang = value; Notify("RegWndLang"); }
        }

        int _RegRegionMode = 0;
        [DataMember]
        public int RegRegionMode
        {
            get { return _RegRegionMode; }
            set { _RegRegionMode = value; Notify("RegRegionMode"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public static bool IsFirstOpen()
        {
            return true;
        }

        /*public static void Save(RecognizeSetting config)
        {
        }*/

        /*public static void Load(AllConfig target)
        {
            string userSettingPath = PPPathManager.UserDataFolder + "\\Setting.dat";
            try
            {
                AllConfig objRead;
                using (StreamReader sr = File.OpenText(userSettingPath))
                {
                    string json = sr.ReadToEnd();
                    objRead = AllConfig.DeserializeFromXML<AllConfig>(json);
                    AllConfig.CopyProperties<AllConfig>(objRead, target);
                }
                //return objRead;
            }
            catch (System.IO.FileNotFoundException)
            {
                //return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }*/

        public static string SerializeToXML<T>(T obj)
        {
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            string retVal = "";
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    serializer.WriteObject(ms, obj);
                }
                catch (SerializationException)
                {
                    return "";
                }
                catch (InvalidDataContractException)
                {
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
                retVal = Encoding.UTF8.GetString(ms.ToArray());
            }
            return retVal;
        }

        public static T DeserializeFromXML<T>(string xmlString)
        {
            if (xmlString.Length == 0) return default(T);
            T obj = Activator.CreateInstance<T>();
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                try
                {
                    obj = (T)serializer.ReadObject(ms);
                }
                catch (System.Runtime.Serialization.SerializationException serExc)
                {
                    return default(T);
                }
            }
            return obj;
        }

        public static void CopyProperties<T>(T source, T destination)
        {
            PropertyInfo[] fromFields = null;
            fromFields = typeof(T).GetProperties();
            RecognizeSetting.PropertyHandler.SetProperties(false, fromFields, source, destination);
        }

        public class PropertyHandler
        {
            #region Set Properties
            public static void SetProperties(PropertyInfo[] fromFields,
                                             PropertyInfo[] toFields,
                                             object fromRecord,
                                             object toRecord)
            {
                PropertyInfo fromField = null;
                PropertyInfo toField = null;

                try
                {

                    if (fromFields == null)
                    {
                        return;
                    }
                    if (toFields == null)
                    {
                        return;
                    }

                    for (int f = 0; f < fromFields.Length; f++)
                    {
                        //MyTracer.WriteLine(fromFields[f].Name);
                        fromField = (PropertyInfo)fromFields[f];

                        for (int t = 0; t < toFields.Length; t++)
                        {

                            toField = (PropertyInfo)toFields[t];

                            if (fromField.Name != toField.Name)
                            {
                                continue;
                            }

                            toField.SetValue(toRecord,
                                                fromField.GetValue(fromRecord, null),
                                                null);

                            break;

                        }

                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            #endregion

            #region Set Properties
            public static void SetProperties(bool bOnlyCopyNonNullValue, PropertyInfo[] fromFields,
                                             object fromRecord,
                                             object toRecord)
            {
                PropertyInfo fromField = null;

                try
                {

                    if (fromFields == null)
                    {
                        return;
                    }

                    for (int f = 0; f < fromFields.Length; f++)
                    {
                        fromField = (PropertyInfo)fromFields[f];
                        if (!bOnlyCopyNonNullValue)
                            fromField.SetValue(toRecord,
                                               fromField.GetValue(fromRecord, null),
                                               null);
                        else
                        {
                            object objGet = fromField.GetValue(fromRecord, null);
                            Type t = fromField.PropertyType;
                            Type t2 = typeof(int);
                            if (fromField.PropertyType == typeof(Int32))
                            {
                                if ((int)objGet == 0) continue;
                            }
                            if (fromField.PropertyType == typeof(object))
                            {
                                if (objGet == null) continue;
                            }

                            fromField.SetValue(toRecord,
                                fromField.GetValue(fromRecord, null),
                                null);
                        }
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            #endregion
        }
    }
}