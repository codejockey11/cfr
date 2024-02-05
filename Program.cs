using System;
using System.IO;
using System.Xml;

namespace CFR
{
    class Program
    {
        static String userProfile = Environment.GetEnvironmentVariable("USERPROFILE");

        static StreamWriter cfrOut;
        static StreamWriter cfrPartOut;

        static String cfrPartOutfilename;
        static String directory;
        static String title;

        static XmlDocument xdcDocument = new XmlDocument();

        static void Main(string[] args)
        {
            xdcDocument.Load(userProfile + "/Downloads/" + args[0]);
            
            XmlElement xmlRoot = xdcDocument.DocumentElement;

            title = new String(args[1].ToCharArray());
            
            directory = new String(args[2].ToCharArray());

            cfrOut = new StreamWriter(directory + "/cfr" + title + "TOC.txt");
            
            cfrPartOutfilename = directory + "/cfr" + title + "Part1.php";

            cfrPartOut = new StreamWriter(cfrPartOutfilename);

            WriteDocumentHeader(cfrOut);
            
            DoTOCIndex(xmlRoot.SelectNodes("TEXT/BODY/ECFRBRWS/DIV1"));

            DoTOC(xmlRoot.SelectNodes("TEXT/BODY/ECFRBRWS/DIV1"));
            
            cfrOut.Close();

            DoFullDoc(xmlRoot.SelectNodes("TEXT/BODY/ECFRBRWS/DIV1"));

            if (cfrPartOut != null)
            {
                cfrPartOut.Close();
            }

        }

        static void WriteDocumentHeader(StreamWriter sw)
        {
            sw.WriteLine("<style>");

            sw.WriteLine(new StreamReader("cfr.css").ReadToEnd());

            sw.WriteLine("</style>");
        }

        static void DoTOCIndex(XmlNodeList nl)
        {
            bool firstOne = true;

            foreach (XmlNode n in nl)
            {
                foreach (XmlNode d1child in n.ChildNodes)
                {
                    if (d1child.Name == "HEAD")
                    {
                        if (!firstOne)
                        {
                            cfrOut.Write("<br/>");
                        }
                        
                        firstOne = false;
                        
                        cfrOut.Write("<h1>" + d1child.InnerText.Trim() + "</h1>");
                        cfrOut.Write(cfrOut.NewLine);
                    }
                    
                    foreach (XmlNode d3child in d1child.ChildNodes)
                    {
                        if (d3child.Name == "HEAD")
                        {
                            cfrOut.Write("<h2>" + d3child.InnerText.Trim() + "</h2>");
                            cfrOut.Write(cfrOut.NewLine);
                        }
                        
                        foreach (XmlNode d4child in d3child.ChildNodes)
                        {
                            if (d4child.Name == "HEAD")
                            {
                                cfrOut.Write("<h3>" + d4child.InnerText.Trim() + "</h3>");
                                cfrOut.Write(cfrOut.NewLine);
                            }
                            
                            foreach (XmlNode d5child in d4child.ChildNodes)
                            {
                                if (d5child.Name == "HEAD")
                                {
                                    cfrOut.Write("<h4><a class=\"index\" href=\"cfr" + title + "index.php#" + d5child.InnerText.Trim() + "\">" + d5child.InnerText.Trim() + "</a></h4>");
                                    cfrOut.Write(cfrOut.NewLine);
                                }
                            }
                        }
                    }
                }
            }

        }

        static void DoTOC(XmlNodeList nl)
        {
            foreach (XmlNode n in nl)
            {
                foreach (XmlNode d1child in n.ChildNodes)
                {
                    if (d1child.Name == "HEAD")
                    {
                        cfrOut.Write("<br/><h1>" + d1child.InnerText.Trim() + "</h1>");
                        cfrOut.Write(cfrOut.NewLine);
                    }
                    
                    foreach (XmlNode d3child in d1child.ChildNodes)
                    {
                        if (d3child.Name == "HEAD")
                        {
                            cfrOut.Write("<br/><h2>" + d3child.InnerText.Trim() + "</h2>");
                            cfrOut.Write(cfrOut.NewLine);
                        }
                        
                        foreach (XmlNode d4child in d3child.ChildNodes)
                        {
                            if (d4child.Name == "DIV5")
                            {
                                if(d4child.Attributes.Count > 0)
                                {
                                    cfrPartOutfilename = "cfr" + title + "Part" + d4child.Attributes["N"].Value + ".php";
                                }
                            }
                            
                            if (d4child.Name == "HEAD")
                            {
                                cfrOut.Write("<br/><h3>" + d4child.InnerText.Trim() + "</h3>");
                                cfrOut.Write(cfrOut.NewLine);
                            }
                            
                            foreach (XmlNode d5child in d4child.ChildNodes)
                            {
                                if (d5child.Name == "HEAD")
                                {
                                    cfrOut.Write("<br/><h4><a class=\"index\" name=\"" + d5child.InnerText.Trim() + "\" href=\"" + cfrPartOutfilename + "#" + d5child.InnerText.Trim() + "\">" + d5child.InnerText.Trim() + "</a></h4>");
                                    cfrOut.Write(cfrOut.NewLine);
                                }
                                
                                foreach (XmlNode d6child in d5child.ChildNodes)
                                {
                                    if (d6child.Name == "HEAD")
                                    {
                                        cfrOut.Write("<h5><a class=\"index\" href=\"" + cfrPartOutfilename + "#" + d6child.InnerText.Trim() + "\">" + d6child.InnerText.Trim() + "</a></h5>");
                                        cfrOut.Write(cfrOut.NewLine);
                                    }
                                    
                                    foreach (XmlNode d7child in d6child.ChildNodes)
                                    {
                                        if (d7child.Name == "HEAD")
                                        {
                                            cfrOut.Write("<h6><a class=\"index\" href=\"" + cfrPartOutfilename + "#" + d7child.InnerText.Trim() + "\">" + d7child.InnerText.Trim() + "</a></h6>");
                                            cfrOut.Write(cfrOut.NewLine);
                                        }
                                        
                                        foreach (XmlNode d8child in d7child.ChildNodes)
                                        {
                                            if (d8child.Name == "HEAD")
                                            {
                                                cfrOut.Write("<h7><a class=\"index\" href=\"" + cfrPartOutfilename + "#" + d8child.InnerText.Trim() + "\">" + d8child.InnerText.Trim() + "</a></h7><br/>");
                                                cfrOut.Write(cfrOut.NewLine);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        static void WritePHPPage(String docname)
        {
            StreamWriter sw = new StreamWriter(directory + "/cfr" + title + "Part" + docname + ".php");
            
            sw.WriteLine("<?php");
            sw.WriteLine("require_once \"../includes.php\"");
            sw.WriteLine("?>");
            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<html>");
            sw.WriteLine("<head>");
            sw.WriteLine("<title>CFR Title " + title + " Part " + docname + "</title>");
            sw.WriteLine("<meta charset=\"UTF-8\">");
            sw.WriteLine("<link rel=\"shortcut icon\" href=\"../favicon.ico\">");
            sw.WriteLine("<link rel=\"stylesheet\" href=\"../base.css?v=1\">");
            sw.WriteLine("<script type=\"text/javascript\" src=\"../base.js?v=1\"></script>");
            sw.WriteLine("</head>");
            sw.WriteLine("<body>");
            sw.WriteLine("<table class=\"topPanel\"><tr><td>");
            sw.WriteLine("<?php");
            sw.WriteLine("require_once \"../navSignOn.php\";");
            sw.WriteLine("?>");
            sw.WriteLine("</td></tr></table>");
            sw.WriteLine("<table class=\"pageResult\"><tr><td>");
            sw.WriteLine("<?php");
            sw.WriteLine("$h = fopen(\"cfr" + title  + "Part" + docname + ".html\", \"r\");");
            sw.WriteLine("$r = fread($h, 4096);");
            sw.WriteLine("while(!feof($h))");
            sw.WriteLine("{");
	        sw.WriteLine("printf(\"%s\", $r);");
	        sw.WriteLine("$r = fread($h, 4096);");
            sw.WriteLine("}");
            sw.WriteLine("printf(\"%s\", $r);");
            sw.WriteLine("fclose($h);");
            sw.WriteLine("?>");
            sw.WriteLine("</td></tr></table>");
            sw.WriteLine("<table class=\"footer\"><tr><td>");
            sw.WriteLine("<?php");
            sw.WriteLine("$f = new Footer(true);");
            sw.WriteLine("?>");
            sw.WriteLine("</td></tr></table>");
            sw.WriteLine("</body></html>");

            sw.Close();

        }

        static void DoFullDoc(XmlNodeList nl)
        {
            foreach (XmlNode n in nl)
            {
                foreach (XmlNode d1child in n.ChildNodes)
                {
                    foreach (XmlNode d3child in d1child.ChildNodes)
                    {
                        foreach (XmlNode d4child in d3child.ChildNodes)
                        {
                            if (d4child.Name == "DIV5")
                            {
                                if (d4child.Attributes.Count > 0)
                                {
                                    if (cfrPartOut != null)
                                    {
                                        cfrPartOut.Close();

                                        cfrPartOutfilename = directory + "/cfr" + title + "Part" + d4child.Attributes["N"].Value + ".html";
                                        cfrPartOut = new StreamWriter(cfrPartOutfilename);
                                        WriteDocumentHeader(cfrPartOut);
                                        WritePHPPage(d4child.Attributes["N"].Value);
                                    }
                                }
                                
                                if (d4child.Name == "HEAD")
                                {
                                    cfrPartOut.Write("<h3>" + d4child.InnerText.Trim() + "</h3>");
                                    cfrPartOut.Write(cfrPartOut.NewLine);
                                }
                            }
                            
                            foreach (XmlNode d5child in d4child.ChildNodes)
                            {
                                if (d5child.Name == "HEAD")
                                {
                                    cfrPartOut.Write("<a class=\"entry\" name=\"" + d5child.InnerText.Trim() + "\"><h4 class=\"entry\">" + d5child.InnerText.Trim() + "</h4></a>");
                                    cfrPartOut.Write(cfrPartOut.NewLine);
                                }
                                
                                if (d5child.Name == "AUTH")
                                {
                                    cfrPartOut.Write(d5child.InnerText.Trim());
                                    cfrPartOut.Write(cfrPartOut.NewLine);
                                }
                                
                                if (d5child.Name == "SOURCE")
                                {
                                    cfrPartOut.Write(d5child.InnerText.Trim());
                                    cfrPartOut.Write(cfrPartOut.NewLine);
                                }
                                
                                if (d5child.Name == "HED")
                                {
                                    cfrPartOut.Write(d5child.InnerText.Trim());
                                    cfrPartOut.Write(cfrPartOut.NewLine);
                                }
                                
                                if (d5child.Name == "PSPACE")
                                {
                                    cfrPartOut.Write(d5child.InnerText.Trim());
                                    cfrPartOut.Write(cfrPartOut.NewLine);
                                }
                                
                                foreach (XmlNode d6child in d5child.ChildNodes)
                                {
                                    if (d6child.Name == "HEAD")
                                    {
                                        cfrPartOut.Write("<a class=\"entry\" name=\"" + d6child.InnerText.Trim() + "\"><h5 class=\"entry\">" + d6child.InnerText.Trim() + "</h5></a>");
                                        cfrPartOut.Write(cfrPartOut.NewLine);
                                    }
                                    
                                    if (d6child.Name == "P")
                                    {
                                        cfrPartOut.Write(d6child.OuterXml.Trim());
                                        cfrPartOut.Write(cfrPartOut.NewLine);
                                    }
                                    
                                    foreach (XmlNode d7child in d6child.ChildNodes)
                                    {
                                        if (d7child.Name == "HEAD")
                                        {
                                            cfrPartOut.Write("<a class=\"entry\" name=\"" + d7child.InnerText.Trim() + "\"><h6 class=\"entry\">" + d7child.InnerText.Trim() + "</h6></a>");
                                            cfrPartOut.Write(cfrPartOut.NewLine);
                                        }
                                        
                                        if (d7child.Name == "P")
                                        {
                                            cfrPartOut.Write(d7child.OuterXml.Trim());
                                            cfrPartOut.Write(cfrPartOut.NewLine);
                                        }
                                        
                                        foreach (XmlNode d8child in d7child.ChildNodes)
                                        {
                                            if (d8child.Name == "HEAD")
                                            {
                                                cfrPartOut.Write("<a class=\"entry\" name=\"" + d8child.InnerText.Trim() + "\"><h7 class=\"entry\">" + d8child.InnerText.Trim() + "</h7></a>");
                                                cfrPartOut.Write(cfrPartOut.NewLine);
                                            }
                                            
                                            if (d8child.Name == "P")
                                            {
                                                cfrPartOut.Write(d8child.OuterXml.Trim());
                                                cfrPartOut.Write(cfrPartOut.NewLine);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
