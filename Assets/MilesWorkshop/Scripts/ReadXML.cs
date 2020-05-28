using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEditor;

public class ReadXML : MonoBehaviour {
    string xmlPath = "Assets/MilesWorkshop/data.xml";
    XmlSerializer xml = new XmlSerializer(typeof(EditEffectList));
    private void Update() {

        
    }

    private void Start() {
        ReadXMLFile(xmlPath); 
    }
    void ReadXMLFile(string xmlPath) {
        FileStream stream = new FileStream(xmlPath, FileMode.Open);

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.DtdProcessing = DtdProcessing.Parse;
        XmlReader reader = XmlReader.Create(stream, settings);

        //reader.MoveToContent();
        //reader.MoveToFirstAttribute();
        while(reader.Read()) {
            switch(reader.NodeType) {
                case XmlNodeType.Element:
                    //Debug.Log(reader.Name);
                    if(reader.Name == "delayTime") {
                        Debug.Log(reader.GetAttribute("delayTime"));
                    }
                    if(reader.Name == "path") {
                        Debug.Log(reader.GetAttribute("path"));
                    }

                    //Debug.Log(reader.GetAttribute(reader.Name));
                    break;
            }
        }
        stream.Close();
    }

}

[System.Serializable]
public class EditEffectList {

}
