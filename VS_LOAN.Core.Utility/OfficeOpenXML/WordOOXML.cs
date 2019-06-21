using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VS_LOAN.Core.Utility.OfficeOpenXML
{
    public class WordOOXML
    {
        public ZipArchive ZipArchiveFile { get; set; }

        public WordOOXML(ZipArchive zipArchiveFile)
        {
            ZipArchiveFile = zipArchiveFile;
        }

        public void SetTableCellData(int rowIndex, int columnIndex, string data)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.PreserveWhitespace = true;
                ZipArchiveEntry entry = ZipArchiveFile.Entries.Single(x => x.FullName == "word/document.xml");
                if (entry == null)
                    return;
                using (Stream entryStream = entry.Open())
                {
                    using (StreamReader reader = new StreamReader(entryStream))
                    {
                        string entryData = "";
                        xmlDocument.LoadXml(reader.ReadToEnd());
                        XmlNodeList lstWTRNode = xmlDocument.GetElementsByTagName("w:tr");
                        if (lstWTRNode != null && lstWTRNode.Count > rowIndex)
                        {
                            XmlElement wtrNode = (XmlElement)lstWTRNode[rowIndex];
                            XmlNodeList lstWTCNode = wtrNode.GetElementsByTagName("w:tc");
                            if (lstWTCNode != null && lstWTCNode.Count > columnIndex)
                            {
                                XmlElement wtcNode = (XmlElement)lstWTCNode[columnIndex];
                                XmlNodeList lstWTNode = wtcNode.GetElementsByTagName("w:t");
                                if (lstWTNode != null && lstWTNode.Count > 0)
                                {
                                    lstWTNode[0].InnerText = data;
                                    entryData = xmlDocument.OuterXml;
                                    entryStream.SetLength(0);
                                    using (StreamWriter writer = new StreamWriter(entryStream))
                                    {
                                        writer.Write(entryData);
                                        writer.Flush();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetData(string elementName, string data)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.PreserveWhitespace = true;
                ZipArchiveEntry entry = ZipArchiveFile.Entries.Single(x => x.FullName == "word/document.xml");
                if (entry == null)
                    return;
                using (Stream entryStream = entry.Open())
                {
                    using (StreamReader reader = new StreamReader(entryStream))
                    {
                        string entryData = "";
                        xmlDocument.LoadXml(reader.ReadToEnd());
                        XmlNodeList lstWTNode = xmlDocument.GetElementsByTagName("w:t");
                        if (lstWTNode != null && lstWTNode.Count > 0)
                        {
                            foreach (XmlElement wtNode in lstWTNode)
                            {
                                if (wtNode.InnerText == elementName)
                                {
                                    wtNode.InnerText = data;

                                    entryData = xmlDocument.OuterXml;
                                    entryStream.SetLength(0);
                                    using (StreamWriter writer = new StreamWriter(entryStream))
                                    {
                                        writer.Write(entryData);
                                        writer.Flush();
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertRow(int rowIndex, int rowNumber)
        {
            if (rowNumber <= 0)
                return;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.PreserveWhitespace = true;
                ZipArchiveEntry entry = ZipArchiveFile.Entries.Single(x => x.FullName == "word/document.xml");
                if (entry == null)
                    return;
                using (Stream entryStream = entry.Open())
                {
                    using (StreamReader reader = new StreamReader(entryStream))
                    {
                        string entryData = "";
                        xmlDocument.LoadXml(reader.ReadToEnd());
                        List<XmlElement> lstWTRNode = xmlDocument.GetElementsByTagName("w:tr").Cast<XmlElement>().ToList();
                        if (lstWTRNode != null && lstWTRNode.Count > rowIndex)
                        {
                            XmlElement wtrNode = lstWTRNode[rowIndex];
                            XmlElement wtblNode = (XmlElement)wtrNode.ParentNode;
                            for (int i = 0; i < rowNumber; i++)
                            {
                                wtblNode.InsertAfter(wtrNode.Clone(), wtrNode);
                            }

                            entryData = xmlDocument.OuterXml;
                            entryStream.SetLength(0);
                            using (StreamWriter writer = new StreamWriter(entryStream))
                            {
                                writer.Write(entryData);
                                writer.Flush();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
