using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class writeFile 
{
    string fileName;

    public writeFile(string s)
    { 
        fileName = s;
        createFile(s);
        
    }

    void writeData(bool append, string data)
    {
        StreamWriter writer = new StreamWriter(fileName, append);
        writer.WriteLine(data);
        writer.Close();
    }

    public void appendData(string data)
    {
        writeData(true, data);
    }

    public void createFile(string fileName)
    {
        
        StreamWriter writer = new StreamWriter(fileName, false);
        writer.Write("");
        writer.Close();
    }

    // Update is called once per frame
    
}
