using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp3931Project
{
    class Wave
    {
        // HEADER = 44 bytes
        
        // RIFF Chunk
        private int ChunkID;
        private int ChunkSize;
        private int Format;

        // FORMAT Chunk
        private int FMTID;
        private int FMTSize;
        private short FMTFormatTag;
        private short FMTChannels;
        private int FMTSampleRate;
        private int FMTByteRate;
        private short FMBlock;
        private short FMTBPS;

        // DATA Chunk
        private int DataID;
        private int DataSize;
        //private Byte[]? Data;

        public Wave()
        {
        }

        public void ReadWavFile(String filename)
        {
            // byte array to hold 44 bytes of data
            byte[] fileHeader = new byte[44];

            using (BinaryReader reader = new BinaryReader(new FileStream(filename, FileMode.Open)))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                reader.Read(fileHeader, 0, 44);
            }

            // will i need to clear the wav instance?

            /* 
             *Not sure if endian matters ? Don't rmemeber it being mentioned in class but have seen it in online sources
             *http://soundfile.sapp.org/doc/WaveFormat/
             Array.Reverse(fileHeader, 4, 4); // ChunkSize
             Array.Reverse(fileHeader, 12, 4); // FMTID
             Array.Reverse(fileHeader, 16, 4); // FMTSize
             Array.Reverse(fileHeader, 20, 2); // FMTFormatTag
             Array.Reverse(fileHeader, 22, 2); // FMTChannels
             Array.Reverse(fileHeader, 24, 4); // FMTSampleRate
             Array.Reverse(fileHeader, 28, 4); // FMTByteRate
             Array.Reverse(fileHeader, 32, 2); // FMTBlock
             Array.Reverse(fileHeader, 34, 2); // FMTBPS
             Array.Reverse(fileHeader, 4, 4); // DataSize*/

            this.ChunkID = BitConverter.ToInt32(fileHeader, 0);
            this.ChunkSize = BitConverter.ToInt32(fileHeader, 4);
            this.Format = BitConverter.ToInt32(fileHeader, 8);

            this.FMTID = BitConverter.ToInt32(fileHeader, 12);
            this.FMTSize = BitConverter.ToInt32(fileHeader, 16);
            this.FMTFormatTag = BitConverter.ToInt16(fileHeader, 20);
            this.FMTChannels = BitConverter.ToInt16(fileHeader, 22);
            this.FMTSampleRate = BitConverter.ToInt32(fileHeader, 24);
            this.FMTByteRate = BitConverter.ToInt32(fileHeader, 28);
            this.FMBlock = BitConverter.ToInt16(fileHeader, 32);
            this.FMTBPS = BitConverter.ToInt16(fileHeader, 34);

            this.DataID = BitConverter.ToInt32(fileHeader, 36);
            this.DataSize = BitConverter.ToInt32(fileHeader, 40);
    }

        public void WriteWavFile(String filename, byte[] arr)
        {
            FileStream fs = new FileStream("music.wav", FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs);

            writer.Write(this.ChunkID);
            writer.Write(this.ChunkSize);
            writer.Write(this.Format);
            writer.Write(this.FMTID);
            writer.Write(this.FMTSize);
            writer.Write(this.FMTFormatTag);
            writer.Write(this.FMTChannels);
            writer.Write(this.FMTSampleRate);
            writer.Write(this.FMTByteRate);
            writer.Write(this.FMBlock);
            writer.Write(this.FMTBPS);
            writer.Write(this.DataID);
            writer.Write(this.DataSize);

            for (int i = 0; i < arr.Length; i++)
            {
                writer.Write(arr[i]);
            }

            fs.Close();

        }
    }
}
