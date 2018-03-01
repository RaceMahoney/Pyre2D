using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class MemStream : MonoBehaviour {

    private static string input;
    private static string path = "Assets/MyScripts/inputSequence.txt";
    private static List<string> input_list; //list of inputs from the file

    private string X = "0";
    private string Y = "0";
    private string JUMP = "_";
    private string DASH = "_";
    private string ATTACK = "_";
    private string TIME = "0";
    private string XPos = "0";
    private string YPos = "0";

    int count;
    byte[] byteArray;
    char[] charArray;
    UnicodeEncoding uniEncoding = new UnicodeEncoding();


    void Start()
    {
        //instanitate list
        input_list = new List<string>();
       
            //create reader and add each object into list
            StreamReader reader = new StreamReader(path);
            for (int i = 0; reader.Peek() > 0; i++)
            {
                input = reader.ReadLine();
                input_list.Add(input);

            }
            reader.Close();


        ////get the current variable values from the string item in the list based on frame count
        ////break that string into its appropriate variables to replicate
        ////every frame value from the human player
        //for (int i = 0; i < input_list.Count; i++)
        //{
        //    string line = input_list[i];
        //    string[] values = line.Split(new Char[] { ',', ',', ',', ',', ',', ',', ',', ',' }, StringSplitOptions.RemoveEmptyEntries);

        //    //save current values to variables to check this frame
        //    X = values[0];
        //    Y = values[1];
        //    JUMP = values[2];
        //    DASH = values[3];
        //    ATTACK = values[4];
        //    TIME = values[5];
        //    XPos = values[6];
        //    YPos = values[7];
        //}

        for(int i = 0; i < input_list.Count; i++)
        {
            string line = input_list[i];
           // byte[] byteInput = uniEncoding.GetBytes(line);

            using (MemoryStream memStream = new MemoryStream(100))
            {
                //write string to the stream
                //memStream.Write(byteInput, 0, byteInput.Length);

                //set the position of the stream to the begining
                memStream.Seek(0, SeekOrigin.Begin);

                //read the first 20 bytes from the stream
                byteArray = new byte[memStream.Length];
                count = memStream.Read(byteArray, 0, 20);

                // Read the remaining bytes, byte by byte.
                while (count < memStream.Length)
                {
                    byteArray[count++] =
                        Convert.ToByte(memStream.ReadByte());
                }

                //decode the byte array into a char array and write to the console
                charArray = new char[uniEncoding.GetCharCount(byteArray, 0, count)];
                uniEncoding.GetDecoder().GetChars(byteArray, 0, count, charArray, 0);

                string output = new string(charArray);
                Debug.Log(output);



            }

        }

       

        


    }


}
