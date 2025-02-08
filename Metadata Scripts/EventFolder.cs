﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

public class EventFolder
{
    // used in main
    public static List<string> AllEvents = new List<string> { };

    public static void ExtractEventFolders(string filePath) 
    {
        // figure out all subfolders from an event path and make an XML for each subfolder
        // aka event:/music/soundtest/pause
        //            ^folder ^folder  ^event (ignore event)

        // This HashSet will track which folders have already been processed
        HashSet<string> processedFolders = new HashSet<string>();

        foreach (var path in AllEvents)
        {
            // Split the path by "/"
            var pathParts = path.Split('/');

            // Get the subfolders
            var folders = new List<string>(pathParts);
            folders.RemoveAt(0); // Remove "event:"
            folders.RemoveAt(folders.Count - 1); // Remove the last part (it's not a folder)

            // goes through every subfolder in cleansed path (ex: /music and /soundtest)
            foreach (var folder in folders)
            {
                if (!processedFolders.Contains(folder))
                {
                    // Create XML
                    CreateXmlFile(filePath, folder);

                    // Mark this folder as processed
                    processedFolders.Add(folder);
                }
            }
        }
    }
    // Honestly I should have done this for AudioFile.cs ngl
    static void CreateXmlFile(string filePath, string folderName)
    {
        string TEMP_GUID = "{00000000-0000-0000-0000-000000000000}";

        // Create XML document
        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
        xmlDoc.AppendChild(declaration);

        // Create the root element
        XmlElement root = xmlDoc.CreateElement("objects");
        root.SetAttribute("serializationModel", "Studio.02.02.00");
        xmlDoc.AppendChild(root);

        // Create object element
        XmlElement objectElement = xmlDoc.CreateElement("object");
        objectElement.SetAttribute("class", "EventFolder");
        objectElement.SetAttribute("id", TEMP_GUID);                                // TEMP
        root.AppendChild(objectElement);

        // Create property element for the name
        XmlElement propertyElement = xmlDoc.CreateElement("property");
        propertyElement.SetAttribute("name", "name");
        XmlElement valueElement = xmlDoc.CreateElement("value");
        valueElement.InnerText = folderName;
        propertyElement.AppendChild(valueElement);
        objectElement.AppendChild(propertyElement);

        // GUID of the folder above it (TEMP IMPLIMENTATION)
        XmlElement relationshipElement = xmlDoc.CreateElement("relationship");
        relationshipElement.SetAttribute("name", "folder");
        XmlElement destinationElement = xmlDoc.CreateElement("destination");
        destinationElement.InnerText = TEMP_GUID;                                   // TEMP
        relationshipElement.AppendChild(destinationElement);
        objectElement.AppendChild(relationshipElement);

        // Save the XML document to a file (TEMP NAMING)
        xmlDoc.Save(filePath + "/" + folderName + ".xml");
    }
}
