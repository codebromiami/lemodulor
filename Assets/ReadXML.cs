using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//http://xmltocsharp.azurewebsites.net use this to get an object graph to build your c# class

public class ReadXML : MonoBehaviour
{
     
    XmlDocument ParseXmlFile(TextAsset xmlRawFile){
        string xmlData = xmlRawFile.text;        
        XmlDocument xmlDoc = new XmlDocument ();
        xmlDoc.Load (new StringReader (xmlData));
        return xmlDoc;
    }

    XmlDocument ParseXmlFile(string xmlData){
        
        XmlDocument xmlDoc = new XmlDocument ();
        xmlDoc.Load (new StringReader (xmlData));
        return xmlDoc;
    }

    public static void Serialize(object item, string path)
	{
		XmlSerializer serializer = new XmlSerializer(item.GetType());
		StreamWriter writer = new StreamWriter(path);
		serializer.Serialize(writer.BaseStream, item);
		writer.Close();
	}
 
	public static T Deserialize<T>(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		StreamReader reader = new StreamReader(path);
		T deserialized = (T)serializer.Deserialize(reader.BaseStream);
		reader.Close();
		return deserialized;
	}
}

