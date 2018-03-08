﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ClassCalculater
{
    /// <summary>
    /// Class for managing xml input and output
    /// </summary>
    public class IOManager
    {
        #region Internal Fields
        /// <summary>
        /// This is the current version of how serializing to XML is handled
        /// within the program. This is NOT the xml version number refering to
        /// the language revisions.
        /// </summary>
        internal static string XMLVersion = "1.0";
        #endregion

        #region Private fields

        private const string ASSIGNMENT_NAME_TAG = "Name";
        private const string GRADE_TAG = "Grade";
        private const string WEIGHT_TAG = "Weight";

        /// <summary>
        /// The path to the save directory for files
        /// </summary>
        private string path;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public IOManager()
        {
            MessageBox.Show("Please select folder for saving and loading grade files.");
            FolderBrowserDialog folderBrowDiag = new FolderBrowserDialog();

            // Shows the folder browser and waits, will execute if statement
            // only if the user presses the okay button
            if (folderBrowDiag.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowDiag.SelectedPath;
            }
        }

        /// <summary>
        /// Will write all data from list of AssignmentInput objects to xml file
        /// </summary>
        /// <param name="boxes"></param>
        public void WriteToFile(List<AssignmentInput> boxes, string fileName)
        {

            if (false == File.Exists(fileName))
            {
                // Create new xml document at path
                XmlWriter writer = XmlWriter.Create(path + @"\" + fileName);
                writer.WriteStartDocument();

                writer.WriteStartElement("Assignments");

                for (int i = 0; i < boxes.Count; i++)
                {
                    writer.WriteStartElement("assignment_number_" + i);
                    writer.WriteAttributeString(ASSIGNMENT_NAME_TAG, boxes[i].AssignmentName);
                    writer.WriteAttributeString(GRADE_TAG, boxes[i].AssignemntGrade.ToString(CultureInfo.CurrentCulture));
                    writer.WriteAttributeString(WEIGHT_TAG, boxes[i].AssignmentWeight.ToString(CultureInfo.CurrentCulture));
                    writer.WriteEndElement();
                }

                writer.WriteEndDocument();
                writer.Close();

            }
            else
            {
                FileInfo f = new FileInfo(fileName);
                MessageBox.Show("ERROR: XML file with name " + fileName + " already exists. At path: " + f.FullName);
            }
        }

        /// <summary>
        /// Prompts the user to find and select a file, then opens that file 
        /// and reads the data into a list, which it then assigns to the refferenced list.
        /// </summary>
        /// <returns></returns>
        public void ReadFromFile(ref List<AssignmentInput> list)
        {
            OpenFileDialog fileDiaglogue = new OpenFileDialog();

            DialogResult result = fileDiaglogue.ShowDialog();

            if (DialogResult.OK == result)
            {
                List<AssignmentInput> tmpLst = new List<AssignmentInput>();

                string file = fileDiaglogue.FileName;
                XmlReader reader = XmlReader.Create(file);

                int i = 0;
                int j = 0;

                while (reader.Read())
                {
                    if(2 < j)
                    {
                        tmpLst.Add(new AssignmentInput());
                        i += 1;
                    }
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            break;
                        case XmlNodeType.Attribute:
                            switch (reader.Value)
                            {
                                case ASSIGNMENT_NAME_TAG:
                                    j += 1;
                                    tmpLst[i].AssignmentName = reader.Value;
                                    break;
                                case GRADE_TAG:
                                    j += 1;
                                    tmpLst[i].AssignemntGrade = int.Parse(reader.Value);
                                    break;
                                case WEIGHT_TAG:
                                    j += 1;
                                    tmpLst[i].AssignmentWeight = int.Parse(reader.Value);
                                    break;
                                default:
                                    MessageBox.Show("Error occured in nested switch");
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }

                list = tmpLst;
            }  
        }

        /// <summary>
        /// Opens the file dialog, and if the user clicks okay after selecting a file
        /// returns the name of that file and its path.
        /// </summary>
        /// <returns></returns>
        public void OpenDialogue(out string path, out string fileName)
        {
            throw new NotImplementedException();
        }
    }
}